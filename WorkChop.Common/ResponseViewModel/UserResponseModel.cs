using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkChop.DataModel.Models;

namespace WorkChop.Common.ResponseViewModel
{
    public class UserResponseModel: BaseViewModel
    {
        public Guid UserID { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public Nullable<Guid> CreatedBy { get; set; }
        public List<RoleResponseModel> RoleResponseModel { get; set; }
        public AccessToken TokenModel { get; set; }
    }
    public class RoleResponseModel
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
    public class AccessToken
    {
        public string access_token { get; set; }

        public string token_type { get; set; }

        public string expires_in { get; set; }

        public string userName { get; set; }

        public string Roles { get; set; }
    }
}
