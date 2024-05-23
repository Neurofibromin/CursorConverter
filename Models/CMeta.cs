using System.Text.RegularExpressions;
using Tommy;

namespace CursorConverter.Models;

class CMeta
{
    //     Meta can parse meta.hl and meta.toml
    //     based on hyprwm/hyprlang and hyprwm/hyprcursor
    bool dataPath = false;
    bool hyprlang = true;
    string rawdata;
    CMeta currentMeta;
    
    //constructor
    public CMeta(string rawdata_, bool hyprlang_ /* false for toml */, bool dataIsPath = false)
    {
        if (!dataIsPath) { return; }
        if (File.Exists(rawdata_ + ".hl"))
        {
            rawdata = rawdata_ + ".hl";
            hyprlang = true;
            return;
        }

        if (File.Exists(rawdata_ + ".toml"))
        {
            rawdata = rawdata_ + ".toml";
            hyprlang = false;
            return;
        }
        dataPath = dataIsPath;
    }

    public void Parse()
    {
        if (String.IsNullOrEmpty(rawdata))
            throw new Exception("Invalid meta (missing?)");

        string res;

        if (hyprlang)
            parseHL();
        else
            parseTOML();

        
    }

    public struct SDefinedSize
    {
        public string file;
        public int size = 0;
        public int delayMs = 200;

        public SDefinedSize()
        {
        }
    }

    public struct ParsedData
    {
        public string resizeAlgo;
        public float hotspotX = 0;
        public float hotspotY = 0;
        public List<string> overrides;
        public List<SDefinedSize> definedSizes;

        public ParsedData()
        {
        }
    }
    public ParsedData parsedData = new ParsedData ();

    void parseOverride(string V)
    {
        currentMeta.parsedData.overrides.Add (V);
    }
    void parseDefineSize(string V)
    {
        
        string VALUE = V;

        if (!VALUE.Contains(","))
        {
            throw new Exception("Invalid define_size");
        }

        string LHS = VALUE.Substring(0, VALUE.IndexOf(",")).Trim();
        string RHS = VALUE.Substring(VALUE.IndexOf(",") + 1).Trim();
        ulong DELAY = 0;

        CMeta.SDefinedSize size = new CMeta.SDefinedSize();

        if (RHS.Contains(","))
        {
            string LL = RHS.Substring(0, RHS.IndexOf(",")).Trim();
            string RR = RHS.Substring(RHS.IndexOf(",") + 1).Trim();          
            size.delayMs = Convert.ToInt32(RR);
            RHS = LL;
        }

        if (!Regex.IsMatch(RHS, "^[A-Za-z0-9_\\-\\.]+$"))
        {
            throw new Exception("Invalid cursor file name, characters must be within [A-Za-z0-9_\\-\\.] (if this seems like a mistake, check for invisible characters)");
        }

        size.file = RHS;

        if (!size.file.EndsWith(".svg"))
        {
            try
            {
                size.size = Convert.ToInt32(LHS);
            }
            catch (Exception e)
            {
                throw;
            }
        }
        else
        {
            size.size = 0;
        }

        currentMeta.parsedData.definedSizes.Add(size);

        
    }

    void parseHL()
    {
        // TODO
    }
    void parseTOML()
    {

        var MANIFEST = TOML.Parse(File.OpenText(rawdata));

        parsedData.hotspotX = MANIFEST["General"]["hotspot_x"];
        parsedData.hotspotY = MANIFEST["General"]["hotspot_y"];

        string OVERRIDES = MANIFEST["General"]["define_override"];
        string SIZES = MANIFEST["General"]["define_size"];

        
        CVarList OVERRIDESLIST = new CVarList(OVERRIDES, 0, ';', true);
        
        for (int i = 0; i < OVERRIDESLIST.size(); i++)
        {
            parseOverride(OVERRIDESLIST.argAtIndex(i));
        }

        CVarList SIZESLIST = new CVarList(SIZES, 0, ';', true);
        for (int i = 0;i < SIZESLIST.size(); i++)
        {
            parseDefineSize(SIZESLIST.argAtIndex(i));
        }

        parsedData.resizeAlgo = MANIFEST["General"]["resize_algorithm"];
    }

    
}

