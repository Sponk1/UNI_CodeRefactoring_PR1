using System;
using System.Text.Json;

namespace RefactoringExample
{
    public class UserService
    {
        private readonly List<User> _users = new List<User>();

        public bool TryProcessUser(Dictionary<string, object> userData, out User? user)
        {
            user = null;

            if (!ValidateUserData(userData, out var error))
            {
                Console.WriteLine(error);
                return false;
            }

            var discount = DiscountCalculator.CalculateDiscount(
                (bool)userData["isVIP"],
                (int)userData["orders"]
            );

            user = new User
            {
                Name = (string)userData["name"],
                Age = (int)userData["age"],
                Discount = discount,
            };

            _users.Add(user);
            Console.WriteLine("User saved: " + JsonSerializer.Serialize(user));

            return true;
        }
        
        private static bool ValidateUserData(Dictionary<string, object> userData, out string? errorMessage)
        {
            const int MinimumUserAge = 18;
            const int LongestUserName = 100;

            // валидация возраста
            if ((int)userData["age"] < MinimumUserAge)
            {
                errorMessage = "User is too young";
                return false;
            }

            // валидация имени
            if (((string)userData["name"]).Length > LongestUserName)
            {
                errorMessage = "Name is too long";
                return false;
            }

            errorMessage = null;
            return true;
        }
    }
}