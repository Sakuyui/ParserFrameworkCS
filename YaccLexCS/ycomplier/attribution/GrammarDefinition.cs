using System;
using System.Collections.Generic;
using System.Linq;

namespace YaccLexCS.ycomplier.attribution
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    [Serializable]
    public class GrammarDefinition : YDefinition
    {
        public readonly string Name;
        public List<string> CfgItem;

        public GrammarDefinition(string name, params string[] cfgItem)
        {
            Name = name;
            CfgItem = cfgItem.ToList();
        }

        public override string ToString()
        {
            return $"[Cfg: {Name} : {CfgItem.ToEnumerationString()}";
        }
    }
}