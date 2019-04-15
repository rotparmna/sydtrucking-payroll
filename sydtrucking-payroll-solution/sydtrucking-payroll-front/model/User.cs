namespace sydtrucking_payroll_front.model
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using System;
    using System.Collections.Generic;

    public class User
    {
        public User()
        {
            LastLogin = new DateTime();
            Roles = new List<Role>();
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public Int64 SocialSecurity { get; set; }
        public string Password { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public DateTime LastLogin { get; set; }
        public bool IsActive { get; set; }
        public List<Role> Roles { get; set; }
    }
}
