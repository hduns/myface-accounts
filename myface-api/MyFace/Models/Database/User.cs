﻿using System.Collections.Generic;

namespace MyFace.Models.Database
{
    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public byte[] Salt { get; set; }
        public string ProfileImageUrl { get; set; }
        public string CoverImageUrl { get; set; }
        public Roles Role { get; set; } = Roles.MEMBER;
        public ICollection<Post> Posts { get; set; } = new List<Post>();
        public ICollection<Interaction> Interactions { get; set; } = new List<Interaction>();
    }

public enum Roles
{
    ADMIN,
    MEMBER
}
}
