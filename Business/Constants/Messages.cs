using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Business.Constants
{
    public static class Messages
    {
        public static string ProductAdded = "Ürün eklendi";
        public static string ProductNameIncalid = "Ürün ismi geçersiz";
        public static string MaintenanceTime = "Sistem bakımda";
        public static string ProductsListed = "ürünler listelendi";
        public static string ProductCountCategoryError = "Bir kategoride en fazla 10 ürün olabilir";
        public static string ProductNameAlreadyExists = "Böyle bir isim zaten var";
        internal static string CategoryLimitExceded = "Max kategori sayısına ulaşıldı";
        public static string AuthorizationDenied = "Yetkiniz Yok";
        internal static string UserRegistered="Kayıt oldu";
        public static string AccessTokenCreated="token oluşturuldu";
    }
}
