using System;
using System.Linq;
using WorkChop.DataModel.Repository;

namespace WorkChop.Common.ResponseViewModel
{
    public class UserRoleResponseModel
    {
        private readonly UnitOfWork _unitOfwork;

        public UserRoleResponseModel()
        {
            _unitOfwork = new UnitOfWork();
        }

        public Guid UserRoleMappingId { get; set; }
        public int Fk_RoleId { get; set; }
        public Guid Fk_UserId { get; set; }
        public DateTime CreatedOn { get; set; }
        private string _roleName;
        public string RoleName
        {
            get
            {
                return _unitOfwork.UserRoleRepository.GetDbSet(a => a.RoleId == Fk_RoleId).FirstOrDefault().RoleName;
                //if (Fk_RoleId == 1)
                //{
                //   return _roleName = "Teacher";
                //}
                //else if (Fk_RoleId == 2)
                //{
                //    return _roleName = "Student";
                //}
                //else if (Fk_RoleId == 3)
                //{
                //    return _roleName = "Admin";
                //}
                //return _roleName;
            }
            set
            {
                _roleName = value;
            }
        }
    }
}
