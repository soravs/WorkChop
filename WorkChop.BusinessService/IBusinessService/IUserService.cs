using System;
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
    }
}
