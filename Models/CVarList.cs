using System.Text.RegularExpressions;

namespace CursorConverter.Models;

class CVarList 
{
    /** Split string into arg list
        lastArgNo stop splitting after argv reaches maximum size, last arg will contain rest of unsplit args
        delim if delimiter is 's', use std::isspace
        removeEmpty remove empty args from argv
        based on hyprwm/hyprlang and hyprwm/hyprcursor
    */
    List<string> m_vArgs;
    
    public CVarList(string input, int lastArgNo = 0, char delim = ',', bool removeEmpty = false)
    {
        m_vArgs = new List<string>();
        if (string.IsNullOrEmpty(input))
        {
            m_vArgs.Add("");
        }

        string args = input;
        int idx = 0;
        int pos = 0;

        if (delim == 's') //shady, original code is a mess // TODO
        {
            args = Regex.Replace(args, @"\s", "\0"); //if delimiter is "s" use spaces?
        }
        else
        {
            args = args.Replace(delim, '\0');
        }

        foreach (var s in args.Split('\0'))
        {
            if (removeEmpty && string.IsNullOrEmpty(s))
                continue;
            if (++idx == lastArgNo)
            {
                m_vArgs.Add((input.Substring(pos)).Trim());
                break;
            }
            pos += s.Length + 1;
            m_vArgs.Add(s.Trim());
        }
    }

    public int size() { return m_vArgs.Count; }

    string join(string joiner, int from = 0, int to = 0) {
        int last;
        if (to == 0) {  last = size(); }
        else { last = to; }

        string rolling = "";
        for (int i = from; i < last; ++i)
        {
            rolling += m_vArgs[i] + (i + 1 < last ? joiner : "");
        }
        return rolling;
    }

    public void append(string arg)
    {
        m_vArgs.Add(arg);
    }

    public string argAtIndex(int idx) {
        if (idx >= m_vArgs.Count) { return "";}
        else { return m_vArgs[idx]; }
    }

    bool contains(string testedString)
    {
        foreach(var a in m_vArgs) 
        {
            if (a == testedString)
                return true;
        }

        return false;
    }
}

