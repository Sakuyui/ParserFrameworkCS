using System.Collections.Generic;

namespace YaccLexCS.ycomplier
{
    public class ParserContext
    {
        private readonly Dictionary<string, object> _kvComp = new();

        public string TokenText
        {
            get => (string) this["v_tokenSourceText"];
            set => this["v_tokenSourceText"] = value;
        }
          

        public dynamic TokenVal
        {
            get => this["v_tokenVal"];
            set => this["v_tokenVal"] = value;
        }

        public ParserContext()
        {
            this["v_tokenSourceText"] = "";
            this["v_tokenVal"] = null!;
        }
        public object this[string key]
        {
            get => _kvComp[key];
            set => _kvComp[key] = value;
        }
    }
}