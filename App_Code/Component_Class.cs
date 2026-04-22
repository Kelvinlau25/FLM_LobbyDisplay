using System;
using System.Web;
using Microsoft.VisualBasic;

public class Component_Class
{
    /// <summary>
    /// Rectify_Date
    /// </summary>
    /// <param name="value">pass in date/datetime</param>
    /// <param name="format">optional to convert date format by default 'dd/MM/yyyy'</param>
    /// <param name="spliter_format">pass in string to split day month year</param>
    /// <param name="type">optional by default date only if pass in other value than '' will get datetime</param>
    public static string Rectify_Date(string value, string format = "dd/MM/yyyy", string spliter_format = "/", string type = "")
    {
        string d = value;

        try
        {
            if (type.Equals(""))
            {
                if (HttpContext.Current.Request.IsLocal)
                {
                    d = Strings.Mid(value, 1, 2) + spliter_format + Strings.Mid(value, 4, 2) + spliter_format + Strings.Mid(value, 7, 4);
                }
                else
                {
                    d = Strings.Mid(value, 4, 2) + spliter_format + Strings.Mid(value, 1, 2) + spliter_format + Strings.Mid(value, 7, 4);
                }
            }
            else
            {
                if (HttpContext.Current.Request.IsLocal)
                {
                    d = Strings.Mid(value, 1, 2) + spliter_format + Strings.Mid(value, 4, 2) + spliter_format + Strings.Mid(value, 7);
                }
                else
                {
                    d = Strings.Mid(value, 4, 2) + spliter_format + Strings.Mid(value, 1, 2) + spliter_format + Strings.Mid(value, 7);
                }
            }

            if (format != "dd/MM/yyyy")
            {
                d = DateTime.Parse(d).ToString(format);
            }
        }
        catch (Exception)
        {
            d = value;
        }

        return d;
    }

    public static string fn_convertMth(string mth)
    {
        string convertMth = null;
        if (mth == "JAN") convertMth = "01";
        else if (mth == "FEB") convertMth = "02";
        else if (mth == "MAR") convertMth = "03";
        else if (mth == "APR") convertMth = "04";
        else if (mth == "MAY") convertMth = "05";
        else if (mth == "JUN") convertMth = "06";
        else if (mth == "JUL") convertMth = "07";
        else if (mth == "AUG") convertMth = "08";
        else if (mth == "SEP") convertMth = "09";
        else if (mth == "OCT") convertMth = "10";
        else if (mth == "NOV") convertMth = "11";
        else if (mth == "DEC") convertMth = "12";

        return convertMth;
    }
}
