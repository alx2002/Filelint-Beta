using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Collections;

namespace DuplicateFileFinder
{
    public class FileNameandHash
    {
        public string FileName { get; set; }
        public string FileHash { get; set; }
    }
}


namespace DuplicateFileFinder
{
    class Program
    {
        
        static void Main(string[] args)
        {
            string path;
            ConsoleKeyInfo cki;
            //identifies the console key that was pressed.
            double totalSize = 0;
            
            
            //pass directory path as argument to command line
            
            if (args.Length > 0)
                path = args[0] as string;
            else
                path = @"C:\Users\USER\Desktop\as3";
            //...Looks for a Folder named E
            
//if (path.Exists = false)
       // {
        //    Console.WriteLine(File.Exists(curFile) ?  "Folder does not exist.");
        // }
        //If folder does not exist write  "Folder does not exist."
       
        
            

            //Get all files from given directory


            var fileLists = Directory.GetFiles(path);
		
            
            int totalFiles = fileLists.Length;
			Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Files detected:{0} \n", totalFiles);
            Console.ForegroundColor = ConsoleColor.White;



            List<FileNameandHash> info = new List<FileNameandHash>();
            List<string> ToDelete = new List<string>();
            info.Clear();
            //loop through all the files by file hash code
            foreach (var item in fileLists)
            {
                using (var ReadStream = new FileStream(item, FileMode.Open, FileAccess.Read))
              	// open stream and read
                {
                    info.Add(new FileNameandHash()
                    {
                        FileName = item,
                        FileHash = BitConverter.ToString(MD5.Create().ComputeHash(ReadStream)),
                        //MD5 for speed
                    });
                }
            }
            //group by file hash code
            var SList = info.GroupBy(f => f.FileHash)
                .Select(g => new { FileHash = g.Key, Files = g.Select(z => z.FileName).ToList() });


            //keeping first item of each group as is and identify rest as duplicate files to delete
            ToDelete.AddRange(SList.SelectMany(f => f.Files.Skip(1)).ToList());
            Console.WriteLine("Duplicates detected:{0}\n", ToDelete.Count);
            Console.ForegroundColor = ConsoleColor.White;
           
            //list all files to be deleted and count total disk space to be empty after delete
            //chooses one of the files to keep, so if there is 3 (A,B,C) duplicates it keeps B and deletes A and C
            if (ToDelete.Count > 0)
            {
                Console.WriteLine("Files to be deleted:");
                foreach (var item in ToDelete)
                {
                    Console.WriteLine(item);
                    FileInfo fi = new FileInfo(item);
                    totalSize += fi.Length;
                }
            }
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nRemoved: {0}  MEGABYTE(S)\n", Math.Round((totalSize / 10000), 4).ToString());
            Console.ForegroundColor = ConsoleColor.White;
            //delete duplicate files
            if (0 < ToDelete.Count)
            	// Do this if count if greater than 0
            {
                Console.WriteLine("Press I to initilize deletion.");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Press the Q key to quit. \n");
                Console.ForegroundColor = ConsoleColor.White;
                do
                	
                {
                	Console.TreatControlCAsInput = true;
                
                    cki = Console.ReadKey();
         if((cki.Modifiers & ConsoleModifiers.Alt) != 0) Console.Write("ALT+");
         if((cki.Modifiers & ConsoleModifiers.Shift) != 0) Console.Write("SHIFT+");
         if((cki.Modifiers & ConsoleModifiers.Control) != 0) Console.Write("CTL+");
         Console.WriteLine(cki.Key.ToString());
                Console.ReadKey();
               
                    Console.WriteLine(" You pressed {0}\n", cki.Key.ToString());
                    
                    Console.ForegroundColor = ConsoleColor.White;
                    if (cki.Key == ConsoleKey.I)
                    {
                        Console.WriteLine("Deleting files... \n");
                        Console.ForegroundColor = ConsoleColor.Green;
                        ToDelete.ForEach(File.Delete);
                        Console.WriteLine("Files are deleted successfully. \n");
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    Console.WriteLine("Press Q key to quit.\n");
                } while (cki.Key != ConsoleKey.Q);
                
                
                
            }
            else
            {
            	Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\nNo files to delete.");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\nPress ENTER to exit.");
                Console.ForegroundColor = ConsoleColor.White;
                Console.ReadLine();
            }
        }
    }
}
