namespace Library.Root.Control
{
    public interface IPageContext
    {
        void Redirect(string url);
        string ResolveUrl(string relativeUrl);
        string UrlDecode(string value);
        string UrlEncode(string value);
    }
}
