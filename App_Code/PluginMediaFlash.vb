Imports Microsoft.VisualBasic

Public Class PluginMediaFlash

    ''' <summary>
    ''' Replacement Flash Player Plugin of Tiny MCE
    ''' </summary>
    ''' <param name="input"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ConvertTo_GDDFLVPLAYER(ByVal input As String) As String
        '<p>
        '<center>
        '<object width="600" height="450" data="http://10.200.1.12:307/plugin/TinyMCE_3.5.8/jscripts/tiny_mce/plugins/media/moxieplayer.swf" type="application/x-shockwave-flash">
        '<param name="src" value="http://10.200.1.12:307/plugin/TinyMCE_3.5.8/jscripts/tiny_mce/plugins/media/moxieplayer.swf" />
        '<param name="flashvars" value="url=/UserFiles/TORAY_40th_Anniversary_Official.flv&amp;poster=/" />
        '<param name="allowfullscreen" value="true" />
        '<param name="allowscriptaccess" value="true" />
        '</object>
        '</center>
        '</p>

        Dim output As String = input

        '# Change Player
        output = output.Replace("media/moxieplayer.swf", "media/gddflvplayer.swf")

        '# Add No Flash de Installer
        Dim idx As Int32 = output.IndexOf("<param name=""flashvars""")
        If (idx > -1) Then
            output = output.Insert(idx, "<a href=""" & System.Configuration.ConfigurationManager.AppSettings("FLASHINSTALLER_FILE").ToString & """><img src=""" & System.Configuration.ConfigurationManager.AppSettings("FLASHINSTALLER_IMAGE").ToString & """ alt=""Install Adobe Flash player"" /></a>")
        End If

        '# Change flashvars path
        'Dim flashvars_vdo As String = String.Concat("vdo=", System.Configuration.ConfigurationManager.AppSettings.Get("FILEMANAGER_BASEURL").ToString)
        output = output.Replace("url=", "vdo=")

        '# Change flashvars autoplay
        output = output.Replace("flv&amp;poster=/", "flv&amp;autoplay=true")

        Return output
    End Function



    ''' <summary>
    ''' Original Flash Player Plugin of Tiny MCE
    ''' </summary>
    ''' <param name="input"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ConvertTo_MOXIEPLAYER(ByVal input As String) As String
        '<p>
        '<center>
        '<object width="600" height="450" data="http://10.200.1.12:307/plugin/TinyMCE_3.5.8/jscripts/tiny_mce/plugins/media/gddflvplayer.swf" type="application/x-shockwave-flash">
        '<param name="src" value="http://10.200.1.12:307/plugin/TinyMCE_3.5.8/jscripts/tiny_mce/plugins/media/gddflvplayer.swf" />
        '<a href="<%= System.Configuration.ConfigurationManager.AppSettings("FLASHINSTALLER_FILE").ToString%>"><img src="<%= System.Configuration.ConfigurationManager.AppSettings("FLASHINSTALLER_IMAGE").ToString%>" alt="Install Adobe Flash player" /></a>
        '<param name="flashvars" value="url=http://10.200.1.12:307/UserFiles/TORAY_40th_Anniversary_Official.flv&amp;autoplay=true" />
        '<param name="allowfullscreen" value="true" />
        '<param name="allowscriptaccess" value="true" />
        '</object>
        '</center>
        '</p>

        Dim output As String = input

        '# Change Player
        output = output.Replace("media/gddflvplayer.swf", "media/moxieplayer.swf")

        '# Remove No Flash de Installer
        Dim idx_e As Int32 = output.IndexOf("</a><param name=""flashvars""")
        If (idx_e > -1) Then
            Dim idx_s As Int32 = output.LastIndexOf("<a href=", idx_e)
            idx_e += 4
            output = output.Remove(idx_s, idx_e - idx_s)
        End If

        '# Change flashvars path
        ''Dim flashvars_vdo As String = String.Concat("vdo=", System.Configuration.ConfigurationManager.AppSettings.Get("FILEMANAGER_BASEURL").ToString)
        ''output = output.Replace(flashvars_vdo, "url=/")
        output = output.Replace("vdo=", "url=")

        '# Change flashvars autoplay
        output = output.Replace("flv&amp;autoplay=true", "flv&amp;poster=/")

        Return output
    End Function

End Class
