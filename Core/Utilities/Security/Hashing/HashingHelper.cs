using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Security.Hashing
{
    public class HashingHelper
    {
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512()) //kullanacağımız Sınıf. "System.Security.Cryptography.HMACSHA512" -- HASING OLUSTURUR
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
        public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt) //kullanıcının gönderdiği şifreyi hash ile karşılaştıran metot. 2 hash değeri birbiri ile eşleşirse true döner. Sisteme tekrardan girdiği parola -- HASING DOGRULAR.
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt)) //salt kullanarak yapılan karşılaştırma. Bu sınıfa bu saltı kullanması gerektiğini biz söylüyoruz.
            {
                var computedHas = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHas.Length; i++)
                {
                    if (computedHas[i] != passwordHash[i]) //hash değeri ile gönderilen paraloyu karşılaştırma. 
                    {
                        return false; //eşleşmezse false.
                    }
                }
                return true; //eşleşirse...
            }
            
        }
    }
}
