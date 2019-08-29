using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace NETX
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            using (var form = new Form())
            {
                form.StartPosition = FormStartPosition.CenterScreen;
                form.Width = 400;
                form.Height = 400;
                form.MaximizeBox = false;
                form.Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);

                var lbl = new Label
                {
                    Text = ".NET ",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font(SystemFonts.DefaultFont.FontFamily, 40, FontStyle.Bold),
                    Parent = form
                };

                string xxx = IntPtr.Size == 8 ? "x64" : "x86";

#if NET20
                lbl.Text += "2.0";
#elif NET30
                lbl.Text += "3.0";
#elif NET35
                lbl.Text += "3.5";
#elif NET40
                lbl.Text += "4.0";
#elif NET45
                lbl.Text += "4.5";
#elif NET451
                lbl.Text += "4.5.1";
#elif NET452
                lbl.Text += "4.5.2";
#elif NET46
                lbl.Text += "4.6";
#elif NET461
                lbl.Text += "4.6.1";
#elif NET462
                lbl.Text += "4.6.2";
#elif NET47
                lbl.Text += "4.7";
#elif NET471
                lbl.Text += "4.7.1";
#elif NET472
                lbl.Text += "4.7.2";
#elif NET48
                lbl.Text += "4.8";
#else
                lbl.Text +="Unknown";
#endif
                lbl.Text += "\n" + xxx;
                lbl.Text += "\n" + Assembly.GetExecutingAssembly().ImageRuntimeVersion;

                Application.Run(form);
            }
        }
    }
}