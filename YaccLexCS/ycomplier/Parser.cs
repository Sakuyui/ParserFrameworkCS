using System.Collections.Generic;
using YaccLexCS.ycomplier;

namespace YaccLexCS
{
    
    public abstract class Parser
    {
       
        
        private readonly ParserContext _context ;

        private Parser(ParserContext context)
        {
            _context = context;
        }
        public abstract void Parse();

        public object SetParserContextVariable(string name, object obj)
        {
            this[name] = obj;
            return obj;
        }
        protected object this[string key]
        {
            set => _context[key] = value;
            get => _context[key];
        }
    }

   
}