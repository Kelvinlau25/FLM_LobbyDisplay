namespace Library.Root.Object
{
    /// <summary>
    /// Object of the List item ( Contain Text and Value )
    /// </summary>
    public class Binder
    {
        public Binder()
        {
            _Text = string.Empty;
            _Value = string.Empty;
        }

        public Binder(string Text, string Value)
        {
            _Text = Text;
            _Value = Value;
        }

        private string _Text;
        public string Text
        {
            get { return _Text; }
            set { _Text = value; }
        }

        private string _Value;
        public string Value
        {
            get { return _Value; }
            set { _Value = value; }
        }
    }
}
