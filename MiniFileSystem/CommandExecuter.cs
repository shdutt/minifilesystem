//Rextester.Program.Main is the entry point for your code. Don't change it.
//Compiler version 4.0.30319.17929 for Microsoft (R) .NET Framework 4.5


/* Use hash data structure for implementing file system */
/* Add validation to command */
/* Add validation for folder name */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniFileSystem
{
    public class CommandExecuter
    {
      
        /* 
           Method - makeDirectory() for creating a new directory, if already exists, display message 
           This method will be called only when input command is "md <full path/folder name>"
           This method can be extended for creating hidden directories.
           This method can also be extended for creating a file
         */
        
        internal static void makeDirectory(string[] commandArguments, string newDir, string rootDir)
        {

            try
            {
                /* Case 1 : if input is "md Z:" */
                if (commandArguments[1][0] == 'z' && commandArguments[1][1] == ':')
                {
                    //throwing exception if input is -> Z:\>md Z:
                    commandArguments[1] = commandArguments[1].Substring(3);
                }

                // creating path for new directory 
                    newDir = rootDir + "\\" + commandArguments[1];

                /*  Directory.CreateDirectory(Path.Combine(PathToParent, SubDirectoryName));  */
                // Check if already exists or not
                if (Directory.Exists(newDir))
                {
                    Console.WriteLine(" The given path already exists.\n\n");
                    return;
                }

                Directory.CreateDirectory(newDir);
                Console.WriteLine("The directory was created successfully at {0}.\n\n", Directory.GetCreationTime(newDir));

                /* If I want to create a hidden directory
                  if (!Directory.Exists(path)) 
                  { 
                       DirectoryInfo di = Directory.CreateDirectory(path); 
                       di.Attributes = FileAttributes.Directory | FileAttributes.Hidden; 
                  }
                        
                 
                 */
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}\n\n", e.ToString());
            }
        }
        /* End of makeDirectory method */


       /*
        * Method changeDirectory() - for changing and moving between directories
        * This method will be called when input command is : "cd ." or "cd .."
        * This method can be extended for "cd * operations" eg. cd docu* , cd p* etc.
       */
        internal static void changeDirectory(string[] CommandArguments)
        {
            try
            {
                /* Case 1: when input is - "cd .." */
                if (CommandArguments[1] == "..")
                {
                    //fetching parent directory
                    CommandParser.rootDirectory = Directory.GetParent(CommandParser.rootDirectory).ToString();

                }

                /* Case 2: when input is - "cd ." or "cd *" "cd .*" */
                else if (CommandArguments[1] == "." || CommandArguments[1] == "*" || CommandArguments[1] == ".*")
                {
                    // nothing to be done.
                }

                /* Case 3 : when input command is - "cd /" */
                else if (CommandArguments[1] == "/")
                {
                    CommandParser.rootDirectory = "Z:\\";
                    Console.WriteLine("");
                }

                
                /* Case 4: when input is - "cd <full path/folder name>" */
                else
                {
                    if (CommandArguments[1][0] == 'z' && CommandArguments[1][1] == ':')
                    {
                        CommandArguments[1] = CommandArguments[1].Substring(2);
                    }

                    String currentDir = CommandParser.rootDirectory + "\\" + CommandArguments[1];
                    if (Directory.Exists(currentDir))
                    {
                        // moving to specified directory
                        CommandParser.rootDirectory = currentDir;
                        Console.WriteLine("");
                    }
                    else
                    {
                        Console.WriteLine(CommandArguments[1] + " does not exist. Please check the directory name.\n");
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error : "+ex.Message+"\n\n");
            }
        }

        /* End of changeDirectory() method */

        /* Method listDirectories() for showing all the directories inside the given path
         * This method will be called when input command is "dir " or "dir . "
         * This method can be extended for hidden files. 
         * */
        internal static void listDirectories(string rootDirectory)
        {
            try
            {
                // Enumerate - efficient when huge number of folders and files. Else .GetDirectories can also be used.

                List<string> dirs = new List<string>(Directory.EnumerateDirectories(rootDirectory));
                Console.WriteLine("\n  Volume in drive Z is OSDisk\n  Volume Serial Number is B897 - DAF8");
                Console.WriteLine("\n Directory of " + rootDirectory + "\n");

                foreach (var dir in dirs)
                {
                    DateTime dt = Directory.GetCreationTime(dir);  // getting directory creation date and time
                    Console.Write(dt);
                    Console.WriteLine("\t<DIR>\t{0}", dir.Substring(dir.LastIndexOf("\\") + 1));

                }

                // using DirectoryInfo for getting directories and files

                DirectoryInfo directory = new DirectoryInfo(rootDirectory);
                FileInfo[] filePaths = directory.GetFiles();
                foreach (FileInfo str in filePaths)
                {
                     DateTime dt = str.CreationTime;  // getting file creation date and time
                     Console.Write(dt);
                     Console.WriteLine("\t\t" + str);
                }

                Console.WriteLine("\t   {0} Dir(s)", dirs.Count);
                Console.WriteLine("\t   {0} Files(s)\n", directory.GetFiles().Length);

                /* For showing all files excluding the hidden files 
                 * 
                 * 
                 * DirectoryInfo directory = new DirectoryInfo(rootDirectory);
                   FileInfo[] files = directory.GetFiles();

                    var filtered = files.Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden));

                    foreach (var f in filtered)
                     {
                    Console.WriteLine(f+"\t"+f.Length);
                     }
                */

                /* DirectoryInfo di = new DirectoryInfo(rootDirectory);
                     var files = di.EnumerateFiles("*", SearchOption.AllDirectories)
                    .Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden) && !f.Attributes.HasFlag(FileAttributes.System))
                    .GroupBy(f => f.DirectoryName);
                */


            }
            catch (UnauthorizedAccessException UAEx)
            {
                Console.WriteLine(UAEx.Message);
            }
            catch (PathTooLongException PathEx)
            {
                Console.WriteLine(PathEx.Message);
            }
        }

        /* End of listDirectories() method */

         /* This method will list directories when input command is "dir .." */

        internal static void listPreviousDirectories(string rootDirectory)
        {
            // Case 1 :  if already root directory ie Z:\ then you just have to list directories of Z:\
            if (rootDirectory == "C:\\Z Drive" || rootDirectory == "C:\\Z Drive\\")
            {
                listDirectories(rootDirectory);
            }

            // Case 2 : if inside some child directory, "dir .." will list parent's directories
            else
            {
                try
                {
                    List<string> dirs = new List<string>(Directory.EnumerateDirectories(Directory.GetParent(rootDirectory).ToString()));
                    Console.WriteLine("\n  Volume in drive Z is OSDisk\n  Volume Serial Number is B897 - DAF8");
                    foreach (var dir in dirs)
                    {
                        DateTime dt = Directory.GetCreationTime(dir);
                        Console.Write(dt);
                        Console.WriteLine("\t<DIR>\t{0}", dir.Substring(dir.LastIndexOf("\\") + 1));
                        //Console.WriteLine("\t"+dir);

                    }

                    // using DirectoryInfo for getting directories and files

                    DirectoryInfo directory = new DirectoryInfo(rootDirectory);
                    FileInfo[] filePaths = directory.GetFiles();
                    foreach (FileInfo str in filePaths)
                    {
                        DateTime dt = str.CreationTime;  // getting file creation date and time
                        Console.Write(dt);
                        Console.WriteLine("\n\t\t\t\t" + str);
                    }

                    Console.WriteLine("\t   {0} Dir(s)", dirs.Count);
                    Console.WriteLine("\t   {0} Files(s)\n", directory.GetFiles().Length);

                }
                catch (UnauthorizedAccessException UAEx)
                {
                    Console.WriteLine(UAEx.Message);
                }
                catch (PathTooLongException PathEx)
                {
                    Console.WriteLine(PathEx.Message);
                }
            }
        }

        /* End of listPreviousDirectories */

        /* method  for checking if directory is empty or not
        public static bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        */

        /* Method deleteDirectory() will be called for deleting the directory using command - "DEL DIR <folder name/full path>" */
        internal static void deleteDirectory(string rootDirectory, string folderName)
        {
            // if directly deleting parent directory, then exception needs to be handled later.
            if (folderName[0] == 'z' && folderName[1] == ':')
            {
                folderName = folderName.Substring(3);
            }

            if (Directory.Exists(rootDirectory + "\\" + folderName))
            {
                Directory.Delete(rootDirectory + "\\" + folderName, true);
                Console.WriteLine("Directory deleted!\n");
            }
            else
            {
                Console.WriteLine(folderName + " does not exist. Please check the directory name.\n");
            }
        }

        /* End of deleteDirectory() */

        /* Method will be called when no path is specified with "cd" or "md" command */
        public static void cdmdnopath()
        {
            Console.WriteLine("Please specify folder or full path\n\n");
            return;
        }
    }
}
