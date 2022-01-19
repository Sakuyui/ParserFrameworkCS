using System.Collections.Generic;

namespace YaccLexCS.ycomplier
{
    public class RuntimeContext
    {
        public abstract class Frame{

        }
        public abstract class LinkedFrame
        {

        }
        public class CommonStackFrame : LinkedFrame
        {
            public Dictionary<string, object> _localVar = new();
            public object GetLocalVar(string name)
            {
                return _localVar[name];
            }
            public void SetLocalVar(string name, object val)
            {
                _localVar[name] = val;
            }
        }

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
        private Stack<CommonStackFrame> _stackFrames = new();

        public RuntimeContext()
        {
            this["v_tokenSourceText"] = "";
            this["v_tokenVal"] = null!;
            _stackFrames.Push(new ());
        }

        public CommonStackFrame GetCurrentCommonFrame()
        {
            return _stackFrames.Peek();
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

        public void SetLocalVar(string name, object val)
        {

        }
    }
}