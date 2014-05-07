using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        private static List<string> _excludeFolders;
        private static string _targetFolder;
        private static string _sourceDrive;
        private static string _beginingChar;
        static void Main(string[] args)
        {
            _excludeFolders = new List<string>();
            _excludeFolders.Add(@"\DTDManage\Data");
            _excludeFolders.Add(@"\Data\Binary");
            _excludeFolders.Add(@"\System\Data");
            _excludeFolders.Add(@"\Postcards");
            _excludeFolders.Add(@"\bin\");
            _excludeFolders.Add(@"\assets\");
            _excludeFolders.Add(@"\asset\");
            _excludeFolders.Add(@"\databaseupdate");
            _targetFolder = args[1];
            _sourceDrive = args[2];
            //_beginingChar = args[3];
            // Start with drives if you have to search the entire computer. 
            //string[] drives = System.Environment.GetLogicalDrives();

            //foreach (string dr in drives)
            //{
            //    System.IO.DriveInfo di = new System.IO.DriveInfo(dr);

            //    // Here we skip the drive if it is not ready to be read. This 
            //    // is not necessarily the appropriate action in all scenarios. 
            //    if (!di.IsReady)
            //    {
            //        Console.WriteLine("The drive {0} could not be read", di.Name);
            //        continue;
            //    }
            //    System.IO.DirectoryInfo rootDir = di.RootDirectory;
            //    WalkDirectoryTree(rootDir);
            //}

            DirectoryInfo di = new DirectoryInfo(args[0]);
            WalkDirectoryTree(di);
            // Write out all the files that could not be processed.
            Console.WriteLine("Files with restricted access:");

            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key");
            Console.ReadKey();
        }


        static void WalkDirectoryTree(System.IO.DirectoryInfo root)
        {
            char start = root.FullName[0];
            
            foreach (var ef in _excludeFolders)
            {
                if (root.FullName.ToLower().Contains(ef.ToLower())) return;
            }
            
            System.IO.FileInfo[] files = null;
            System.IO.DirectoryInfo[] subDirs = null;

            // First, process all the files directly under this folder 
            try
            {
                files = root.GetFiles("*.config");
            }
            // This is thrown if even one of the files requires permissions greater 
            // than the application provides. 
            catch (UnauthorizedAccessException e)
            {
                // This code just writes out the message and continues to recurse. 
                // You may decide to do something different here. For example, you 
                // can try to elevate your privileges and access the file again.
                //log.Add(e.Message);
            }

            catch (System.IO.DirectoryNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }

            if (files != null)
            {
                foreach (System.IO.FileInfo fi in files)
                {
                    Console.WriteLine(fi.FullName);
                    var fileDic = fi.Directory.FullName;
                    var newFileDic = _targetFolder + fileDic.Replace(_sourceDrive + @":\", "");
                    Console.WriteLine(newFileDic);
                    if (!Directory.Exists(newFileDic))
                    {
                        DirectoryInfo di = Directory.CreateDirectory(newFileDic);
                    }
                    var newFile = _targetFolder + fi.FullName.Replace(_sourceDrive + @":\", "");
                    File.Copy(fi.FullName, newFile);
                }

                // Now find all the subdirectories under this directory.
                subDirs = root.GetDirectories();

                foreach (System.IO.DirectoryInfo dirInfo in subDirs)
                {
                    // Resursive call for each subdirectory.
                    WalkDirectoryTree(dirInfo);
                }
            }
        }
    }
}
