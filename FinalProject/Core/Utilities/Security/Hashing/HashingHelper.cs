using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Security.Hashing
{
    public class HashingHelper
    {
        //Password hash i oluşturacak.
        //Password vereceğiz ve dışarıya hash ve salt değerlerini çıkaracağız.
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())  //kriptografi için sha512 algoritmasını kullandık.
            {
                passwordSalt = hmac.Key; //ilgili kullandığımız algoritmanın(sha512) o an oluşturduğu keydir

                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                //verdiğimiz bir password değerinin salt ve hash değerlerini oluşturduk.
            }
        }
        //Password hashini doğrula demek. 
        //Kullanıcının göndereceği passwordun hashi ile passwordhashi karşılaştırıyor. doğruysa true gönderiyor.
        //Kullanıcı sisteme tekrardan giriş yapmaya çalışıyor.
        public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt)) // daha önce oluşturduğumuz keyi passwordsalt olarak yazdık.
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)); //tekrardan girdiği parolanın hashini oluşturduk.
                for (int i = 0; i < computedHash.Length; i++)  //byte[] tiplerinini karşılaştırdık.
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
