using System;

public class PluginMediaFlash
{
    /// <summary>
    /// Replacement Flash Player Plugin of Tiny MCE
    /// </summary>
    public static string ConvertTo_GDDFLVPLAYER(string input)
    {
        string output = input;

        //# Change Player
        output = output.Replace("media/moxieplayer.swf", "media/gddflvplayer.swf");

        //# Add No Flash de Installer
        int idx = output.IndexOf("<param name=\"flashvars\"");
        if (idx > -1)
        {
            output = output.Insert(idx, "<a href=\"" + System.Configuration.ConfigurationManager.AppSettings["FLASHINSTALLER_FILE"].ToString() + "\"><img src=\"" + System.Configuration.ConfigurationManager.AppSettings["FLASHINSTALLER_IMAGE"].ToString() + "\" alt=\"Install Adobe Flash player\" /></a>");
        }

        //# Change flashvars path
        output = output.Replace("url=", "vdo=");

        //# Change flashvars autoplay
        output = output.Replace("flv&amp;poster=/", "flv&amp;autoplay=true");

        return output;
    }

    /// <summary>
    /// Original Flash Player Plugin of Tiny MCE
    /// </summary>
    public static string ConvertTo_MOXIEPLAYER(string input)
    {
        string output = input;

        //# Change Player
        output = output.Replace("media/gddflvplayer.swf", "media/moxieplayer.swf");

        //# Remove No Flash de Installer
        int idx_e = output.IndexOf("</a><param name=\"flashvars\"");
        if (idx_e > -1)
        {
            int idx_s = output.LastIndexOf("<a href=", idx_e);
            idx_e += 4;
            output = output.Remove(idx_s, idx_e - idx_s);
        }

        //# Change flashvars path
        output = output.Replace("vdo=", "url=");

        //# Change flashvars autoplay
        output = output.Replace("flv&amp;autoplay=true", "flv&amp;poster=/");

        return output;
    }
}
