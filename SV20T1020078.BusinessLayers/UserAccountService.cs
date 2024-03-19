using SV20T1020078.DataLayers;
using SV20T1020078.DataLayers.SQLServer;
using SV20T1020078.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1020078.BusinessLayers
{
    public static class UserAccountService
    {
        private static readonly IUserAccountDAL userAccountDB;
        static UserAccountService()
        {
            userAccountDB = new EmployeeAccountDAL(Configuration.ConnectionString);
        }
        public static UserAccount? Authorize(string userName, string password)
        {
            //TODO: Kiểm tra thông tin đăng nhập của Employee
            return userAccountDB.Authorize(userName, password);
        }
        public static bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            //TODO: Thay đổi mật khẩu của Employee
            return userAccountDB.ChangePassword(userName, oldPassword, newPassword);
        }
        public static string? GetPassword(string userName)
        {
            return userAccountDB.GetPassword(userName);
        }
    }
}
