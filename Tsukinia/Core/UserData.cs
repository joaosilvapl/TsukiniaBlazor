using System;

namespace Tsukinia.Core
{
    public class UserData
    {
        public string Id { get; set; }

        public string FamilyId { get; set; }

        public string Name { get; set; }

        public UserType Type { get; set; }

        public string ParentEmail { get; set; }
    }
}