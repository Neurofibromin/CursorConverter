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
            Ani, Ico, XCursor, Cur, Png, Jpg, hyprcursor, CursorFX, CurXPTheme,
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
                    if (chosenFormat == Methods.AssessFileType(file))
                    {
                        //copy file without change
                        File.Copy(file, Path.Combine(outdirectory, Path.GetFileName(file)));
                        continue;
                    }
                    if (Methods.AssessFileType(file) != null)
                    {
                        InvokeAppropriateFunction(file, outdirectory, chosenFormat);    
                    }
                }
            }
        }

        public static Methods.ChosenFormat? AssessFileType(string file)
        {
            if (File.Exists(file))
            {
                string extension = Path.GetExtension(file);
                extension = Strings.LCase(extension);
                extension = extension.Substring(1);
                //Ani, Ico, XCursor, Cur, Png, Jpg, hyprcursor, CursorFX, CurXPTheme,
                switch (extension)
                {
                    case "ico":
                        return ChosenFormat.Ico;
                    case "ani":
                        return ChosenFormat.Ani;
                    case "cur":
                        return ChosenFormat.Cur;
                    case "png":
                        return ChosenFormat.Png;
                    case "jpg":
                        return ChosenFormat.Jpg;
                    case "jpeg":
                        return ChosenFormat.Jpg;
                    case "cursorfx":
                        return ChosenFormat.CursorFX;
                    case "curxptheme":
                        return ChosenFormat.CurXPTheme;
                    case "gz": //tar.gz for xcursors
                        return ChosenFormat.XCursor;
                    case "hyprcursor": //TODO: change names here for some cursors, now the checking is not working as intended
                        return ChosenFormat.hyprcursor;
                    default:
                        return null;
                }
            }
            else
            {
                return null;
            }
        }
        
        public static string ToHypr(string sourcefile)
        {
            ChosenFormat? IncomingFileFormat = Methods.AssessFileType(sourcefile);
            switch (IncomingFileFormat)
            {
                case ChosenFormat.Ico:
                    return ConverterFunctions.Ico2Hypr(sourcefile);
                case ChosenFormat.Ani:
                    return ConverterFunctions.Ani2Hypr(sourcefile);
                case ChosenFormat.Cur:
                    return ConverterFunctions.Cur2Hypr(sourcefile);
                case ChosenFormat.Png:
                    return ConverterFunctions.Png2Hypr(sourcefile);
                case ChosenFormat.Jpg:
                    return ConverterFunctions.Jpg2Hypr(sourcefile);
                case ChosenFormat.CursorFX:
                    return ConverterFunctions.Cursorfx2Hypr(sourcefile);
                case ChosenFormat.CurXPTheme:
                    return ConverterFunctions.Curxptheme2Hypr(sourcefile);
                case ChosenFormat.XCursor:
                    return ConverterFunctions.Xcursor2Hypr(sourcefile);
                case ChosenFormat.hyprcursor:
                    return sourcefile; //hypr to hypr conversion not neccessary (should not happen anyway)
                default:
                    throw new Exception("Incomingfileformat is not one of the allowed types");
            }
        }
        
        public static string FromHypr(string hyprfile, ChosenFormat chosenFormat)
        {
            switch (chosenFormat)
            {
                case ChosenFormat.Ico:
                    return ConverterFunctions.Hypr2Ico(hyprfile);
                case ChosenFormat.Ani:
                    return ConverterFunctions.Hypr2Ani(hyprfile);
                case ChosenFormat.Cur:
                    return ConverterFunctions.Hypr2Cur(hyprfile);
                case ChosenFormat.Png:
                    return ConverterFunctions.Hypr2Png(hyprfile);
                case ChosenFormat.Jpg:
                    return ConverterFunctions.Hypr2Jpg(hyprfile);
                case ChosenFormat.CursorFX:
                    return ConverterFunctions.Hypr2Cursorfx(hyprfile);
                case ChosenFormat.CurXPTheme:
                    return ConverterFunctions.Hypr2Curxptheme(hyprfile);
                case ChosenFormat.XCursor:
                    return ConverterFunctions.Hypr2Xcursor(hyprfile);
                case ChosenFormat.hyprcursor:
                    return hyprfile; //hypr to hypr conversion not neccessary (should not happen anyway)
                default:
                    throw new Exception("Outgoing file format is not one of the allowed types");
            }
        }

        public static void InvokeAppropriateFunction(string sourcefile, string outdirectory, ChosenFormat chosenFormat)
        {
            string changedtohypr = ToHypr(sourcefile);
            string correct = FromHypr(changedtohypr, chosenFormat);
            File.Copy(correct, Path.Combine(outdirectory, Path.GetFileName(correct))); //copy converted file to outfolder
        }
    }
}