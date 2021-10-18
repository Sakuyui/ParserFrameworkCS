using System.Collections.Generic;

namespace YaccLexCS.ycomplier
{
    public class ParserContext
    {
        private readonly Dictionary<string, object> _kvComp = new();

        public string TokenText
        {
            get => (string) this["attr_tokenSourceText"];
            set => this["attr_tokenSourceText"] = value;
        }
          

        public dynamic TokenVal
        {
            get => this["attr_tokenVal"];
            set => this["attr_tokenVal"] = value;
        }

        public ParserContext()
        {
            this["attr_tokenSourceText"] = "";
            this["attr_tokenVal"] = null!;
        }
        public object this[string key]
        {
            get => _kvComp[key];
            set => _kvComp[key] = value;
        }
    }
}