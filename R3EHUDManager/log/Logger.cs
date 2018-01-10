using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace R3EHUDManager.log
{
    class Logger
    {
        private static string filePath;

        public static void Initialize(string filePath)
        {
            Logger.filePath = filePath;
        }

        public static void Clear()
        {
            if (filePath != null)
                File.WriteAllText(filePath, "");
        }

        public static void Add(string text)
        {
            if (filePath != null)
                File.AppendAllText(filePath, $"{text}{Environment.NewLine}");
        }

    }
}
