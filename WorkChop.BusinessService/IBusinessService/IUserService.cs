using System;
using System.Collections.Generic;
using System.Linq;
using WorkChop.Common.ResponseViewModel;
using WorkChop.Common.ViewModel;
using WorkChop.DataModel.Models;

namespace WorkChop.BusinessService.IBusinessService
{
    public interface IUserService
    {
        User Get(int userId);
        User Get(Guid userId);
        IQueryable<User> GetAll();
        User Insert(User userVM);
        User GetByQuery(string email);
        UserResponseModel GetUserByRole(LoginViewModel loginVM);
        List<UserRoleResponseModel> GetUserRoleByUserId(Guid UserId);
        List<User> GetAllTeachers(string userId);
    }
}
