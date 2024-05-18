using CursorConverter.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;


public class Methods
{

    public enum ChosenFormat
    {
        Ani, Ico, XCursor, hyprcursor
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

    public static void InvokeAppropriateFunction(string file, string outdirectory, ChosenFormat chosenFormat)
    {
        string extension = Path.GetExtension(file);

        switch (extension)
        {
            case "ani":
                if (chosenFormat == ChosenFormat.Ico)
                {
                    Ani2Ico.Ani2IcoMain("C:\\Users\\Ötövi\\Documents\\Forrás\\Avalonia\\CursorConverter\\dinosaur.ani", outdirectory);
                }
                if (chosenFormat == ChosenFormat.XCursor)
                {
                    // aa
                }
                Console.WriteLine("ani");
                break;
            case "ico":
                Console.WriteLine("its ico");
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