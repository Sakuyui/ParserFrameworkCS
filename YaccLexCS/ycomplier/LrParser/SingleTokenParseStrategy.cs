using System;
using System.Text.RegularExpressions;
using YaccLexCS.ycomplier.util;

namespace YaccLexCS.ycomplier.LrParser
{
    public class SingleTokenParseStrategy : ValueTupleSlim
    {
        public string TokenName
        {
            get => (string) this[0];
            private init => this[0] = value;
        }

        public Regex Regex
        {
            get => (Regex) this[1];
            private init => this[1] = value;
        }

        public Action<CIExam.Complier.Token> Action
        {
            get => (Action<CIExam.Complier.Token>) this[2];
            private init => this[2] = value;
        }

        public SingleTokenParseStrategy(string tName, Regex r, Action<CIExam.Complier.Token> action = null):base(null, null, null)
        {
            TokenName = tName;
            Regex = r;
            Action = action;
        }
        
    }
}