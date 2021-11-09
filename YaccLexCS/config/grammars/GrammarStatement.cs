using YaccLexCS.ycomplier;
using YaccLexCS.ycomplier.attribution;

namespace YaccLexCS.config{
	[GrammarConfiguration]
	public static class GrammarStatement{

		[GrammarDefinition("statement","if_statement","while_statement","expression SEMICOLON")]
		public static void statement(CompilerContext context){

		}


		[GrammarDefinition("while_statement","WHILE LP expression RP block")]
		public static void while_statement(CompilerContext context){

		}


		[GrammarDefinition("if_statement","IF LP expression RP block")]
		public static void if_statement(CompilerContext context){

		}


		[GrammarDefinition("statement_list","statement","statement_list statement")]
		public static void statement_list(CompilerContext context){

		}

	}
}