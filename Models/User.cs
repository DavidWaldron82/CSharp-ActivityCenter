using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System;
using System.Collections.Generic;
namespace ActCenter.Models {
    public class User {
        [Key]
        public int UserId {get;set;}
        [Required]
        [MinLength(2)]
        [RegularExpression(@"[\p{L} ]+$", ErrorMessage="Name must consist of letters and spaces only")]
        public string Name {get;set;}
        [Required]
        [EmailAddress]
        public string Email {get;set;}
        [DataType(DataType.Password)]
        [Required(ErrorMessage="Password is required")]
        [MinLength(8, ErrorMessage="Password must be at least 8 characters!")]
        
        public string Password {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
        [NotMapped]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword {get;set;}
        public List<UserEvent> Joined {get;set;}
    }
}