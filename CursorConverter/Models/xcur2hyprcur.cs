using System.IO.Compression;
using System.Text.RegularExpressions;

namespace CursorConverter.Models;

class xcur2hyprcur
{
    static eHyprcursorResizeAlgo explicitResizeAlgo = eHyprcursorResizeAlgo.HC_RESIZE_INVALID; //this does not work
    const string HYPRCURSOR_VERSION = "1.0";
    static void PromptForDeletion(string path)
    {
        bool emptyDirectory = !Directory.Exists(path);
        if (!emptyDirectory)
        {
            var files = Directory.EnumerateFiles(path);
            emptyDirectory = !files.Any(); //if no files in path emptyDirectory is true
        }
        bool ismanifesthlpresent = File.Exists(Path.Combine(path, "manifest.hl"));
        bool ismanifesttomlpresent = File.Exists(Path.Combine(path, "manifest.toml"));

        if (!ismanifesthlpresent && !ismanifesttomlpresent &&
            Directory.Exists(path) && !emptyDirectory)
        {
            throw new IOException($"Refusing to remove {path} because it doesn't look like a hyprcursor theme.\n ");
        }

        Console.WriteLine($"Deleting (recursively) {path}.");
        Directory.Delete(path, true);
    }


    static void CreateCursorThemeFromPath(string path_, string out_ = "")
    {
        if (!Directory.Exists(path_))
            throw new FileNotFoundException("input path does not exist");

        SCursorTheme currentTheme = new SCursorTheme();
        string path = Path.GetFullPath(path_);

        CManifest manifest = new CManifest(Path.Combine(path, "manifest"));
        manifest.Parse();
        string themename = manifest.parsedData.name;
        string outPath;
        if (out_ == "")
        {
            outPath = path;
        }
        else
        {
            outPath = out_;
        }

        outPath = Path.Combine(outPath, "theme_", themename);

        string cursorsSubdir = manifest.parsedData.cursorsDirectory;
        string cursorDir = Path.Combine(path, cursorsSubdir);

        if (string.IsNullOrEmpty(cursorsSubdir) || !Directory.Exists(cursorDir))
            throw new IOException("manifest: cursors_directory missing or empty");

        foreach (var dir in Directory.EnumerateDirectories(cursorDir))
        {
            if (!Regex.IsMatch(dir, "^[A-Za-z0-9_\\-\\.]+$"))
                throw new IOException($"Invalid cursor directory name at {dir} : " +
                    "characters must be within [A-Za-z0-9_\\-\\.]");

            string metaPath = Path.Combine(dir, "meta");

            var shape = new SCursorShape();


            CMeta meta = new CMeta(metaPath, true, true);
            meta.Parse();
            foreach (var i in meta.parsedData.definedSizes)
            {
                shape.images.Add(new SCursorImage(i.file, i.size, i.delayMs));
            }

            shape.overrides = meta.parsedData.overrides;

            foreach (var i in shape.images)
            {
                if (shape.shapeType == eShapeType.SHAPE_INVALID)
                {
                    if (i.filename.EndsWith(".svg"))
                        shape.shapeType = eShapeType.SHAPE_SVG;
                    else if (i.filename.EndsWith(".png"))
                        shape.shapeType = eShapeType.SHAPE_PNG;
                    else
                    {
                        Console.WriteLine($"WARNING: image {i.filename} has no known extension, assuming png.");
                        shape.shapeType = eShapeType.SHAPE_PNG;
                    }
                }
                else
                {
                    if (shape.shapeType == eShapeType.SHAPE_SVG && !i.filename.EndsWith(".svg"))
                        throw new FileNotFoundException("meta invalid: cannot add .png files to an svg shape");
                    else if (shape.shapeType == eShapeType.SHAPE_PNG && i.filename.EndsWith(".svg"))
                        throw new FileNotFoundException("meta invalid: cannot add .svg files to a png shape");
                }

                if (!File.Exists(Path.Combine(dir, i.filename)))
                    throw new FileNotFoundException($"meta invalid: image {i.filename} does not exist");
                break; // I don't think this is in the right place...
            }

            

            DirectoryInfo directoryInfo = new DirectoryInfo(dir);
            if (shape.images.Count == 0)
                throw new FileNotFoundException($"meta invalid: no images for shape {directoryInfo.Name}");
            shape.directory = directoryInfo.Name;
            shape.hotspotX = meta.parsedData.hotspotX;
            shape.hotspotY = meta.parsedData.hotspotY;
            shape.resizeAlgo = stringToAlgo(meta.parsedData.resizeAlgo);

            Console.WriteLine(
                $"Shape {shape.directory}: \n\toverrides: {shape.overrides.Count}" +
                $"\n\tsizes: {shape.images.Count}\n");
            currentTheme.shapes.Add(shape);
        }

        //create output fs structure
        if (!Directory.Exists(outPath))
        {
            Directory.CreateDirectory(outPath);
        }
        else
        {
            PromptForDeletion(outPath);
            Directory.CreateDirectory(outPath);
        }

        // manifest is copied
        File.Copy(manifest.getPath(),
            Path.Combine(outPath, $"manifest.{(manifest.getPath().EndsWith(".hl") ? "hl" : "toml")}"));

        // create subdir for cursors
        string cursorsOutDir = Path.Combine(outPath, cursorsSubdir);
        Directory.CreateDirectory(cursorsOutDir);

        // create zips (.hlc) for each
        foreach (var shape in currentTheme.shapes)
        {
            string currentCursorsDir = Path.Combine(path, cursorsSubdir, shape.directory);
            string outputFile = Path.Combine(cursorsOutDir, shape.directory + ".hlc");

            using (var zip = ZipFile.Open(outputFile, ZipArchiveMode.Create))
            {
                string metaDir = File.Exists(Path.Combine(currentCursorsDir, "meta.hl"))
                    ? Path.Combine(currentCursorsDir, "meta.hl")
                    : Path.Combine(currentCursorsDir, "meta.toml");
                zip.CreateEntryFromFile(metaDir, $"meta.{(metaDir.EndsWith(".hl") ? "hl" : "toml")}");

                // add each cursor image
                foreach (var i in shape.images)
                {
                    string imageFilePath = Path.Combine(currentCursorsDir, i.filename);
                    zip.CreateEntryFromFile(imageFilePath, i.filename);
                    Console.WriteLine($"Added image {i.filename} to shape {shape.directory}");
                }
            }

            Console.WriteLine($"Written {outputFile}");
        }

        Console.WriteLine($"Done, written {currentTheme.shapes.Count} shapes.");
    }

    static void ExtractXTheme(string xpath_, string out_)
    {
        // TODO: add xcur2png


        if (!Directory.Exists(xpath_) || !Directory.Exists(Path.Combine(xpath_, "cursors")))
        {
            throw new FileNotFoundException($"input path {xpath_} does not exist or is not an xcursor theme");
        }

        string xpath = Path.GetFullPath(xpath_);
        string outPath = string.IsNullOrEmpty(out_) ? Path.GetDirectoryName(xpath) : out_;
        outPath = Path.Combine(outPath, "/extracted_", Path.GetFileName(xpath));

        // create output fs structure
        if (!Directory.Exists(outPath))
        {
            Directory.CreateDirectory(outPath);
        }
        else
        {
            // clear the entire thing, avoid melting themes together
            PromptForDeletion(outPath);
            Directory.CreateDirectory(outPath);
        }

        // write a manifest
        using (var manifest = new StreamWriter(Path.Combine(outPath, "manifest.hl"), false))
        {
            manifest.WriteLine("name = Extracted Theme");
            manifest.WriteLine("description = Automatically extracted with hyprcursor-util");
            manifest.WriteLine("version = 0.1");
            manifest.WriteLine("cursors_directory = hyprcursors");
        }

        // make a cursors dir
        string hyprCursorsDir = Path.Combine(outPath, "hyprcursors");
        Directory.CreateDirectory(hyprCursorsDir);

        // create a temp extract dir
        string tempExtractDir = Path.Combine(Path.GetTempPath(), "hyprcursor-util");
        Directory.CreateDirectory(tempExtractDir);

        // write all cursors
        foreach (var xcursor in Directory.EnumerateDirectories(Path.Combine(xpath, "cursors")))
        {
            // ignore symlinks, we'll write them to the meta.hl file.
            // TODO
            string cursorDir = Path.Combine(outPath, "hyprcursors", Path.GetDirectoryName(xcursor));
            Directory.CreateDirectory(cursorDir);
            Console.WriteLine($"Found xcursor {Path.GetDirectoryName(xcursor)}");
            System.IO.DirectoryInfo deleteAllHere = new DirectoryInfo(tempExtractDir);
            foreach (FileInfo file in deleteAllHere.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in deleteAllHere.GetDirectories())
            {
                dir.Delete(true);
            }
            // decompile xcursor
            string command = $"cd /tmp/hyprcursor-util && xcur2png '{Path.GetFullPath(xcursor)}' -d /tmp/hyprcursor-util 2>&1";

            List<XCursorConfigEntry> entries = new List<XCursorConfigEntry>();
            // read the config
            string configPath = Path.Combine(tempExtractDir, $"{Path.GetFileNameWithoutExtension(xcursor)}.conf");

            if (!File.Exists(configPath))
            {
                throw new Exception($"Failed reading xconfig for {xcursor} at {configPath}");
            }

            foreach (var line in File.ReadLines(configPath))
            {
                if (line.StartsWith("#"))
                    continue;
                //parse
                try
                {
                    XCursorConfigEntry entry = new XCursorConfigEntry();

                    var parts = line.Split('\t');
                    entry.size = int.Parse(parts[0]);
                    entry.hotspotX = int.Parse(parts[1]);
                    entry.hotspotY = int.Parse(parts[2]);
                    entry.image = parts[3];
                    entry.delay = int.Parse(parts[4]);

                    entries.Add(entry);

                    Console.WriteLine($"Extracted {Path.GetFileNameWithoutExtension(xcursor)} at size {entry.size}");
                }
                catch
                {
                    throw;
                }
            }

            if (entries.Count == 0)
            {
                throw new Exception("Empty xcursor");
            }

            // copy pngs
            foreach (var extracted in Directory.EnumerateFiles(tempExtractDir))
            {
                if (extracted.EndsWith(".conf"))
                    continue;

                File.Copy(extracted, Path.Combine(cursorDir, Path.GetFileName(extracted)), overwrite: true);
            }

            // write a meta.hl
            string metaString = $"resize_algorithm = {(explicitResizeAlgo == eHyprcursorResizeAlgo.HC_RESIZE_INVALID ? "none" : algoToString(explicitResizeAlgo))}\n";

            // find hotspot from first entry
            metaString += $"hotspot_x = {(float)entries[0].hotspotX / entries[0].size:F2}\n" +
                $"hotspot_y = {(float)entries[0].hotspotY / entries[0].size:F2}\n\n"; // seems fishy

            // define all sizes
            foreach (var entry in entries)
            {
                string entryStem = Path.GetFileName(entry.image);
                //maybe? string entryStem = Path.GetFileName(entry.image.Substring(entry.image.IndexOf('\')+1)));
                metaString += $"define_size = {entry.size}, {entryStem}, {entry.delay}\n";
            }

            metaString += "\n";

            // define overrides, scan for symlinks

            foreach (var xcursor2 in Directory.EnumerateFiles(Path.Combine(xpath, "cursors")))
            {
                var fileInfo2 = new FileInfo(xcursor2);
                if ((File.GetAttributes(fileInfo2.FullName) & FileAttributes.ReparsePoint) == 0)
                    continue;

                if (Path.GetFullPath(xcursor2) != Path.GetFullPath(xcursor))
                    continue;

                // this sym points to us
                metaString += $"define_override = {Path.GetFileNameWithoutExtension(fileInfo2.Name)}\n";
            }

            File.WriteAllText(Path.Combine(cursorDir, "meta.hl"), metaString);
        }


        Directory.Delete(tempExtractDir, true);
    }

    static eHyprcursorResizeAlgo stringToAlgo(string s)
    {
        if (s == "none") { return eHyprcursorResizeAlgo.HC_RESIZE_NONE; }
        if (s == "nearest") { return eHyprcursorResizeAlgo.HC_RESIZE_NEAREST; }
        return eHyprcursorResizeAlgo.HC_RESIZE_BILINEAR;
    }

    static string algoToString(eHyprcursorResizeAlgo a)
    {
        switch (a)
        {
            case eHyprcursorResizeAlgo.HC_RESIZE_BILINEAR: return "bilinear";
            case eHyprcursorResizeAlgo.HC_RESIZE_NEAREST: return "nearest";
            case eHyprcursorResizeAlgo.HC_RESIZE_NONE: return "none";
            default: return "none";
        }

        return "none";
    }

    public static void Executor(string mode, string inpath, string outPath, string resizealgo)
    {
        eOperation op = eOperation.OPERATION_CREATE;
        if (mode == "create") { op = eOperation.OPERATION_CREATE; }
        else if (mode == "extract") { op = eOperation.OPERATION_EXTRACT; }
        eHyprcursorResizeAlgo explicitResizeAlgo = stringToAlgo(resizealgo); // TODO: resizealgo is not used during operation
        switch (op)
        {
            case eOperation.OPERATION_CREATE:
                {
                    CreateCursorThemeFromPath(inpath, outPath);
                    break;
                }
            case eOperation.OPERATION_EXTRACT:
                {
                    ExtractXTheme(inpath, outPath);
                    break;
                }
        }
    }

}

