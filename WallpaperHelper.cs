using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Drawing;
using System;

namespace WallpaperShare
{
    public static class WallpaperHelper
    {
        [DllImport("user32.dll")]
        private static extern bool SetSysColors(int cElements, int[] lpaElements, uint[] lpaRgbValues);
        [DllImport("user32.dll")]
        private static extern int GetSysColor(int nIndex);

        public static void SetDesktopColor(Color desktopColor)
        {
            SetDesktopColorRegistry(desktopColor);
            SetDesktopColorMemory(desktopColor);
        }
        public static void SetDesktopColorRegistry(Color desktopColor)
        {
            //RegistryHelper.CreateRegistryValue(new RegistryValueRefrence("Computer\\HKEY_CURRENT_USER\\Control Panel\\Colors\\Background"), $"{desktopColor.R} {desktopColor.G} {desktopColor.B}", RegistryValueKind.String);
        }
        public static void SetDesktopColorMemory(Color desktopColor)
        {
            SetSysColors(1, new int[1] { 1 }, new uint[1] { (uint)ColorTranslator.ToWin32(Color.FromArgb(255, desktopColor.R, desktopColor.G, desktopColor.B)) });
        }
        public static Color GetDesktopColor()
        {
            return GetDesktopColorMemory();
        }
        public static Color GetDesktopColorMemory()
        {
            Color output = ColorTranslator.FromWin32(GetSysColor(1));
            return Color.FromArgb(255, output.R, output.G, output.B);
        }
        public static Color GetDesktopColorRegistry()
        {
            object backgroundValue = /*RegistryHelper.GetRegistryValue("Computer\\HKEY_CURRENT_USER\\Control Panel\\Colors\\Background")*/null;
            if(backgroundValue is null || backgroundValue.GetType() != typeof(string))
            {
                throw new Exception("Background color was invalid.");
            }
            string backgroundString = (string)backgroundValue;
            string[] splitBackgroundString = backgroundString.Split(' ');
            if(splitBackgroundString.Length != 3)
            {
                throw new Exception("Background color was invalid.");
            }
            return Color.FromArgb(255, int.Parse(splitBackgroundString[0]), int.Parse(splitBackgroundString[1]), int.Parse(splitBackgroundString[2]));
        }
    }
}
