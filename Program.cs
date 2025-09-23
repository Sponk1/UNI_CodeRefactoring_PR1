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

            var discount = DiscountCalculator.CalculateDiscount(
                (bool)userData["isVIP"], 
                (int)userData["orders"]
            );
            Console.WriteLine("Calculated discount: " + discount);
        }
    }
}