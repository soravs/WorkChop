using System;
using System.ComponentModel.DataAnnotations;

namespace WorkChop.Common.ViewModel
{
    public class UserViewModel
    {
        public Guid UserID { get; set; }
        [Required]
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string Password { get; set; }
        public Nullable<Guid> CreatedBy { get; set; }
        public int RoleId { get; set; }
    }
}
