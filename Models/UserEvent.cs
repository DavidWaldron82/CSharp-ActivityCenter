using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System;
using System.Collections.Generic;
namespace ActCenter.Models {
    public class UserEvent {
        [Key]
        public int UserEventId{get;set;}
        public int UserId {get;set;}
        public int EventId {get;set;}
        public User User {get;set;}
        public Event Event {get;set;}
    }
}