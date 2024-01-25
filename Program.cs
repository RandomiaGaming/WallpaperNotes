using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Drawing;
using Microsoft.Win32;


namespace WallpaperShare
{
    public static class Program
    {
        //0 means no error.
        //1 means too many or too few arguments.
        public static int Main(string[] args)
        {
            RegistryHelper.Yeet();

            Bitmap wallpaper = new Bitmap(192, 108);
            for (int x = 0; x < wallpaper.Width; x++)
            {
                for (int y = 0; y < wallpaper.Height; y++)
                {
                    if ((x + y) % 2 == 0)
                    {
                        wallpaper.SetPixel(x, y, Color.AliceBlue);
                    }
                }
            }
            SetWallpaper(wallpaper, Style.Stretch);
            return 0;
            if (args is null || args.Length is 0)
            {
                args = new string[1] { "/Run" };
            }

            if (!(args[0] is null) && args[0].ToLower() is "/?")
            {
                Console.WriteLine("Usage: WallpaperShare.exe [command] [optional:arguments]");
                Console.WriteLine("Specifying no arguments is the same as specifying /Run");
                Console.WriteLine("Arguments are not case sensitive, and therfore /RUN is the same as /run");
                Console.WriteLine("Valid arguments are as follows:");
                Console.WriteLine("     /? - Displays this help screen.");
                Console.WriteLine("     /Run - Starts the wait loop and begins checking periodically for new wallpapers.");
                Console.WriteLine("     /SetDelay [delay] - Sets the delay between between checking for new wallpapers.");
                Console.WriteLine("     /SetUsername [username] - Sets the imap username to be used when checking for new wallpapers.");
                Console.WriteLine("     /SetToken [token] - Sets the imap account access token used when checking for new wallpapers. Usually your token is the same as your email password.");
                Console.WriteLine("     /Install [optional:installDirecory] - Installs WallpaperShare to the specified folder. If left blank then the install location defaults to %ProgramFiles%\\WallpaperShare.");
                Console.WriteLine("     /Uninstall [optional:keepSettings] - Uninstalls WallpaperShare and removes all changed settings including setting the users wallpaper back to a system default.");
                Console.WriteLine("     /Debug - Prints a bunch of debug info to the console such as installation status and current wallpaper message.");
            }
            else if (!(args[0] is null) && args[0].ToLower() is "/run")
            {
/*                Bitmap wallpaper = new Bitmap(1920, 1080);
                Graphics wallpaperGraphics = Graphics.FromImage(wallpaper);
                wallpaperGraphics.Clear(Color.CornflowerBlue);
                wallpaperGraphics.DrawString("Minnie's a cutie!", new Font(FontFamily.GenericSansSerif, 100.0f, FontStyle.Regular), Brushes.Black, new PointF(0, 0), StringFormat.GenericTypographic);
                SetWallpaper(wallpaper, Style.Fill);*/
            }
            else if (!(args[0] is null) && args[0].ToLower() is "/setdelay")
            {

            }
            else if (!(args[0] is null) && args[0].ToLower() is "/setusername")
            {

            }
            else if (!(args[0] is null) && args[0].ToLower() is "/settoken")
            {

            }
            else if (!(args[0] is null) && args[0].ToLower() is "/install")
            {

            }
            else if (!(args[0] is null) && args[0].ToLower() is "/uninstall")
            {

            }
            else if (!(args[0] is null) && args[0].ToLower() is "/debug")
            {

            }
            else
            {
                Console.WriteLine("Invalid command line arguments. For usage type WallpaperShare /?");
                return 1;
            }
            return 0;
        }
        /*
        public static Font GetBestFitFont(this Graphics graphics, string s, Font font, Brush brush, Rectangle rect, StringFormat formatString)
        {
            return null;
        }
        public static void DrawText(this Graphics graphics, string s, Font font, Brush brush, Rectangle rect, StringFormat formatString)
        {
            if(font is null)
            {
                font = new Font(FontFamily., float emSize, FontStyle style, GraphicsUnit unit, byte gdiCharSet, bool gdiVerticalFont);
            }
            Graphics g = this.CreateGraphics();

            float width = ((float)this.ClientRectangle.Width);
            float height = ((float)this.ClientRectangle.Width);

            float emSize = height;

            font = FindBestFitFont(g, letter.ToString(), font, this.ClientRectangle.Size);

            SizeF size = g.MeasureString(letter.ToString(), font);
            g.DrawString(letter, font, new SolidBrush(Color.Black), (width - size.Width) / 2, 0);

        }
        private Font FindBestFitFont(Graphics g, String text, Font font, Size proposedSize)
        {
            // Compute actual size, shrink if needed
            while (true)
            {
                SizeF size = g.MeasureString(text, font);

                // It fits, back out
                if (size.Height <= proposedSize.Height &&
                     size.Width <= proposedSize.Width) { return font; }

                // Try a smaller font (90% of old size)
                Font oldFont = font;
                font = new Font(font.Name, (float)(font.Size * .9), font.Style);
                oldFont.Dispose();
            }
        }
        */
        public const int SPI_SETDESKWALLPAPER = 20;
        public const int SPIF_UPDATEINIFILE = 0x01;
        public const int SPIF_SENDWININICHANGE = 0x02;
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);
        public enum Style : int
        {
            Fill,
            Fit,
            Span,
            Stretch,
            Tile,
            Center
        }
        public static void SetWallpaper(Image wallpaper, Style style)
        {
            if (wallpaper is null)
            {
                throw new Exception("wallpaper cannot be null.");
            }

            string wallpaperPath = Path.GetDirectoryName(typeof(WallpaperHelper).Assembly.Location) + "Wallpaper.png";
            wallpaper.Save(wallpaperPath, ImageFormat.Png);

            RegistryKey key = Registry.CurrentUser.OpenSubKey("Control Panel\\Desktop", true);

            if (style == Style.Fill)
            {
                key.SetValue(@"WallpaperStyle", 10.ToString());
                key.SetValue(@"TileWallpaper", 0.ToString());
            }
            if (style == Style.Fit)
            {
                key.SetValue(@"WallpaperStyle", 6.ToString());
                key.SetValue(@"TileWallpaper", 0.ToString());
            }
            if (style == Style.Span)
            {
                key.SetValue(@"WallpaperStyle", 22.ToString());
                key.SetValue(@"TileWallpaper", 0.ToString());
            }
            if (style == Style.Stretch)
            {
                key.SetValue(@"WallpaperStyle", 2.ToString());
                key.SetValue(@"TileWallpaper", 0.ToString());
            }
            if (style == Style.Tile)
            {
                key.SetValue(@"WallpaperStyle", 0.ToString());
                key.SetValue(@"TileWallpaper", 1.ToString());
            }
            if (style == Style.Center)
            {
                key.SetValue(@"WallpaperStyle", 0.ToString());
                key.SetValue(@"TileWallpaper", 0.ToString());
            }

            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, wallpaperPath, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
        }
    }
}