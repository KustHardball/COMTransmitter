using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Drawing;

namespace СCOMTransmitter
{
    static class Program
    {
       
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            var form = new MainForm();

            using (NotifyIcon icon = new NotifyIcon())
            {
                icon.Icon = Icon.ExtractAssociatedIcon("Icon.ico");
                icon.ContextMenu = new ContextMenu(
                    new[]
                    {
                new MenuItem("Показать", (s, e) => form.Show()),
                new MenuItem("Скрыть", (s, e) => form.Hide()),
                new MenuItem("Выход", (s, e) => Application.Exit()),
                    });
                icon.Visible = true;

                Application.Run();
                icon.Visible = false;
            }

           
            

     
        }
    }
}
