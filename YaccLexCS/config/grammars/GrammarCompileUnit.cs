using YaccLexCS.ycomplier;
using YaccLexCS.ycomplier.attribution;

namespace YaccLexCS.config{
	[GrammarConfiguration]
	public static class GrammarCompileUnit{

		[BeginningGrammarDefinition("compile_unit","definition_or_statement","compile_unit definition_or_statement")]
		public static void compile_unit(RuntimeContext context){

		}


		[GrammarDefinition("definition_or_statement","statement")]
		public static void definition_or_statement(RuntimeContext context){

		}

	}
}