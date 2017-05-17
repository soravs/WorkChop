using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WorkChop.BusinessService.IBusinessService;
using WorkChop.Common.ResponseViewModel;
using WorkChop.Common.ViewModel;
using WorkChop.DataModel.Models;
using WorkChop.DataModel.Repository;

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
        public Users Get(int i)
        {
            return _unitOfwork.UserRepository.Get(i);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Users Get(Guid i)
        {
            return _unitOfwork.UserRepository.Get(i);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IQueryable<Users> GetAll()
        {
            return _unitOfwork.UserRepository.GetAll();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="student"></param>
        public void Insert(UserViewModel user)
        {
            AutoMapper.Mapper.CreateMap<UserViewModel, Users>();
            var userData = AutoMapper.Mapper.Map<UserViewModel, Users>(user);
            userData.UserID = new Guid();
            userData.CreatedOn = DateTime.UtcNow;
            userData.UpdatedOn = DateTime.UtcNow;
            _unitOfwork.UserRepository.Add(userData);
            if (user.RoleId != 0)
            {
                UserRoleRelation userRelation = new UserRoleRelation
                {
                    Fk_UserId = userData.UserID,
                    Fk_RoleId = user.RoleId,
                    CreatedOn = DateTime.UtcNow
                };
                _unitOfwork.UserRoleRelationRepository.Add(userRelation);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Users GetByQuery(string email)
        {
            return _unitOfwork.UserRepository.GetByQuery(x => x.Email, email);
        }

        /// <summary>
        ///  Get User By Email/UserName
        /// </summary>
        /// <param name="email"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public UserResponseModel GetUserByRole(LoginViewModel objLoginViewModel)
        {
            var getUserData = (from user in _unitOfwork.UserRepository.GetAll()
                               join roleRelation in _unitOfwork.UserRoleRelationRepository.GetAll() on user.UserID equals roleRelation.Fk_UserId
                               join role in _unitOfwork.UserRoleRepository.GetAll() on roleRelation.Fk_RoleId equals role.RoleId
                               where user.Email == objLoginViewModel.UserName
                               select new { UserResponseModel = user, RoleResponseModel = role }).ToList();

            if (getUserData.Count > 0)
            {
                AutoMapper.Mapper.CreateMap<Users, UserResponseModel>();
                var userData = AutoMapper.Mapper.Map<Users, UserResponseModel>(getUserData.FirstOrDefault().UserResponseModel);
                userData.RoleResponseModel = new List<RoleResponseModel>();
                foreach (var itemData in getUserData)
                {
                    AutoMapper.Mapper.CreateMap<UserRole, RoleResponseModel>();
                    var roleData = AutoMapper.Mapper.Map<UserRole, RoleResponseModel>(itemData.RoleResponseModel);
                    userData.RoleResponseModel.Add(roleData);
                }
                return userData;
            }
            return null;
        }


    }
}
