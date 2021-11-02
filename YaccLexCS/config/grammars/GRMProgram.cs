using YaccLexCS.ycomplier.attribution;

namespace YaccLexCS.config.grammars
{
    [GrammarConfiguration]
    public class GRMProgram
    {
        //[]是一回 {}是0~ 
        //[GrammarDefinition("program", "[statement](SEMICOL|CR)")]
        [GrammarDefinition("program", "definition_or_statement", "program definition_or_statement")]
        public static void Program()
        {
            
        }
        
        [GrammarDefinition("definition_or_statement", "expression SEMICOLON")]
        public static void DefinitionOrStatement()
        {
            
        }
        
        
    }
}