using System.Collections.Generic;

namespace YaccLexCS.ycomplier
{
    public class CompilerContext
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

        public CompilerContext()
        {
            this["v_tokenSourceText"] = "";
            this["v_tokenVal"] = null!;
        }
        public object? this[string key]
        {
            get  { return _kvComp.ContainsKey(key) ? _kvComp[key] : null; }
 
            set => _kvComp[key] = value;
        }
    }
}