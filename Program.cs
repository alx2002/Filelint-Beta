using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;


namespace DuplicateFileFinder
{
    public class organizer
    {
        public string FileName { get; set; }
        public string FileHash { get; set; }
        //public static string GetDirectoryName (string path);
    }
}
 
    namespace DuplicateFileFinder
    {
        public class Program
        {

            static void Main(string[] args)
            {
          
            
            	{
                string path;
                ConsoleKeyInfo cki;
                //identifies the console key that was pressed.
                decimal totalSize = 0;
                //double

              
                //pass directory path as argument to command line
                Console.WriteLine("Example --C:/users/testing/desktop/photoalbum2018");
                if (args.Length > 0)
                    path = args[0] as string;
                
                else
                	Console.WriteLine("Enter a path:");
                
               
                
                  path = Console.ReadLine();


                Console.Clear();
		
                if (!Directory.Exists(path))
                {
               
                    Console.WriteLine(" <{0}/> is not a valid file or directory.", path);
                }

                if (Directory.Exists(path))
                {
                    Console.WriteLine("Directory Location: {0} ", path);

                }
    
                else if (Directory.Exists (path))

                	
                {
                    Console.WriteLine(" <{0}> is not a valid file or directory.", path);
                    
                }


                var fileLists = Directory.GetFiles(path, "*", SearchOption.AllDirectories);


                // will send you here if path is invalid.

                int totalFiles = fileLists.Length;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Files detected:{0} \n", totalFiles);
                Console.ForegroundColor = ConsoleColor.White;

                List<organizer> info = new List<organizer>();
                List<string> ToDelete = new List<string>();
                info.Clear();
                // <Summary>
                // Iterate through all the files by file hash code
                // <Summary/>
                foreach (var item in fileLists)
                {
                    using (var ReadStream = new FileStream(item, FileMode.Open, FileAccess.Read))
                    // open stream and read
                    {
                        info.Add(new organizer()
                        {
                            FileName = item,
                            FileHash = BitConverter.ToString(MD5.Create().ComputeHash(ReadStream)),
                   
                            //Hash MD5 for each file
                        });
                    }
                }
                //group  by file hash code
                var SList = info.GroupBy(first => first.FileHash)
                    .Select(g => new { FileHash = g.Key, Files = g.Select(z => z.FileName).ToList() });


                //keeping first item of each group as is and identify rest as duplicate files usedb to delete
                ToDelete.AddRange(SList.SelectMany(first => first.Files.Skip(1)).ToList());
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
                
                Console.WriteLine("\nWill remove: {0}  mbytes\n", Math.Round((totalSize / 10000), 4).ToString());
                Console.ForegroundColor = ConsoleColor.White;
                //delete duplicate files
                if (0 < ToDelete.Count)
                // do this if count if greater than 0
                {
                    Console.WriteLine("Press I to initilize deletion.");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Press the Q key to quit. \n");
                    Console.ForegroundColor = ConsoleColor.White;
                    do

                    {
                        Console.TreatControlCAsInput = true;
                        //Provides info about key pressed
                        cki = Console.ReadKey();

                        Console.WriteLine(cki.Key.ToString());
                        Console.ReadKey();

                        //Console.WriteLine(" You pressed {0}\n", cki.Key.ToString());

                        Console.ForegroundColor = ConsoleColor.Green;
                        if (cki.Key == ConsoleKey.I)
                        	
                        {
                            Console.WriteLine("Deleting files... \n");
                            Console.ForegroundColor = ConsoleColor.Green;
                            ToDelete.ForEach(File.Delete);
                            Console.WriteLine("{0} files deleted successfully. \n", ToDelete.Count);
                            Console.ForegroundColor = ConsoleColor.Green;
                        }
                        Console.WriteLine("Press Q key to quit.\n");
                    } while (cki.Key != ConsoleKey.Q);

                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("\nNo duplicates to delete.");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("\nPress ENTER to exit.");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.ReadLine();
                }
            }
        }
    }
}
