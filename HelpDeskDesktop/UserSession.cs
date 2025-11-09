using System;

namespace HelpDeskDesktop
{
    public static class UserSession
    {
        public static int UserId { get; set; }
        public static string UserName { get; set; }
        public static string UserEmail { get; set; }
        public static bool IsAdmin { get; set; }

        public static void Inicializar(int userId, string userName, string userEmail, bool isAdmin)
        {
            UserId = userId;
            UserName = userName;
            UserEmail = userEmail;
            IsAdmin = isAdmin;
        }

        public static void Limpar()
        {
            UserId = 0;
            UserName = string.Empty;
            UserEmail = string.Empty;
            IsAdmin = false;
        }
    }
}