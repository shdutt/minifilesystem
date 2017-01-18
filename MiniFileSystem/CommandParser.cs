//Rextester.Program.Main is the entry point for your code. Don't change it.
//Compiler version 4.0.30319.17929 for Microsoft (R) .NET Framework 4.5

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace MiniFileSystem
{
    public class CommandParser
    {
        private string command;
        private string root;
        public static string rootDirectory;

        //constructor
        public CommandParser(string root, string command)
        {
            this.root = root;
            this.command = command;
            getCommand(root, command);
        }

        /* function for getting command inputs and calling for execution */

        private void getCommand(string root, string inputCommand)
        {
            string newDirectory = "";

            //To reduce comparison statements
            inputCommand = inputCommand.ToLower();

            //To remove multiple whitespaces
            inputCommand = Regex.Replace(inputCommand, @"\s+", " ");  
                
            string[] commandArguments = inputCommand.Split(' ');

            // "CD" and "MD" coomand 
            /* Case 1 : if command is "cd" or "md" but with no path specified */
            if ((commandArguments[0] == "md" || commandArguments[0] == "cd") && commandArguments.Length < 2)
            {
                CommandExecuter.cdmdnopath();
            }

            /* Case 2 : for creating a directory with specified path - "md <full path/folder name>" */
            else if (commandArguments[0] == "md")
            {
                CommandExecuter.makeDirectory(commandArguments, newDirectory, rootDirectory);
            }

            /* Case 3 : for changing and moving between directories- "cd <full path>" */
            else if (commandArguments[0] == "cd")
            {
                CommandExecuter.changeDirectory(commandArguments);

            }

            /* "DIR" command 
            Case 1 : command input is "dir" with no path specified
            */
            else if (commandArguments[0] == "dir" && commandArguments.Length == 1)
            {
                CommandExecuter.listDirectories(rootDirectory);
            }

            /* Case 2 : command input is "dir ." */
            else if (commandArguments[0] == "dir" && commandArguments.Length == 2 && commandArguments[1] == ".")
            {
                CommandExecuter.listDirectories(rootDirectory);
            }

            /*  Case 3 : command input is "dir .." */
            else if (commandArguments[0] == "dir" && commandArguments.Length == 2 && commandArguments[1] == "..")
            {
                CommandExecuter.listPreviousDirectories(rootDirectory);
            }
            

            /* "DEL DIR" command   */
            /* Case 1 : When path is specified - "DEL DIR <path>"  */
            else if (commandArguments[0] == "del" && commandArguments.Length == 3 && commandArguments[1] == "dir")
            {
                CommandExecuter.deleteDirectory(rootDirectory, commandArguments[2]);
            }

            /* Case 2 : When path is not specified - "DEL DIR " */
            else if (commandArguments[0] == "del" && commandArguments.Length < 3 && commandArguments[1] == "dir")
            {
                Console.WriteLine(" Please specify folder or full path.\n");
            }


            /* For "help" command */
            else if (commandArguments[0] == "help" && commandArguments.Length == 1)
            {
                Console.WriteLine(" CD\t\tDisplays the name of or changes the current directory.");
                Console.WriteLine(" DIR\t\tDisplays a list of files and subdirectories in a directory.");
                Console.WriteLine(" MD\t\tCreates a directory.");
                Console.WriteLine(" DEL DIR\t\tRemoves a directory.\n");
            }
            else if (commandArguments[0] == "help" && commandArguments.Length > 1)
            {

                switch(commandArguments[1])
                {
                    case "cd"   : Console.WriteLine(" Displays the name of or changes the current directory.\n");
                                    break;

                    case "dir"  : Console.WriteLine(" Displays a list of files and subdirectories in a directory.\n");
                                    break;

                    case "md"   : Console.WriteLine(" Creates a directory.\n");
                                    break;

                    case "del"  : Console.WriteLine(" Removes a directory.\n");
                                    break;
                }
            }

            /* for clearing the console */
            else if (commandArguments[0] == "cls" || commandArguments[0] == "clear")
            {
                Console.Clear();
            }

            /* for exiting the console */
            else if (commandArguments[0] == "exit")
            {
                System.Environment.Exit(1);
            }

            /* Invalid input */
            else
            {
                Console.WriteLine("Command not found!");
            }
        }
    }
}
