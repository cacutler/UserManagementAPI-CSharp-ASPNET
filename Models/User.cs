using System.ComponentModel.DataAnnotations;
namespace UserManagementApi.Models {
    public class User {
        public int Id {get; set;}
        [Required, MinLength(1), MaxLength(100)]
        public string FirstName {get; set;}
        [Required, MinLength(1), MaxLength(100)]
        public string LastName {get; set;}
        [Required, EmailAddress]
        public string Email {get; set;}
        [Required]
        public string Password {get; set;}
    }
}