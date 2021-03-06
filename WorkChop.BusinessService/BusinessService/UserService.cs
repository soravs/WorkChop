﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WorkChop.BusinessService.IBusinessService;
using WorkChop.Common.ResponseViewModel;
using WorkChop.Common.Utils;
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
        public UserResponseModel Insert(User userVM)
        {
            var isUserExists = GetByQuery(userVM.Email);
            var userData = new UserResponseModel();
            if (isUserExists != null)
            {
               
                userData.HasError = true;
                userData.ErrorMessage = "Email Already registered";
                return userData;
            }
               
            userVM.UserID = new Guid();
            userVM.CreatedOn = DateTime.UtcNow;
            userVM.UpdatedOn = DateTime.UtcNow;

            string saltKey = Security.CreateSalt();

            userVM.PasswordSalt = saltKey;
            userVM.Password = Security.Encrypt(userVM.Password, saltKey);

            _unitOfwork.UserRepository.Add(userVM);

            var userRoleMapping = new UserRoleMapping()
            {
                UserRoleMappingId = new Guid(),
                Fk_UserId = userVM.UserID,
                CreatedOn = DateTime.UtcNow,
                Fk_RoleId = (int)UserRoleEnum.Teacher
            };
            _unitOfwork.UserRoleRelationRepository.Add(userRoleMapping);



            AutoMapper.Mapper.CreateMap<User, UserResponseModel>();
            userData.HasError = false;
            userData.ErrorMessage = string.Empty;

            return userData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public User GetByQuery(string email)
        {
            //return _unitOfwork.UserRepository.GetByQuery(x => x.Email.ToLower(), email.ToLower());
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
            var getUserData = (from user in _unitOfwork.UserRepository.GetAll()
                               join roleRelation in _unitOfwork.UserRoleRelationRepository.GetAll() on user.UserID equals roleRelation.Fk_UserId
                               join role in _unitOfwork.UserRoleRepository.GetAll() on roleRelation.Fk_RoleId equals role.RoleId
                               where user.Email == loginVM.UserName 
                               select new { UserResponseModel = user, RoleResponseModel = role }).ToList();

            if (getUserData.Count > 0)
            {
                AutoMapper.Mapper.CreateMap<User, UserResponseModel>();
                userData = AutoMapper.Mapper.Map<User, UserResponseModel>(getUserData.FirstOrDefault().UserResponseModel);
                userData.RoleResponseModel = new List<RoleResponseModel>();
                foreach (var itemData in getUserData)
                {
                    AutoMapper.Mapper.CreateMap<UserRole, RoleResponseModel>();
                    var roleData = AutoMapper.Mapper.Map<UserRole, RoleResponseModel>(itemData.RoleResponseModel);
                    userData.RoleResponseModel.Add(roleData);
                }
                userData.HasError = false;
                userData.ErrorMessage = string.Empty;
                return userData;
            }
            userData.HasError = true;
            userData.ErrorMessage = "User not found";
            return userData;
        }

        public List<UserRoleResponseModel> GetUserRoleByUserId(Guid UserId)
        {
            try
            {
                var getUserRoles = _unitOfwork.UserRoleRelationRepository.GetDbSet(x => x.Fk_UserId == UserId).ToList();
                AutoMapper.Mapper.CreateMap<UserRoleMapping, UserRoleResponseModel>();
                var userRoleData = AutoMapper.Mapper.Map<List<UserRoleMapping>, List<UserRoleResponseModel>>(getUserRoles);
                return userRoleData;
            }
            catch(Exception ex)
            {
                return null;
            }
       
        }

        public List<User> GetAllTeachers(string userId)
        {
            //(from user in _unitOfwork.UserRepository.GetAll().Where(a => a.UserID.ToString() != userId)
            var getAllTeachers = (from user in _unitOfwork.UserRepository.GetAll()
                               join userMapping in _unitOfwork.UserCourseMappingRepository.GetAll() on user.UserID equals userMapping.Fk_UserId
                               where userMapping.IsAssignee
                               select user).Distinct().ToList();


            return getAllTeachers;
        }
    }
}
