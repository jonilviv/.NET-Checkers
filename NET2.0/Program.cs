using System;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

[assembly: AssemblyCopyright("GNU GPLv3")]
[assembly: ComVisible(false)]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

public static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        if (args.Length > 0)
        {
            switch (args[0].ToLower())
            {
                case "--console":
                case "-c":
                case "/c":
                    Console.Write(Application.ProductName.PadRight(25));
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("OK");
                    Console.ResetColor();

                    return;
            }
        }
        else
        {
            Application.SetCompatibleTextRenderingDefault(false);
            Application.EnableVisualStyles();

            using (var form = new Form())
            {
                form.StartPosition = FormStartPosition.CenterScreen;
                form.Width = 400;
                form.Height = 400;
                form.MaximizeBox = false;
                form.Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);

                new Label
                {
                    Text = Application.ProductName,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font(SystemFonts.DefaultFont.FontFamily, 40, FontStyle.Bold),
                    Parent = form
                };

                Application.Run(form);
            }
        }
    }
}