using YaccLexCS.ycomplier;
using YaccLexCS.ycomplier.attribution;

namespace YaccLexCS.config{
	[GrammarConfiguration]
	public static class GrammarCompileUnit{

		[GrammarDefinition("compile_unit","definition_or_statement","program definition_or_statement")]
		public static void compile_unit(CompilerContext context){

		}


		[GrammarDefinition("definition_or_statement","statement")]
		public static void definition_or_statement(CompilerContext context){

		}

	}
}