using YaccLexCS.ycomplier.attribution;

namespace YaccLexCS.config.grammars
{
    [GrammarConfiguration]
    public class GRMPrimary
    {
        [GrammarDefinition("primary", "LBRACE expr RBRACE", "DOUBLE_LITERAL", "ID", "STRING")]
        public static void Primary()
        {
            
        }
        
        [GrammarDefinition("factor", "SUB primary", "primary")]
        public static void Factor()
        {
            
        }
        
        [GrammarDefinition("expr", "factor LCBRACE OP factor RCBRACE")]
        public static void Expr()
        {
            
        }
        
      
    }
}