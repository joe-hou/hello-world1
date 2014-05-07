using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ConsoleApplication2
{
    class Program
    {
        private static List<string> _sb;
        private static string _stringIndex;
        static void Main(string[] args)
        {
            
            _sb= new List<string>();

            DirectoryInfo di = new DirectoryInfo(args[0]);

            int counter = 0;
            string line;

            // Read the file and display it line by line.
            System.IO.StreamReader file =
               new System.IO.StreamReader(args[0] + "\\databaseList.txt");
            while ((line = file.ReadLine()) != null)
            {
                Console.WriteLine(line);
                _stringIndex = line;
                WalkDirectoryTree(di, line);
                Console.WriteLine(_stringIndex);
                _sb.Add(_stringIndex);
            }

            file.Close();

            //using (StreamWriter outfile = new StreamWriter(args[0] + @"\databaseList_updated.txt"))
            //{
            //    outfile.Write(_sb.ToString());
            //}

            using (TextWriter writer = File.CreateText(args[0] + @"\databaseList_updated.txt"))
            {
                foreach (string str in _sb)
                {
                    writer.WriteLine(str);
                }
                // etc
            }
            // Suspend the screen.
            Console.ReadLine();
        }


        static void WalkDirectoryTree(System.IO.DirectoryInfo root, string line)
        {
            System.IO.FileInfo[] files = null;
            System.IO.DirectoryInfo[] subDirs = null;

            files = root.GetFiles("*.config", SearchOption.TopDirectoryOnly);
            if (files != null)
            {
                foreach (System.IO.FileInfo fi in files)
                {
                    //Console.WriteLine(fi.FullName);
                    System.IO.StreamReader file = new System.IO.StreamReader(fi.FullName);
                    string aline;
                    while ((aline = file.ReadLine()) != null)
                    {
                        if (aline.Contains(line))
                        {
                            _stringIndex += "&&&&&&&&&";
                            _stringIndex += fi.FullName;
                            _stringIndex += "---------";
                            _stringIndex += aline;
                        }
                    }

                    file.Close();
                }

                
            }
            // Now find all the subdirectories under this directory.
            subDirs = root.GetDirectories();

            foreach (System.IO.DirectoryInfo dirInfo in subDirs)
            {
                // Resursive call for each subdirectory.
                WalkDirectoryTree(dirInfo, line);
            }
        }
    }
}
