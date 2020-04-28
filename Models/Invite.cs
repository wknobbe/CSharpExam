using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace CSharpExam.Models
{
    public class Invite
    {
        [Key]
        public int InviteId {get;set;}
        public int UserId {get;set;}
        public int EventId {get;set;}
        public User User {get;set;}
        public Event Event {get;set;}
    }
}