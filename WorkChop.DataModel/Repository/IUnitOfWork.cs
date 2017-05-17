using WorkChop.DataModel.Models;

namespace WorkChop.DataModel.Repository
{
    public interface IUnitOfWork
    {
        #region Properties
        /// <summary>
        /// UserRepository
        /// </summary>
        IRepository<Users> UserRepository { get; }
        /// <summary>
        /// User Role Relation Repository
        /// </summary>
        IRepository<UserRoleRelation> UserRoleRelationRepository { get; }
        /// <summary>
        /// User Role Repository
        /// </summary>
        IRepository<UserRole> UserRoleRepository { get; }
        #endregion
    }
}
