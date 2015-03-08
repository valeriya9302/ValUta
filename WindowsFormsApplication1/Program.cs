using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MainWindow
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }

    // Не логично такое делать на полудохлой стационарке
}
