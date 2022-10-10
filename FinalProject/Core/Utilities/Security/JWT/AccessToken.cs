using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Security.JWT
{
    public class AccessToken //Erişim anahtarı
    {
        public string Token { get; set; } //tokeni string olarak tutuyoruz.
        public DateTime Expiration { get; set; } //tokenin bitiş zamanı

        //postmanda kullanıcı adı vere parolasını verecek ve bizde ona bir tane token vereceğiz ve sonlanma süresini vereceğiz.
    }
}
