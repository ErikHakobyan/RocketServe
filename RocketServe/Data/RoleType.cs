using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.Text;

namespace RocketServe.Data
{
    /// <summary>
    /// Roles in DB
    /// </summary>
    public static class RoleType
    {
        public const string Admin = "admin";
        public const string Manager = "manager";
        public const string BussinessAdmin = "bussiness_admin";
        public const string BussnessManager = "bussiness_manager";
        public const string Employee = "employee";


        /// <summary>
        /// Return all available roles
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> GetRoles()
        {
            return typeof(RoleType).GetFields(BindingFlags.Instance |
                BindingFlags.Public | BindingFlags.Static).Select(f => f.GetValue(f).ToString());
        }

        /// <summary>
        /// adding roles together with commas
        /// </summary>
        /// <param name="roles">Roles to be added</param>
        /// <returns></returns>
        public static string Select(params string[] roles)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var item in roles)
            {
                builder.Append($",{item}");
            }
            return builder.ToString();
        }

        public static string SelectAll()
        {
            List<string> roles = new List<string>();
            foreach (var item in GetRoles())
            {
                roles.Add(item);
            }
            return Select(roles.ToArray());
        }
        public static string GetAdmins()
        {
            return Select(Admin, BussinessAdmin);
        }
    }
}
