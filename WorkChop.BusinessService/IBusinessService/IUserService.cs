using System;
using System.Linq;
using System.Linq.Expressions;
using WorkChop.Common.ResponseViewModel;
using WorkChop.Common.ViewModel;
using WorkChop.DataModel.Models;

namespace WorkChop.BusinessService.IBusinessService
{
    public interface IUserService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Users Get(int userId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Users Get(Guid userId);
      
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IQueryable<Users> GetAll();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="student"></param>
        void Insert(UserViewModel student);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Users GetByQuery(string email);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        UserResponseModel GetUserByRole(LoginViewModel objLoginViewModel);
            
    }
}
