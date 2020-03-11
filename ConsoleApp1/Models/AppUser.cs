using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1.Models
{
    public class AppUser
    {
        public int UserId { get; private set; }
        public string Name { get; set; }
        public bool Gender { get; set; }
        public int Age { get; set; }
        public string PhoneNumber { get; set; }
        public List<Photo> photos { get; set; }
        
        public AppUser(int userId, string name, bool gender, int age, string phoneNumber)
        {
            UserId = userId;
            Name = name;
            Gender = gender;
            Age = age;
            photos = new List<Photo>();
            PhoneNumber = phoneNumber;
        }
    }
    public class Photo
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public string GetPhysicalName => $"{UserId}_{Id.ToString()}.jpg";

        public Photo(Guid id, int userId)
        {
            Id = id;
            UserId = userId;
        }
    }
}
