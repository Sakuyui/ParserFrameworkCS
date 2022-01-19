using System.Collections.Generic;

namespace YaccLexCS.ycomplier
{
    public class RuntimeContext
    {
        public class RuntimeMemory
        {
            private readonly Dictionary<string, object> _kvComp = new();
           
            public object? this[string key]
            {
                get  { return _kvComp.ContainsKey(key) ? _kvComp[key] : null; }
 
                set => _kvComp[key] = value;
            }
           
        }
        private readonly RuntimeMemory _runtimeMemory = new();
        public RuntimeContext()
        {
            this["v_tokenSourceText"] = "";
            this["v_tokenVal"] = null!;
        }
        public object? this[string key]
        {
            get => _runtimeMemory[key];
            set => _runtimeMemory[key] = value;
        }
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

      
    }
}