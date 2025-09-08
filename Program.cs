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
    public class Program
    {
        private static List<User> users = new List<User>();

        // конфигурация
        private static Dictionary<string, object> config = new Dictionary<string, object>
        {
            { "reportType", "detailed" }
        };

        public static void ProcessUser(Dictionary<string, object> userData)
        {
            double discount;

            //Валидация данных пользователя
            if (!ValidateUserData(userData, out var error))
            {
                Console.WriteLine(error);
                return;
            }

            // расчет скидки
            discount = CalculateDiscount((bool)userData["isVIP"], (int)userData["orders"]);

            // создание пользователя
            var user = new User
            {
                Name = (string)userData["name"],
                Age = (int)userData["age"],
                Discount = discount,
            };

            // сохранение
            users.Add(user);
            Console.WriteLine("User saved: " + JsonSerializer.Serialize(user));

            // генерация отчета
            GenerateReport(
                user,
                DateTime.Now,
                (string)config["reportType"]
            );
        }

        public static void GenerateReport(User user, DateTime dt, string t)
        {
            Console.WriteLine("Starting report generation...");

            // проверка типа отчета
            if (t == "detailed")
            {
                Console.WriteLine("=== DETAILED REPORT ===");
            }
            else if (t == "summary")
            {
                Console.WriteLine("=== SUMMARY REPORT ===");
            }
            else
            {
                Console.WriteLine("=== STANDARD REPORT ===");
            }

            // форматирование заголовка
            string header = $"Report for: {user.Name}";
            Console.WriteLine(header);
            Console.WriteLine(new string('-', header.Length));

            // основная информация (дублирование формата вывода)
            Console.WriteLine($"Age: {user.Age}");
            Console.WriteLine($"Discount: {user.Discount:P0}");
            Console.WriteLine($"Active status: {(user.IsActive ? "Active" : "Inactive")}");
            Console.WriteLine($"Report date: {dt:yyyy-MM-dd HH:mm:ss}");

            // генерация раздела с деталями
            Console.WriteLine("\nDETAILS:");
            Console.WriteLine("Account information:");

            if (user.Age < 25)
            {
                if (user.Discount > 0.1)
                {
                    Console.WriteLine("Young customer with high discount");
                }
                else
                {
                    Console.WriteLine("Young customer with standard discount");
                }
            }
            else if (user.Age >= 25 && user.Age < 40)
            {
                if (user.Discount > 0.15)
                {
                    Console.WriteLine("Adult customer with premium discount");
                }
                else
                {
                    Console.WriteLine("Adult customer with standard discount");
                }
            }
            else
            {
                if (user.Discount > 0.2)
                {
                    Console.WriteLine("Senior customer with special discount");
                }
                else
                {
                    Console.WriteLine("Senior customer with standard discount");
                }
            }

            // дополнительная информация на основе статуса
            if (user.IsActive)
            {
                Console.WriteLine("Customer is currently active");
                Console.WriteLine("Last activity: " + dt.AddDays(-7).ToString("yyyy-MM-dd"));
            }
            else
            {
                Console.WriteLine("Customer is not active");
                Console.WriteLine("Account will be archived soon");
            }

            // генерация рекомендаций (искусственно сложная логика)
            Console.WriteLine("\nRECOMMENDATIONS:");
            if (!user.IsActive && user.Age < 30)
            {
                Console.WriteLine("Consider reactivation campaign for young inactive customer");
            }
            else if (user.IsActive && user.Discount < 0.05)
            {
                Console.WriteLine("Consider loyalty program for active customer with low discount");
            }
            else if (user.Age > 60 && user.Discount < 0.1)
            {
                Console.WriteLine("Consider senior discount program");
            }

            // футер отчета
            Console.WriteLine("\n" + new string('=', 50));
            Console.WriteLine("Report generated by: Automated Reporting System");
            Console.WriteLine($"Generation timestamp: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine(new string('=', 50));

            // имитация сохранения отчета в разные форматы
            if (t == "detailed")
            {
                Console.WriteLine("Saving detailed report to database...");
                // Имитация длительной операции
                System.Threading.Thread.Sleep(100);
                Console.WriteLine("Report saved successfully");
            }

            Console.WriteLine($"Report for {user.Name} completed at {DateTime.Now:HH:mm:ss}");
        }

        public static double CalculateDiscount(bool isVIP, int orders)
        {
            const double BigDiscount = 0.15;
            const double NormalDiscount = 0.1;
            const double NoDiscount = 0;

            double discount;

            if (isVIP)
            {
                discount = BigDiscount;
            }
            else if (orders > 5)
            {
                discount = NormalDiscount;
            }
            else
            {
                discount = NoDiscount;
            }

            return discount;
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

            var discount = CalculateDiscount(true, 10);
            Console.WriteLine("Calculated discount: " + discount);
        }
    }
}