using YaccLexCS.ycomplier;
using YaccLexCS.ycomplier.attribution;

namespace YaccLexCS.config{
	[GrammarConfiguration]
	public static class GrammarStatement{

		[GrammarDefinition("statement","if_statement","while_statement","expression SEMICOLON", "for_statement",
			"block")]
		public static void statement(RuntimeContext context){

		}


		[GrammarDefinition("while_statement","WHILE LP expression RP statement")]
		public static void while_statement(RuntimeContext context){
			
		}


		[GrammarDefinition("if_statement","IF LP expression RP statement", "IF LP expression RP statement ELSE statement",
			"IF LP expression RP statement elsif_list", "IF LP expression RP statement elsif_list ELSE statement")]
		public static void if_statement(RuntimeContext context){

		}
		[GrammarDefinition("elsif_list", "elsif", "elsif_list elsif")]
		public static void eliif_list(RuntimeContext context)
		{

		}
		[GrammarDefinition("elsif", "ELSIF LP expression RP statement")]
		public static void eliif(RuntimeContext context)
		{
			
		}

		[GrammarDefinition("statement_list","statement","statement_list statement")]
		public static void statement_list(RuntimeContext context){

		}

		[GrammarDefinition("for_statement", "FOR LP expression SEMICOLON expression SEMICOLON expression RP statement")]
		public static void for_statement(RuntimeContext context)
		{

		}
	}
}