using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace CSharpExam.Models
{
    public class User
    {
        [Key]
        public int UserId {get;set;}
        [Required(ErrorMessage="Please enter your name.")]
        [Display(Name="Name:")]
        [MinLength(2, ErrorMessage="Your name must be at least 2 characters.")]
        public string Name {get;set;}
        [Required(ErrorMessage="Please enter your email address.")]
        [EmailAddress(ErrorMessage="Please enter a valid email address.")]
        [Display(Name="Email Address:")]
        public string Email {get;set;}
        [Required(ErrorMessage="You must create a password to register.")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage="Your password must be at least 8 characters.")]
        [Display(Name="Password:")]
        public string Password {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
        [NotMapped]
        [Compare("Password")]
        [DataType(DataType.Password)]
        [Display(Name="Confirm Password:")]
        public string ConfirmPassword {get;set;}
        public List<Event> EventsPlanned {get;set;}
        public List<Invite> EventsAttending {get;set;}
    }
}