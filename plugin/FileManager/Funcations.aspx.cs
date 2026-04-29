using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Configuration;
using System.Text;


public partial class FileManager_Funcations : System.Web.UI.Page
{
    public String RootDirectory_Path = "";
    public String RootDirectory_Url = "";


    protected void Page_Load(object sender, EventArgs e)
    {
        // Customize
        RootDirectory_Path = ConfigurationManager.AppSettings["FILESERVER_PATH"].ToString();
        RootDirectory_Url = ConfigurationManager.AppSettings["FILESERVER_URL"].ToString();
        if (Session["FILESERVER_PATH"] != null)
        {
            RootDirectory_Path = Session["FILESERVER_PATH"].ToString().Trim();
        }
        if (Session["FILESERVER_URL"] != null)
        {
            RootDirectory_Url = Session["FILESERVER_URL"].ToString().Trim();
        }
        FileServerTransfer.ConfirmDirectory(RootDirectory_Path);



        // AuthenticateFileManager();
        Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetNoStore();
        if (Request["funcation"] != null)
        {
            if (Request["funcation"].ToString() == "LoadFiles")
            {
                if (string.IsNullOrEmpty(Request["Path"].ToString()) || Request["Path"].ToString() == "/" || Request["Path"].ToString() == "/UserFiles/")
                {
                    //LoadFiles(ConfigurationManager.AppSettings["FileManager"].ToString());
                    LoadFiles(RootDirectory_Url);
                }
                else
                {
                    LoadFiles(Request["Path"].ToString());
                }
            }


            if (Request["funcation"].ToString() == "FilesULLI")
            {
                if (string.IsNullOrEmpty(Request["Path"].ToString()) || Request["Path"].ToString() == "/" || Request["Path"].ToString() == "/UserFiles/")
                {
                    //FilesULLI(ConfigurationManager.AppSettings["FileManager"].ToString());
                    FilesULLI(RootDirectory_Url);
                }
                else
                {
                    FilesULLI(Request["Path"].ToString());
                }
            }


            if (Request["funcation"].ToString() == "LoadFilesHTML")
            {
                if (string.IsNullOrEmpty(Request["Path"].ToString()) || Request["Path"].ToString() == "/" || Request["Path"].ToString() == "/UserFiles/")
                {
                    //LoadFilesHTML(ConfigurationManager.AppSettings["FileManager"].ToString());
                    LoadFilesHTML(RootDirectory_Url);
                }
                else
                {
                    LoadFilesHTML(Request["Path"].ToString());
                }
            }


            if (Request["funcation"].ToString() == "CreateFolder")
            {
                CreateFolder(Request["Path"].ToString(), Request["FolderName"].ToString());
            }

            if (Request["funcation"].ToString() == "DelteFileFolder")
            {
                DelteFileFolder(Request["FolderName"].ToString());
            }

            if (Request["funcation"].ToString() == "CopyFiles")
            {
                CopyFiles(Request["SelectedFiles"].ToString(), Request["FolderName"].ToString());
            }

            if (Request["funcation"].ToString() == "CutDirectory")
            {
                CutDirectory(Request["SelectedFiles"].ToString(), Request["FolderName"].ToString());
            }

        }
    }

    // chnage this funcation if you want to create your Authenticate 
    public void AuthenticateFileManager()
    {

        /* Edit this funcation to  AuthenticateFileManager*/
        string SessionID = Request["sessionid"].ToString();

        if (Request.Cookies[SessionID] != null)
        {

        }
        else
        {
            Response.Clear();
            Response.Write("Access Denied");
            Response.End();
        }

    }


    private void FilesULLI(string DirPath)
    {
        //DirPath = DirPath.Replace("//", "/");
        //DirPath = (DirPath.Length - 1 > 1) ? (DirPath.Substring(DirPath.Length - 1, 1).Equals("/") ? DirPath.Substring(0, DirPath.Length - 1) : DirPath) : DirPath;
        string FullPath = MapServerPhysicalPath(DirPath);

        if (Request.PhysicalApplicationPath.LastIndexOf('\\') - 1 == Request.PhysicalApplicationPath.Length)
            FullPath = Request.PhysicalApplicationPath;

        string[] Files = Directory.GetFiles(FullPath);
        string[] Directorys = Directory.GetDirectories(FullPath);
        string HTML = "<ol id=selectable>";


        foreach (string File in Files)
        {
            FileInfo f = new FileInfo(File);
            int FileType;
            string iconestring = GetIcone(System.IO.Path.GetExtension(File), out FileType);
            //HTML += " <input class='btn' style='float:right;' onclick=OpenFile('" + FileType.ToString() + "','" + (DirPath + "/" + System.IO.Path.GetFileName(File)).Replace(" ", "%20") + "') id='Button1' type='button' value='open' /> ";
            //HTML += " <input class='btn' style='float:right;' onclick=InsertFile('" + (DirPath + "/" + System.IO.Path.GetFileName(File)).Replace(" ", "%20") + "') id='Button2' type='button' value='insert' /> ";
            //HTML += "<li path='" + (DirPath + "/" + System.IO.Path.GetFileName(File)).Replace(" ", "%20") + "' ondblclick=OpenFile('" + FileType.ToString() + "','" + (DirPath + "/" + System.IO.Path.GetFileName(File)).Replace(" ", "%20") + "') class=ui-widget-content>";
            //HTML += "<img style='float:left;' width='25px;' src='" + iconestring + "'/>";
            //HTML += "<span float:left;padding-right:20px;>" + System.IO.Path.GetFileName(File) + "</span>";
            //HTML += "<br><span float:left;padding-right:20px;>" + f.CreationTime + "</span>";
            //HTML += "<div style='float:right;padding-right:20px;padding-top:0px;'>" + f.Length / 1024 + " KB</div>";
            //HTML += "</li>";

            if (DirPath.LastIndexOf('/') != DirPath.Length - 1 && DirPath.Length > 1)
            {
                DirPath = DirPath + "/";
            }

            HTML += " <input class='btn' style='float:right;' onclick=OpenFile('" + FileType.ToString() + "','" + (DirPath + System.IO.Path.GetFileName(File)).Replace(" ", "%20") + "') id='Button1' type='button' value='open' /> ";
            HTML += " <input class='btn' style='float:right;' onclick=InsertFile('" + (DirPath + System.IO.Path.GetFileName(File)).Replace(" ", "%20") + "') id='Button2' type='button' value='insert' /> ";
            HTML += "<li path='" + (DirPath + System.IO.Path.GetFileName(File)).Replace(" ", "%20") + "' ondblclick=OpenFile('" + FileType.ToString() + "','" + (DirPath + System.IO.Path.GetFileName(File)).Replace(" ", "%20") + "') class=ui-widget-content>";
            HTML += "<img style='float:left;' width='25px;' src='" + iconestring + "'/>";
            HTML += "<span float:left;padding-right:20px;>" + System.IO.Path.GetFileName(File) + "</span>";
            HTML += "<div style='float:right;padding-right:20px;padding-top:0px;'>" + f.Length / 1024 + " KB</div>";
            HTML += "</li>";
        }

        foreach (string Dir in Directorys)
        {

            int FileType;
            //HTML += " <input class='btn' style='float:right;' onclick=OpenFile('" + "2" + "','" + (DirPath + "/" + System.IO.Path.GetFileName(Dir)).Replace(" ", "%20") + "') id='Button1' type='button' value='open' /> ";
            //// HTML += " <input class='btn' style='float:right;' onclick=InsertFile('" + (DirPath + "/" + System.IO.Path.GetFileName(Dir)).Replace(" ", "%20") + "') id='Button2' type='button' value='insert' /> ";
            //HTML += "<li path='" + (DirPath + "/" + System.IO.Path.GetFileName(Dir)).Replace(" ", "%20") + "'" + " ondblclick=OpenFile('" + "2" + "','" + (DirPath + "/" + System.IO.Path.GetFileName(Dir)).Replace(" ", "%20") + "') class=ui-widget-content>";
            //HTML += "<img style='float:left;' width='25px;' src='icons/folder-32.png' />";
            //HTML += "<span float:left;padding-right:20px;>" + System.IO.Path.GetFileName(Dir) + "</span>";
            //HTML += "</li>";

            if (DirPath.LastIndexOf('/') != DirPath.Length - 1 && DirPath.Length > 1)
            {
                DirPath = DirPath + "/";
            }

            HTML += " <input class='btn' style='float:right;' onclick=OpenFile('" + "2" + "','" + (DirPath + System.IO.Path.GetFileName(Dir)).Replace(" ", "%20") + "') id='Button1' type='button' value='open' /> ";
            HTML += "<li path='" + (DirPath + System.IO.Path.GetFileName(Dir)).Replace(" ", "%20") + "'" + " ondblclick=OpenFile('" + "2" + "','" + (DirPath + System.IO.Path.GetFileName(Dir)).Replace(" ", "%20") + "') class=ui-widget-content>";
            HTML += "<img style='float:left;' width='25px;' src='icons/folder-32.png' />";
            HTML += "<span float:left;padding-right:20px;>" + System.IO.Path.GetFileName(Dir) + "</span>";
            HTML += "</li>";
        }
        HTML = HTML + "</ol>";
        Response.Clear();
        Response.Write(HTML);
        Response.End();
    }

    // this funcation not used its return the files and folder as json array
    private void LoadFiles(string DirPath)
    {


        string FullPath = MapServerPhysicalPath(DirPath);

        // additional securite check to make sure that no operation can done out of the user file
        //String userFileFolder = MapServerPhysicalPath(ConfigurationManager.AppSettings["FileManager"].ToString());
        String userFileFolder = MapServerPhysicalPath(RootDirectory_Url);
        if (!FullPath.Contains(userFileFolder))
            return;

        string[] Files = Directory.GetFiles(FullPath);
        string[] Directorys = Directory.GetDirectories(FullPath);
        string Json = "[";

        foreach (string File in Files)
        {
            if (DirPath.LastIndexOf('/') != DirPath.Length - 1 && DirPath.Length > 1)
            {
                DirPath = DirPath + "/";
            }

            FileInfo f = new FileInfo(File);
            int FileType;
            Json += "{\"FilePath\":\"" + (DirPath + System.IO.Path.GetFileName(File)).Replace(" ", "%20") + "\",";
            Json += "\"FileName\":\"" + System.IO.Path.GetFileName(File) + "\",";
            Json += "\"FileSize\":\"" + f.Length.ToString() + " bytes" + "\",";
            Json += "\"Fileicon\":\"" + GetIcone(System.IO.Path.GetExtension(File), out FileType) + "\",";
            Json += "\"FileType\":\"" + FileType.ToString() + "\"";

            Json += "},";
        }

        foreach (string Dir in Directorys)
        {
            if (DirPath.LastIndexOf('/') != DirPath.Length - 1 && DirPath.Length > 1)
            {
                DirPath = DirPath + "/";
            }

            Json += "{\"FilePath\":\"" + (DirPath + Path.GetFileName(Dir)).Replace(" ", "%20") + "/" + "\",";
            Json += "\"FileName\":\"" + Path.GetFileName(Dir) + "\",";
            Json += "\"FileType\":\"" + "2" + "\",";
            Json += "\"FileSize\":\"" + "" + "\",";
            Json += "\"Fileicon\":\"" + "icons/folder-32.png" + "\"";
            Json += "},";
        }

        Json = Json.Trim(',');
        Json = Json + "]";
        Response.Clear();
        Response.Write(Json);
        Response.End();


    }


    private void LoadFilesHTML(string DirPath)
    {

        string html = "";
        if (DirPath == "0")
        {
            //html += "<li path='" + ConfigurationManager.AppSettings["FileManager"].ToString() + "' class='jstree-closed' id='root2'><a onclick=LoadFiles('" + ConfigurationManager.AppSettings["FileManager"].ToString() + "') href='#'>" + "Root" + "</a></li>";
            html += "<li path='" + RootDirectory_Url + "' class='jstree-closed' id='root2'><a onclick=LoadFiles('" + RootDirectory_Url + "') href='#'>" + "Root" + "</a></li>";
        }
        else
        {
            if (DirPath.LastIndexOf('/') != DirPath.Length - 1 && DirPath.Length > 1)
            {
                DirPath = DirPath + "/";
            }

            string FullPath = MapServerPhysicalPath(DirPath);
            // additional securite check to make sure that no operation can done out of the user file
            //String userFileFolder = MapServerPhysicalPath(ConfigurationManager.AppSettings["FileManager"].ToString());
            String userFileFolder = MapServerPhysicalPath(RootDirectory_Url);
            if (!FullPath.Contains(userFileFolder))
                return;

            string[] Files = Directory.GetFiles(FullPath);
            string[] Directorys = Directory.GetDirectories(FullPath);
            foreach (string Dir in Directorys)
            {
                html += "<li path='" + (DirPath + System.IO.Path.GetFileName(Dir)) + "/" + "' class='jstree-closed'><a onclick=LoadFiles('" + (DirPath + System.IO.Path.GetFileName(Dir)).Replace(" ", "%20") + "/')  href='#'>" + Path.GetFileName(Dir) + "</a></li>";
                //html += "{\"FilePath\":\"" + DirPath + Path.GetFileName(Dir) + "/" + "\",";
                //html += "\"FileName\":\"" + Path.GetFileName(Dir) + "\",";
                //html += "\"FileType\":\"" + "2" + "\",";
                //html += "\"FileSize\":\"" + "" + "\",";
                //html += "\"Fileicon\":\"" + "icons/folder-32.png" + "\"";
                //html += "},";
            }
        }


        Response.Clear();
        Response.Write(html);
        Response.End();


    }

    private string GetIcone(string EX, out int FileType)
    {
        FileType = 1;
        switch (EX.ToLower())
        {

            case ".gif":
                return "icons/Paint-32.png";
            case ".jpg":
                return "icons/Paint-32.png";
            case ".png":
                return "icons/Paint-32.png";
            case ".bmp":
                return "icons/Paint-32.png";
            case ".icon":
                return "icons/Paint-32.png";
            case ".zip":
                return "icons/Zip-file-32.png";
            case ".rar":
                return "icons/rar_32.png";
            default:
                FileType = 3;
                return "icons/Unknown-icon.png";




        }
    }


    private void CreateFolder(string Path, string FolderName)
    {

        string FolderPath = MapServerPhysicalPath(Path + FolderName);

        // additional securite check to make sure that no operation can done out of the user file
        //String userFileFolder = MapServerPhysicalPath(ConfigurationManager.AppSettings["FileManager"].ToString());
        String userFileFolder = MapServerPhysicalPath(RootDirectory_Url);
        if (!FolderPath.Contains(userFileFolder))
            return;

        Directory.CreateDirectory(FolderPath);
    }

    private void DelteFileFolder(string FolderNames)
    {

        //FolderNames.Replace("//", "/");
        var NFolderNames = FolderNames.Split(',');

        foreach (string FolderName in NFolderNames)
        {
            if (!string.IsNullOrEmpty(FolderName))
            {
                if (System.IO.Directory.Exists(MapServerPhysicalPath(FolderName)))
                {
                    try
                    {

                        DeleteFolder(MapServerPhysicalPath(FolderName));
                    }
                    catch (Exception ex)
                    {
                        output(ex.Message);
                    }
                }

                if (System.IO.File.Exists(MapServerPhysicalPath(FolderName)))
                {
                    System.IO.File.Delete(MapServerPhysicalPath(FolderName));
                }
            }
        }

        output("success");
    }



    private void DeleteFolder(string FolderPath)
    {

        // additional securite check to make sure that no operation can done out of the user file
        //String userFileFolder = MapServerPhysicalPath(ConfigurationManager.AppSettings["FileManager"].ToString());
        String userFileFolder = MapServerPhysicalPath(RootDirectory_Url);
        if (!FolderPath.Contains(userFileFolder))
            return;

        string[] files = System.IO.Directory.GetFiles(FolderPath);
        foreach (string file in files)
        {
            System.IO.File.Delete(file);
        }
        string[] Folders = System.IO.Directory.GetDirectories(FolderPath);
        foreach (string Folder in Folders)
        {
            DeleteFolder(Folder);
        }

        System.IO.Directory.Delete(FolderPath);
    }

    private void CopyFiles(string selectedFiles, string CurentPath)
    {

        string FullCurentPath = MapServerPhysicalPath(CurentPath);
        // additional securite check to make sure that no operation can done out of the user file
        //String userFileFolder = MapServerPhysicalPath(ConfigurationManager.AppSettings["FileManager"].ToString());
        String userFileFolder = MapServerPhysicalPath(RootDirectory_Url);
        if (!FullCurentPath.Contains(userFileFolder))
            return;

        //selectedFiles.Replace("//", "/");
        var NFolderNames = selectedFiles.Split(',');
        foreach (string FolderName in NFolderNames)
        {
            if (!string.IsNullOrEmpty(FolderName))
            {
                // additional securite check to make sure that no operation can done out of the user file                
                if (!MapServerPhysicalPath(FolderName).Contains(userFileFolder))
                    return;

                string NewPath = MapServerPhysicalPath(CurentPath + Path.GetFileName(FolderName));
                if (!File.Exists(NewPath))
                {
                    if (File.Exists(MapServerPhysicalPath(FolderName)))
                    {
                        File.Copy(MapServerPhysicalPath(FolderName), NewPath);
                    }

                    if (Directory.Exists(MapServerPhysicalPath(FolderName)))
                    {
                        copyDirectory(MapServerPhysicalPath(FolderName), NewPath);
                    }
                }
            }
        }
    }



    private void output(string Message)
    {
        Response.Clear();
        Response.Write(Message);
        Response.End();
    }


    // Copy directory structure recursively
    //http://www.codeproject.com/KB/files/copydirectoriesrecursive.aspx
    public static void copyDirectory(string Src, string Dst)
    {
        String[] Files;

        if (Dst[Dst.Length - 1] != Path.DirectorySeparatorChar)
            Dst += Path.DirectorySeparatorChar;
        if (!Directory.Exists(Dst)) Directory.CreateDirectory(Dst);
        Files = Directory.GetFileSystemEntries(Src);
        foreach (string Element in Files)
        {
            // Sub directories
            if (Directory.Exists(Element))
                copyDirectory(Element, Dst + Path.GetFileName(Element));
            // Files in directory
            else
                File.Copy(Element, Dst + Path.GetFileName(Element), true);
        }
    }





    private void CutDirectory(string selectedFiles, string CurentPath)
    {
        //selectedFiles.Replace("//", "/");

        string FullCurentPath = MapServerPhysicalPath(CurentPath);
        // additional securite check to make sure that no operation can done out of the user file
        //String userFileFolder = MapServerPhysicalPath(ConfigurationManager.AppSettings["FileManager"].ToString());
        String userFileFolder = MapServerPhysicalPath(RootDirectory_Url);
        if (!FullCurentPath.Contains(userFileFolder))
            return;

        var NFolderNames = selectedFiles.Split(',');
        foreach (string FolderName in NFolderNames)
        {
            if (!string.IsNullOrEmpty(FolderName))
            {
                // additional securite check to make sure that no operation can done out of the user file                
                if (!MapServerPhysicalPath(FolderName).Contains(userFileFolder))
                    return;

                string NewPath = MapServerPhysicalPath(CurentPath + Path.GetFileName(FolderName));
                if (!File.Exists(NewPath))
                {
                    if (File.Exists(MapServerPhysicalPath(FolderName)))
                    {
                        File.Move(MapServerPhysicalPath(FolderName), NewPath);
                    }

                    if (Directory.Exists(MapServerPhysicalPath(FolderName)))
                    {
                        Directory.Move(MapServerPhysicalPath(FolderName), NewPath);
                    }
                }
            }
        }

    }

    private String MapServerPhysicalPath(String filepath)
    {
        //filepath = filepath.Replace(RootDirectory_Url, RootDirectory_Path);
        //return filepath;

        
        String FullPath = "";

        if (Request.PhysicalApplicationPath.LastIndexOf('\\') + 1 == Request.PhysicalApplicationPath.Length)
            FullPath = RootDirectory_Path;
            //FullPath = Request.PhysicalApplicationPath.Substring(0, Request.PhysicalApplicationPath.LastIndexOf('\\'));

        filepath = filepath.Replace(RootDirectory_Url, "");
        filepath = filepath.Replace("/", "\\");
        //filepath = filepath.Replace("\\IPCMS\\", "\\");

        if (filepath.IndexOf("\\") == 0)
            filepath = filepath.Substring(1);

        return FullPath + filepath;
        
    }


}