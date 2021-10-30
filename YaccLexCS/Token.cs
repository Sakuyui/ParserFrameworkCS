namespace YaccLexCS
{
    public class Token
    {
        private readonly string _sourceText;
        public string Type { get; }

        public Token(string sourceText, string type)
        {
            _sourceText = sourceText;
            Type = type;
        }

        public override string ToString()
        {
            return $"<{Type}, {_sourceText}>";
        }
    }
}