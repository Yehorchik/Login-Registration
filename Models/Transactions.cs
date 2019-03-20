using System;
using System.ComponentModel.DataAnnotations;

namespace LoginRegistration.Models
{
    public class Transactions
    {
        [Key]
        public int TransId {get;set;}
        [Required]
        public decimal Amount {get; set;}
        public DateTime CreatedAt {get; set;}
        public int UserId {get;set;}
        public Users User {get;set;}

    }
    
}