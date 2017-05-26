using MongoDB.Driver;
using System.Configuration;
using WorkChop.DataModel.Models;

namespace WorkChop.DataModel.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Field

        private MongoDatabase _database;

        #endregion

        #region Property

        protected IRepository<User> _users;

        protected IRepository<UserRoleMapping> _userRoleRelation;

        protected IRepository<UserRole> _userRole;

        protected IRepository<Course> _course;

        protected IRepository<UserCourseMapping> _userCourseMapping;

        protected IRepository<UserRoleMapping> _userRoleMapping;

        #endregion

        #region Methods

        public UnitOfWork()
        {
            var connectionString = ConfigurationManager.AppSettings["MongoDBConectionString"];
            var client = new MongoClient(connectionString);
            var server = client.GetServer();
            var databaseName = ConfigurationManager.AppSettings["MongoDBDatabaseName"];
            _database = server.GetDatabase(databaseName);
        }

        /// <summary>
        /// User Repository
        /// </summary>
        public IRepository<User> UserRepository
        {
            get { return _users ?? (_users = new Repository<User>(_database,"User")); }
        }
        /// <summary>
        /// 
        /// </summary>
        public IRepository<UserRoleMapping> UserRoleRelationRepository
        {
            get { return _userRoleMapping ?? (_userRoleMapping = new Repository<UserRoleMapping>(_database, "UserRoleMapping")); }
        }

        /// <summary>
        /// User Repository
        /// </summary>
        public IRepository<UserRole> UserRoleRepository
        {
            get { return _userRole ?? (_userRole = new Repository<UserRole>(_database, "UserRole")); }
        }

        /// <summary>
        /// Course Repository
        /// </summary>
        public IRepository<Course> CourseRepository
        {
            get { return _course ?? (_course = new Repository<Course>(_database, "Course")); }
        }
       
        /// <summary>
        /// User Course Repository
        /// </summary>
        public IRepository<UserCourseMapping> UserCourseMappingRepository
        {
            get { return _userCourseMapping ?? (_userCourseMapping = new Repository<UserCourseMapping>(_database, "UserCourseMapping")); }
        }
        #endregion
    }
}
