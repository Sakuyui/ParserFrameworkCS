using System;
using System.Text.RegularExpressions;
using YaccLexCS.ycomplier.util;

namespace YaccLexCS.ycomplier.conf.attribution
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TokenConfiguration : Attribute
    {
        
    }
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class TokenDefinition : Attribute
    {
        public readonly Regex Pattern;
        
        public readonly bool UseRegex;
        public string TokenName { get; }

        public readonly int Priority;

        public TokenDefinition(string tokenName, string patternDesc, bool useRegex = false, int priority = 0)
        {
            TokenName = tokenName;
            Pattern = useRegex ? new Regex(patternDesc) : StringProcess.StringToRegex(patternDesc);
            UseRegex = useRegex;
            Priority = priority;
        }
    }

    
}