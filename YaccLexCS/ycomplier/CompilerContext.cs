using System.Collections.Generic;

namespace YaccLexCS.ycomplier
{
    public class CompilerContext
    {
      
    
        private readonly Dictionary<string, object> _kvMemory = new();
        
        public CompilerContext()
        {
            this["v_tokenSourceText"] = "";
            this["v_tokenVal"] = null!;
        }

      
        public object? this[string key]
        {
            get => _kvMemory.ContainsKey(key) ? _kvMemory[key] : null;
            set => _kvMemory[key] = value;
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

        public void SetLocalVar(string name, object val)
        {

        }
    }
}