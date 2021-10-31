using YaccLexCS.ycomplier.attribution;

namespace YaccLexCS.config.grammars
{
    [GrammarConfiguration]
    public class GRMBlock
    {
        [GrammarDefinition("block", "LCBRACE [statement] {(SEMICOL|CR)[statement]} RBARCE", "primary")]
        public static void Factor()
        {
            
        }
        
        [GrammarDefinition("statement", "simple", "WHILE expr block", "IF expr block [ELSE block]")]
        public static void Statement()
        {
            
        }
        
    }
}