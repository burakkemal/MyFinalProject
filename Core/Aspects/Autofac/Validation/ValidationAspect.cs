using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Interceptors;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Aspects.Autofac.Validation
{
    public class ValidationAspect : MethodInterception //Aspect - metodun başında-sonunda veya neresinde çalışmasını istersek çalışır.
    {
        private Type _validatorType;
        public ValidationAspect(Type validatorType)
        {
            //defensive cod - denir tipi doğru alma amacı ile 
            if (!typeof(IValidator).IsAssignableFrom(validatorType)) 
            {
                throw new System.Exception("Bu bir doğrulama sınıfı değil");
            }

            _validatorType = validatorType;
        }
        protected override void OnBefore(IInvocation invocation)
        {
            var validator = (IValidator)Activator.CreateInstance(_validatorType);  //çalışma anında instance oluşturma. validatör tipi
            var entityType = _validatorType.BaseType.GetGenericArguments()[0]; //argümanlarından 0. olanın tipini yakala.
            var entities = invocation.Arguments.Where(t => t.GetType() == entityType); //metodun argümanlarını gez eğer ordaki tip yukarıda yakaladığım tipe eşit mi?
            // eşitse validate et.
            foreach (var entity in entities)
            {
                ValidationTool.Validate(validator, entity);
            }
        }
    }
}
