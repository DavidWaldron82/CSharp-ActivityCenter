using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System;
using System.Collections.Generic;
namespace ActCenter.Models {
    public class ViewModel {
        public UserEvent OneUserEvent {get;set;}
        public User OneUser { get; set; }
        public Event OneEvent { get; set; }
        List<User> AllUsers {get; set;}
        public List<Event> AllEvents { get; set; }
        public List<UserEvent> Joined{get;set;}
    }}