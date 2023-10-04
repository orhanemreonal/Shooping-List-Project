namespace shopping_list.Models.Role
{
    public static class Roles
    {
        public static readonly string Admin = "Admin";
        public static readonly string User = "User";
        public static readonly List<string> RoleList = new List<string>
        {
            Admin, User
        };
    }
}
