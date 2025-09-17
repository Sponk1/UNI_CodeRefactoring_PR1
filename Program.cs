using System;
using System.Collections.Generic;
using System.Text.Json;

namespace RefactoringExample
{
    public class User
    {
        public required string Name { get; set; }
        public required int Age { get; set; }
        public required double Discount { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public enum ReportType
    {
        Standard,
        Summary,
        Detailed
    }
    public class Program
    {
        // private static List<User> users = new List<User>();

        // конфигурация
        private static Dictionary<string, object> config = new Dictionary<string, object>
        {
            { "reportType", ReportType.Detailed }
        };

        private static UserService userService = new UserService();  
        public static void ProcessUser(Dictionary<string, object> userData)
        {
            if (userService.TryProcessUser(userData, out var user))
            {
                ReportGenerator.Generate(
                    user!,
                    DateTime.Now,
                    (ReportType)config["reportType"]
                );
            }
        }
        
        // public static double CalculateDiscount(bool isVIP, int orders)
        // {
        //     const double BigDiscount = 0.15;
        //     const double NormalDiscount = 0.1;
        //     const double NoDiscount = 0;

        //     double discount;

        //     if (isVIP)
        //     {
        //         discount = BigDiscount;
        //     }
        //     else if (orders > 5)
        //     {
        //         discount = NormalDiscount;
        //     }
        //     else
        //     {
        //         discount = NoDiscount;
        //     }

        //     return discount;
        // }

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

        static void Main(string[] args)
        {
            var userData = new Dictionary<string, object>
            {
                { "name", "John Doe" },
                { "age", 25 },
                { "isVIP", true },
                { "orders", 10 }
            };

            ProcessUser(userData);

            // var discount = CalculateDiscount(true, 10);
            var discount = DiscountCalculator.CalculateDiscount(
                (bool)userData["isVIP"], 
                (int)userData["orders"]
            );
            Console.WriteLine("Calculated discount: " + discount);
        }
    }
}