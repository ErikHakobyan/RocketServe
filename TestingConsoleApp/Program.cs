using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TestingConsoleApp
{
    public static class RoleType
    {
        public const string Admin = "admin";
        public const string RestaurantAdmin = "restaurant_admin";
        public const string Manager = "manager";
        public const string Worker = "worker";

        public static IEnumerable<string> GetRoles()
        {
            return typeof(RoleType).GetFields(BindingFlags.Instance |
                BindingFlags.Public | BindingFlags.Static).Select(f => f.GetValue(f).ToString());
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            string a = "test";
            string b = "test";
            Console.WriteLine(a.Equals(b));

            int c = 1;
            int d = 1;
            Console.WriteLine(c.Equals(d));
            Console.ReadLine();
        }
    }
}
