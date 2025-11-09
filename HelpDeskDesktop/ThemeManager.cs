using System.Drawing;
using System.Collections.Generic;

namespace HelpDeskDesktop
{
    public static class ThemeManager
    {
        public static bool IsDarkMode { get; private set; } = false;

        public static class LightTheme
        {
            public static Color Primary = Color.FromArgb(31, 78, 121);
            public static Color Secondary = Color.FromArgb(52, 152, 219);
            public static Color Background = Color.FromArgb(245, 245, 245);
            public static Color CardBackground = Color.White;
            public static Color Text = Color.Black;          
            public static Color TextMuted = Color.Gray;
            public static Color Sidebar = Color.FromArgb(31, 78, 121); 
            public static Color SidebarText = Color.White;   
            public static Color Success = Color.FromArgb(46, 204, 113);
            public static Color Warning = Color.FromArgb(241, 196, 15);
            public static Color Danger = Color.FromArgb(192, 57, 43);
        }

        public static class DarkTheme
        {
            public static Color Primary = Color.FromArgb(86, 156, 214);
            public static Color Secondary = Color.FromArgb(66, 133, 244);
            public static Color Background = Color.FromArgb(30, 30, 30);
            public static Color CardBackground = Color.FromArgb(45, 45, 45);
            public static Color Text = Color.White;         
            public static Color TextMuted = Color.LightGray;
            public static Color Sidebar = Color.FromArgb(25, 25, 25);
            public static Color SidebarText = Color.White;   
            public static Color Success = Color.FromArgb(56, 214, 123);
            public static Color Warning = Color.FromArgb(255, 206, 35);
            public static Color Danger = Color.FromArgb(255, 77, 77);
        }

        public static Color GetSidebarTextColor()
        {
            return Color.White; 
        }

        public static void ToggleTheme()
        {
            IsDarkMode = !IsDarkMode;
        }

        public static Color GetBackgroundColor()
        {
            return IsDarkMode ? DarkTheme.Background : LightTheme.Background;
        }

        public static Color GetTextColor()
        {
            return IsDarkMode ? DarkTheme.Text : LightTheme.Text;
        }

        public static Color GetPrimaryColor()
        {
            return IsDarkMode ? DarkTheme.Primary : LightTheme.Primary;
        }

        public static Color GetSidebarColor()
        {
            return IsDarkMode ? DarkTheme.Sidebar : LightTheme.Sidebar;
        }

        public static Color GetCardBackgroundColor()
        {
            return IsDarkMode ? DarkTheme.CardBackground : LightTheme.CardBackground;
        }
    }
}