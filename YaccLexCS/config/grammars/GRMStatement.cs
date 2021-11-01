using YaccLexCS.ycomplier.attribution;

namespace YaccLexCS.config.grammars
{
    [GrammarConfiguration]
    public class GRMStatement
    {
        [GrammarDefinition("statement",  "if_statement", "while_statement")]
        public static void DefinitionOrStatement()
        {
            
        }
        
        
    }
}