//Rextester.Program.Main is the entry point for your code. Don't change it.
//Compiler version 4.0.30319.17929 for Microsoft (R) .NET Framework 4.5

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniFileSystem
{
    class Program
    {
        static void Main(string[] args)
        {

            /* Entry point to the like command prompt screen in console application */
            Console.WriteLine("Mini File System [Version 1.0]");
            Console.WriteLine("(c) 2016 Microsoft Corporation. All rights reserved.\n");

            /* Start directory */

            string rootDir = @"Z:\";        // virtual drive Z:\ created using subst command
            CommandParser.rootDirectory = rootDir;
            string promptDisplay;

            while (true)
            {
                promptDisplay = "Z:\\" + CommandParser.rootDirectory.Substring(3) + ">";
                //Console.WriteLine("Print : "+promptDisplay[3]);

                if (promptDisplay[3] == '\\')
                {
                    // if no path is specified, back to root directory
                    promptDisplay = "Z:\\" + promptDisplay.Substring(4);
                }


                Console.Write(promptDisplay);
                string inputCommand = Console.ReadLine();

                CommandParser dirClass = new CommandParser(rootDir, inputCommand);

                Console.WriteLine("");
            }
        }
    }
}
