using RainCustomers.API.Models;
using System.Collections.Generic;

namespace RainCustomers.UnitTests.Fixtures;


public static class UsersFixture
{
    public static List<User> GetTestUsers() =>
        new()
        {
            new User
            {
                Name = "Test User 1",
                Email = "test.user.1@example.com",
                Address = new Address
                {
                    Street = "123 Market St",
                    City = "Somewhere",
                    ZipCode = "213124"
                }
            },
            new User
            {
                Name = "Test User 2",
                Email = "test.user.2@example.com",
                Address = new Address
                {
                    Street = "900 Main St",
                    City = "Somewhere",
                    ZipCode = "123456",
                }
            },
            new User
            {
                Name = "Test User 3",
                Email = "test.user.3@example.com",
                Address = new Address
                {
                    Street = "515 Maroon st",
                    City = "Yokohama",
                    ZipCode = "234456",
                }
            },
        };

}