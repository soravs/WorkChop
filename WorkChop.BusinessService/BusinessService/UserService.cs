using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WorkChop.BusinessService.IBusinessService;
using WorkChop.Common.ResponseViewModel;
using WorkChop.Common.ViewModel;
using WorkChop.DataModel.Models;
using WorkChop.DataModel.Repository;
using static WorkChop.Common.EnumUtil;

namespace WorkChop.BusinessService.BusinessService
{
    public class UserService : IUserService
    {
        private readonly UnitOfWork _unitOfwork;

        public UserService()
        {
            _unitOfwork = new UnitOfWork();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public User Get(int userId)
        {
            return _unitOfwork.UserRepository.Get(userId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public User Get(Guid userId)
        {
            return _unitOfwork.UserRepository.Get(userId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IQueryable<User> GetAll()
        {
            return _unitOfwork.UserRepository.GetAll();
        }

        /// <summary>
        /// Method to save user with role
        /// </summary>
        /// <param name="userVM"></param>
        /// <returns></returns>
        public User Insert(User userVM)
        {
            userVM.UserID = new Guid();
            userVM.CreatedOn = DateTime.UtcNow;
            userVM.UpdatedOn = DateTime.UtcNow;

            userVM.UserRoleMapping = new UserRoleMapping()
            {
                CreatedOn=DateTime.UtcNow,
                Fk_RoleId=(int)UserRoleEnum.Teacher
            };

            _unitOfwork.UserRepository.Add(userVM);
            return userVM;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public User GetByQuery(string email)
        {
            return _unitOfwork.UserRepository.GetByQuery(x => x.Email, email);
        }

        /// <summary>
        ///  Get User By Email/UserName
        /// </summary>
        /// <param name="email"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public UserResponseModel GetUserByRole(LoginViewModel loginVM)
        {
            var userData = new UserResponseModel();
            var userList = _unitOfwork.UserRepository.GetDbSet(x => x.Email == loginVM.UserName).ToList();

            if (userList.Count == 0)
            {
                userData.HasError = true;
                userData.ErrorMessage = "User not found";
                return userData;
            }
            AutoMapper.Mapper.CreateMap<User, UserResponseModel>();
            userData = AutoMapper.Mapper.Map<User, UserResponseModel>(userList.FirstOrDefault());
            userData.RoleResponseModel = new List<RoleResponseModel>();
            foreach (var itemData in userList)
            {
                if (itemData.UserRoleMapping != null)
                {
                    var userRoleVM = new RoleResponseModel();
                    userRoleVM.RoleId = itemData.UserRoleMapping.Fk_RoleId;
                    userRoleVM.RoleName = ((UserRoleEnum)itemData.UserRoleMapping.Fk_RoleId).ToString();
                    userData.RoleResponseModel.Add(userRoleVM);
                }
            }
            userData.HasError = false;
            userData.ErrorMessage = string.Empty;
            return userData;
        }
    }
}
