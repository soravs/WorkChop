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

        protected IRepository<Users> _users;

        protected IRepository<UserRoleRelation> _userRoleRelation;

        protected IRepository<UserRole> _userRole;

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
        public IRepository<Users> UserRepository
        {
            get { return _users ?? (_users = new Repository<Users>(_database,"Users")); }
        }

        /// <summary>
        /// User Repository
        /// </summary>
        public IRepository<UserRole> UserRoleRepository
        {
            get { return _userRole ?? (_userRole = new Repository<UserRole>(_database, "UserRole")); }
        }

        /// <summary>
        /// UserRole Repository
        /// </summary>
        public IRepository<UserRoleRelation> UserRoleRelationRepository
        {
            get { return _userRoleRelation ?? (_userRoleRelation = new Repository<UserRoleRelation>(_database, "UserRoleRelation")); }
        }
        #endregion
    }
}
