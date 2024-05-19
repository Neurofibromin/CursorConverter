using CursorConverter.Models;
using System;
using System.Collections.Generic;
using System.IO;
using ImageMagick;

namespace CursorConverter
{


    public class Methods
    {
        public static void Ico2Png(string filepath, string outdirectory)
        {
            Console.WriteLine(filepath + outdirectory);
            Console.WriteLine(Path.Combine(outdirectory, Path.GetFileNameWithoutExtension(filepath) + ".png"));
            var image = new MagickImage(filepath);
            image.Format = MagickFormat.Png;
            image.Write(Path.Combine(outdirectory, Path.GetFileNameWithoutExtension(filepath) + ".png"));
        }



        public enum ChosenFormat
        {
            Ani, Ico, XCursor, hyprcursor, Cur, CursorFX, CurXPTheme, Png
        }

        public static List<string> AllFiles(string FolderToSerach)
        {
            string[] files = Directory.GetFiles(FolderToSerach, "*", SearchOption.AllDirectories);
            return new List<String>(files);
        }

        public static void ExecutionStarts(List<string> files, string outdirectory, ChosenFormat chosenFormat)
        {
            foreach (string file in files)
            {
                if (File.Exists(file))
                {
                    InvokeAppropriateFunction(file, outdirectory, chosenFormat);
                }
            }
        }

        public static void InvokeAppropriateFunction(string sourcefile, string outdirectory, ChosenFormat chosenFormat)
        {
            string extension = Path.GetExtension(sourcefile);
            Console.WriteLine(extension);
            extension = extension.Substring(1);
            switch (extension)
            {
                case "ani":
                    if (chosenFormat == ChosenFormat.Ico)
                    {
                        Ani2Ico.Ani2IcoMain(sourcefile, outdirectory);
                    }
                    if (chosenFormat == ChosenFormat.XCursor)
                    {
                        // aa
                    }
                    Console.WriteLine("ani");
                    break;
                case "ico":
                    if (chosenFormat == ChosenFormat.Png)
                    {
                        Ico2Png(sourcefile, outdirectory);
                    }
                    break;

                case "cur":
                    break;
                default:
                    break;
            }

            try
            {

            }
            catch (IOException)
            {
                Console.WriteLine("exception");
                return;
            }
        }

    }
}