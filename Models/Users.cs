using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginRegistration.Models
{
    public class Users
    {
        [Key]
        public int UserId {get; set;}
        [Required]
        [MinLength(3)]
        public string FirstName {get; set;}

        [Required]
        [MinLength(3)]
        public string LastName {get; set;}

        [EmailAddress]
        [Required]
        public string Email {get; set;}

        [DataType(DataType.Password)]
        [Required]
        [MinLength(8, ErrorMessage="Password must be 8 characters or longer!")]
        public string Password {get; set;}
        public DateTime CreatedAt {get;set;}
        public DateTime UpdatedAt {get;set;}

        [NotMapped]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string Confirm {get;set;}

        public List<Transactions> Transactions{get ;set;}


    }
}