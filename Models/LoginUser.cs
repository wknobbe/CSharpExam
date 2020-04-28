using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace CSharpExam.Models
{
    public class LoginUser
    {
        [Required(ErrorMessage="Please enter your email address.")]
        [EmailAddress(ErrorMessage="Please enter a valid email address.")]
        [Display(Name="Email Address:")]
        public string Email {get;set;}
        [Required(ErrorMessage="Please enter a password.")]
        [Display(Name="Password:")]
        public string Password {get;set;}
    }
}