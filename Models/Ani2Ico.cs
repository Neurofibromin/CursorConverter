using System;
using System.IO;

namespace CursorConverter.Models
{


    class Ani2Ico
    {
        /* ani2ico: C program created by TeoBigusGeekus - October 2012 *
        * Extract .ico images from .ani animated Windows cursors.   
          rewritten to c# by Neurofibromin */

        public static void Ani2IcoMain(string filepath, string outdirectory)
        {
            if (!File.Exists(filepath))
            {
                Console.Error.WriteLine("Please give an .ani filename as an argument.");
                return;
            }
            try
            {
                ReadFile(filepath, outdirectory);
            }
            catch (FileNotFoundException) { }
            catch (FileLoadException) { }
            catch (IOException) { }
            // TODO
        }

        static bool TestForIconString(byte[] buffer, int start)
        {
            return buffer[start] == 0x69 && buffer[start + 1] == 0x63 &&
                   buffer[start + 2] == 0x6f && buffer[start + 3] == 0x6e;
        }

        static void ReadFile(string name, string outdirectory)
        {
            if (!name.EndsWith(".ani", StringComparison.OrdinalIgnoreCase))
            {
                throw new FileNotFoundException("Please give an .ani filename as an argument.");
            }

            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(name);

            byte[] buffer;
            try
            {
                buffer = File.ReadAllBytes(name);
            }
            catch
            {
                throw new FileLoadException($"Unable to open file {name}.");
            }

            int fileLen = buffer.Length;
            int icoCounter = 1;
            int maxIcoCounter = 9999;
            int j = 8;

            for (int i = 0; i <= fileLen; i++)
            {
                if (icoCounter == maxIcoCounter)
                {
                    return;
                }

                if (i + 4 <= fileLen && TestForIconString(buffer, i))
                {
                    string newIcoName = $"{fileNameWithoutExtension}{icoCounter}.ico";
                    icoCounter++;

                    try
                    {
                        using (FileStream icoImage = new FileStream(Path.Combine(outdirectory, newIcoName), FileMode.Create, FileAccess.Write))
                        {
                            j = 8;
                            while (i + j + 4 <= fileLen)
                            {
                                if (TestForIconString(buffer, i + j + 1))
                                    break;
                                if (j == 10)
                                    icoImage.WriteByte(0x01);
                                else
                                    icoImage.WriteByte(buffer[i + j]);
                                j++;
                            }
                            if (i + j <= fileLen)
                                icoImage.WriteByte(buffer[i + j]);
                            if (fileLen - i - j <= 3)
                            {
                                icoImage.WriteByte(buffer[i + j + 1]);
                                icoImage.WriteByte(buffer[i + j + 2]);
                            }
                            icoImage.Flush();
                        }
                    }
                    catch
                    {
                        throw new IOException($"Unable to open file {newIcoName}.");
                    }

                    i += j;
                }
            }
        }
    }



}
