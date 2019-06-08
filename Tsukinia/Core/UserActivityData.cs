using System;

namespace Tsukinia.Core
{
    public class UserActivityData
    {
        public string UserId {get;set;}
        public string ActivityId {get;set;}
        public ActivityType ActivityType {get;set;}
        public string ActivityTitle {get;set;}
        public string ActivityComments {get;set;}
        public int Points {get;set;}
        public DateTime DateUtc {get;set;}
    }
}