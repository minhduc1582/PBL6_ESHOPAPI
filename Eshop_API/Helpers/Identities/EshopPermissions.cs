using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eshop_pbl6.Helpers.Identities
{
    public static class EshopPermissions
    {
        public const string GroupName = "EshopApp";

        public static class ProductPermissions
        {
            public const string Default = GroupName + ".Products";
            public const string Get = Default + ".Get";
            public const string Add = Default + ".Add";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }
        public static class OrderPermissions
        {
            public const string Default = GroupName + ".Orders";
            public const string Get = Default + ".Get";
            public const string Add = Default + ".Add";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
            public const string GetList = Default + ".GetList";
        }
        public static class UserPermissions
        {
            public const string Default = GroupName + ".Users";
            public const string Get = Default + ".Get";
            public const string Add = Default + ".Add";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }
        public static class CategoryPermissions
        {
            public const string Default = GroupName + ".Categories";
            public const string Get = Default + ".Get";
            public const string Add = Default + ".Add";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }
        // not add
        public static class ManagerPermissions
        {
            public const string Default = GroupName + ".Permissions";
            public const string Get = Default + ".Get";
            public const string Add = Default + ".Add";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }
        public static class Statis
        {
            public const string Default = GroupName + ".Statis";
            public const string Get = Default + ".Get";
        }
    }
}