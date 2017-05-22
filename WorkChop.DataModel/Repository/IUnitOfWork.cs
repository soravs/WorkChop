using WorkChop.DataModel.Models;

namespace WorkChop.DataModel.Repository
{
    public interface IUnitOfWork
    {
        #region Properties
        /// <summary>
        /// User Repository
        /// </summary>
        IRepository<User> UserRepository { get; }

        /// <summary>
        /// User Role Repository
        /// </summary>
        IRepository<UserRole> UserRoleRepository { get; }

        /// <summary>
        /// Course Repository
        /// </summary>
        IRepository<Course> CourseRepository { get; }

        /// <summary>
        /// User Course Mapping Repository
        /// </summary>
        IRepository<UserCourseMapping> UserCourseMappingRepository { get; }
        #endregion
    }
}
