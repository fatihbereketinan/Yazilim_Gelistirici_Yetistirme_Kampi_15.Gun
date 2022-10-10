using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Security.JWT
{
    public interface ITokenHelper  
    {
        AccessToken CreateToken(User user, List<OperationClaim> operationClaims);
        //Bu bizim token oluşturacak mekanizmamız.
        //İlgili kullanıcı için ilgili kullanıcıların claimlerini içerecek bir token üretecek bize.
    }
}
