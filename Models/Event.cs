using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System;
using System.Collections.Generic;
namespace ActCenter.Models {
    public class Event {
        [Key]
        public int EventId {get;set;}
        [Required(ErrorMessage="What is the name of your event")]
        public string EventName {get;set;}
        [Required]
        public int Duration {get;set;}
        public int UserId {get;set;}
        public User CreatedBy{get;set;}
        public List<UserEvent> Joined {get;set;}
        [Required]
        public DateTime D {get;set;}
        [Required]
        public string Hr {get;set;}
        [Required]
        public string Description {get;set;}
        public DateTime CreatedAt { get; set;} = DateTime.Now;
        public DateTime UpdatedAt { get; set;} = DateTime.Now;
    }
}