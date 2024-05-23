using Tommy;

namespace CursorConverter.Models;

class CManifest
{
    //     Manifest can parse manifest.hl and manifest.toml
    /*  path_ is the path to a manifest WITHOUT the extension.
        CManifest will attempt parsable extensions (.hl, .toml)
        based on hyprwm/hyprlang and hyprwm/hyprcursor 
    */
    eParser selectedParser;
    public ParsedData parsedData;
    string path;

    public CManifest(string path_)
    {
        parsedData = new ParsedData();
        if (File.Exists(path_ + ".hl"))
        {
            path = path_ + ".hl";
            selectedParser = eParser.PARSER_HYPRLANG;
            return;
        }
        else if (File.Exists(path_ + ".toml"))
        {
            path = path_ + ".toml";
            selectedParser = eParser.PARSER_TOML;
            return;
        }
        else { path = ""; }
    }

    public void Parse()
    {
        if (string.IsNullOrEmpty(path))
            throw new Exception("Failed to find an appropriate manifest.");
        if (selectedParser == eParser.PARSER_HYPRLANG)
        { parseHL(); }
        else if (selectedParser == eParser.PARSER_TOML)
        { parseTOML(); }
        else
        {
            throw new Exception("No parser available for " + path);
        }
    }
    public string getPath() {  return path; }

    public struct ParsedData
    {
        public string name;
        public string description;
        public string version;
        public string cursorsDirectory;
        public string author;
    }
    enum eParser
    {
        PARSER_HYPRLANG = 0,
        PARSER_TOML
    };

    void parseHL()
    {
        // TODO
        /*
        var manifest = std::make_unique<Hyprlang::CConfig>(path.c_str(), Hyprlang::SConfigOptions{.throwAllErrors = true});
            manifest->addConfigValue("cursors_directory", Hyprlang::STRING{ ""});
            manifest->addConfigValue("name", Hyprlang::STRING{ ""});
            manifest->addConfigValue("description", Hyprlang::STRING{ ""});
            manifest->addConfigValue("version", Hyprlang::STRING{ ""});
            manifest->addConfigValue("author", Hyprlang::STRING{ ""});
            manifest->commence();
            manifest->parse();
        

        parsedData.cursorsDirectory = manifest.getConfigValue("cursors_directory");
        parsedData.name = manifest.getConfigValue("name");
        parsedData.description = manifest.getConfigValue("description");
        parsedData.version = manifest.getConfigValue("version");
        parsedData.author = manifest.getConfigValue("author");
        */
        }
    void parseTOML()
    {
        TomlTable MANIFEST = TOML.Parse(File.OpenText(path));
        parsedData.cursorsDirectory = MANIFEST["General"]["cursors_directory"];
        parsedData.name = MANIFEST["General"]["name"];
        parsedData.description = MANIFEST["General"]["description"];
        parsedData.version = MANIFEST["General"]["version"];
        parsedData.author = MANIFEST["General"]["author"];
    }

    
};

