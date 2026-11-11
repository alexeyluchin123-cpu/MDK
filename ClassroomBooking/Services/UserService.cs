using ClassroomBooking.Models;
using System.Collections.Generic;

namespace ClassroomBooking.Services
{
    public class UserService
    {
        private readonly List<User> users = new List<User>
        {
            new User { Id = 1, Name = "Иван Петров", Login = "petrov", Role = "Преподаватель" },
            new User { Id = 2, Name = "Анна Смирнова", Login = "smirnova", Role = "Преподаватель" },
            new User { Id = 3, Name = "Администратор", Login = "admin", Role = "Администратор" }
        };

        public List<User> GetUsers()
        {
            return users;
        }
    }
}