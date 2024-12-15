using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerNode.Middleware
{
    public class UserContextFeature
    {
        public string Email { get; }
        public IEnumerable<string> Roles { get; }

        public UserContextFeature(string email, IEnumerable<string> roles)
        {
            Email = email;
            Roles = roles;
        }
    }
}
