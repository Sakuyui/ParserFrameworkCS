using System.Collections.Generic;

namespace YaccLexCS.runtime
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
            public Stack<Dictionary<string, object>> _linkedLocalStorage = new();
            public CommonStackFrame()
            {
                _linkedLocalStorage.Push(new Dictionary<string, object>());
            }
            public Dictionary<string, object> CreateNewStorageBlockForNewCodeBlock()
            {
                var b = new Dictionary<string, object>();
                _linkedLocalStorage.Push(b);
                return b;
            }
            public void RemoveNewestStorageBlock()
            {
                if (!_linkedLocalStorage.Any()) throw new Exception();
                _linkedLocalStorage.Pop();
            }
            public object GetLocalVar(string name)
            {
                foreach(var storage in _linkedLocalStorage)
                {
                    if(storage.ContainsKey(name)) return storage[name];
                }
                return null;
            }
            /*SetLocalVar will allocate a new var in currentBlock. It correspondent to var/let*/
            public dynamic SetLocalVar(string name, object val)
            {
                if (_linkedLocalStorage.Peek().ContainsKey(name))
                {
                    return EEStatusValue.REDUPLICATE_VAR_DEF;
                }
                _linkedLocalStorage.Peek()[name] = val;
                return val;
            }
            public void FindAndSetVar(string name, object val)
            {
                foreach (var storage in _linkedLocalStorage)
                {
                    if (storage.ContainsKey(name))
                    {
                        storage[name] = val;
                        return;
                    }
                }
                _linkedLocalStorage.Peek()[name] = val;
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