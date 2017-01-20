using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace BEE2
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                //MessageBox.Show(Environment.CurrentDirectory);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
            catch (Exception ex)
            {
                File.AppendAllText("mainexceptions.txt", "\n\n======================================================================\n\n" + ex.ToString());
            }
        }
    }
}
