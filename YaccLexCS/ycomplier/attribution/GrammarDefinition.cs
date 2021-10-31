using System;
using System.Collections.Generic;
using System.Linq;

namespace YaccLexCS.ycomplier.attribution
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    [Serializable]
    public class GrammarDefinition : Attribute
    {
        public readonly string Name;
        public List<string> CfgItem;

        public GrammarDefinition(string name, params string[] cfgItem)
        {
            Name = name;
            CfgItem = cfgItem.ToList();
        }
        
        
    }
}