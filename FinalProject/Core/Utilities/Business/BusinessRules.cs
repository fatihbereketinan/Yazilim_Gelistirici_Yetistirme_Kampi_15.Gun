using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Business
{
    public class BusinessRules
    {
        public static IResult Run(params IResult[] logics)
        {   //params yazdığımız zaman istedğimiz kadar IResult paremetresi verebiliyoruz.
            //İş motoru yazdık ve buraya iş kurallarımızı göndereceğiz.
            foreach (var logic in logics)
            {
                if (!logic.Success) //logic'lerin succes durumunu kontrol ediyor
                {
                    return logic;  //başarısızsa errorresult gönderir.
                }
            }
            return null;
        }
    }
}
