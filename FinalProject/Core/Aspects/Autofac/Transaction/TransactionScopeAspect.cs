using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace Core.Aspects.Autofac.Transaction
{
    public class TransactionScopeAspect : MethodInterception
    {
        public override void Intercept(IInvocation invocation) //invocation benim metodum.
        {
            using (TransactionScope transactionScope = new TransactionScope()) //.NET Core'dan geliyor
            {
                try
                {
                    invocation.Proceed();
                    transactionScope.Complete();
                }
                catch (System.Exception e)
                {
                    //İşlem Başarısızsa işlemi db'de geri alır
                    transactionScope.Dispose();
                    throw;
                }
            }
        }
    }
}
