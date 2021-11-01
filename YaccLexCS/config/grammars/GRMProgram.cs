using YaccLexCS.ycomplier.attribution;

namespace YaccLexCS.config.grammars
{
    [GrammarConfiguration]
    public class GRMProgram
    {
        //[]是一回 {}是0~ 
        //[GrammarDefinition("program", "[statement](SEMICOL|CR)")]
        [GrammarDefinition("program", "[statement](SEMICOL|CR)")]
        public void Program()
        {
            
        }
    }
}