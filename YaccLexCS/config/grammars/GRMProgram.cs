using YaccLexCS.ycomplier.attribution;

namespace YaccLexCS.config.grammars
{
    [GrammarConfiguration]
    public class GRMProgram
    {
        [GrammarDefinition("program", "[statement](SEMICOL|CR)")]
        public void Program()
        {
            
        }
    }
}