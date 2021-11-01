using YaccLexCS.ycomplier.attribution;

namespace YaccLexCS.config.grammars
{
    [GrammarConfiguration]
    public class GRMStatement
    {
        [GrammarDefinition("statement",  "if_statement", "while_statement")]
        public static void Statement()
        {
            
        }
        
        [GrammarDefinition("while_statement",  "WHILE LP expression RP block")]
        public static void  WhileStatement()
        {
            
        }
        
        [GrammarDefinition("if_statement",  "IF LP expression RP block")]
        public static void  IfStatement()
        {
            
        }
        
        [GrammarDefinition("statement_list",  "statement", "statement_list statement")]
        public static void  StatementList()
        {
            
        }
       
        
    }
}