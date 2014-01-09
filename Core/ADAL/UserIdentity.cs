using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy.Security;

namespace Core.ADAL
{
    public class UserIdentity : IUserIdentity
    {
        public UserIdentity()
        { }
        public UserIdentity(Microsoft.IdentityModel.Clients.ActiveDirectory.UserInfo userInfo)
        {
            UserName = userInfo.UserId;
            // ignore Claims for now -- haven't received any help from the Nancy boys on how this should be used
            // to map claims provided via ADAL
        }

        public string UserName { get; set; }

        public IEnumerable<string> Claims { get; set; }
    }
}
