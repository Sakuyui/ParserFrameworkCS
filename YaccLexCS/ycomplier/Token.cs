namespace YaccLexCS
{
    public class Token
    {
        public readonly string SourceText;
        
        public string Type { get; }
        public int LineNum;
        public Token(string sourceText, string type)
        {
            SourceText = sourceText;
            Type = type;
        }

        public override string ToString()
        {
            return $"<{Type}, {SourceText}>";
        }
    }
}