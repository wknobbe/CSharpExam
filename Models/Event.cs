using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace CSharpExam.Models
{
    public class Event
    {
        [Key]
        public int EventId {get;set;}
        [Required(ErrorMessage="Please enter a title for the event.")]
        [Display(Name="Title:")]
        public string Title {get;set;}
        [Required(ErrorMessage="Please enter a time.")]
        [Display(Name="Time:")]
        public string Time {get;set;}
        [Required]
        [Display(Name="Date:")]
        [DataType(DataType.Date)]
        [Future]
        public DateTime Date {get;set;}
        [Required(ErrorMessage="Please enter a description.")]
        [Display(Name="Description:")]
        public string Description {get;set;}
        [Required(ErrorMessage="Please enter a duration.")]
        [Display(Name="Duration:")]
        public int DurationAmount {get;set;}
        [Required(ErrorMessage="Please enter a duration.")]
        public string DurationType {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
        public int UserId {get;set;}
        public User Planner {get;set;}
        public List<Invite> Participants {get;set;}
    }
    public class FutureAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value == null)
                {
                    return new ValidationResult("Please enter a date for the wedding.");
                }
                else
                {
                    DateTime compare;
                    if (value is DateTime)
                    {
                        compare = (DateTime)value;
                        if (compare < DateTime.Now)
                        {
                            return new ValidationResult("Please enter a future date.");
                        }
                        else
                        {
                            return ValidationResult.Success;
                        }
                    }
                    else
                    {
                        return new ValidationResult("Please enter a valid date.");
                    }
                }
            }
        }
}