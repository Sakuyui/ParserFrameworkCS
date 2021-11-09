using YaccLexCS.ycomplier;
using YaccLexCS.ycomplier.attribution;

namespace YaccLexCS.config{
	[GrammarConfiguration]
	public static class GrammarExpress{

		[GrammarDefinition("expression","terminal_expression assign_expression","additive_expression","multiplicative_expression","unary_expression","primary_expression")]
		public static void expression(CompilerContext context){

		}


		[GrammarDefinition("assign_expression","ID ASSIGN expression")]
		public static void assign_expression(CompilerContext context){

		}


		[GrammarDefinition("terminal_expression","ID DOUBLE_LITERAL STRING")]
		public static void terminal_expression(CompilerContext context){

		}


		[GrammarDefinition("additive_expression","additive_expression ADD multiplicative_expression","multiplicative_expression")]
		public static void additive_expression(CompilerContext context){

		}


		[GrammarDefinition("multiplicative_expression","unary_expression","multiplicative_expression MUL unary_expression","multiplicative_expression DIV unary_expression","multiplicative_expression MOD unary_expression")]
		public static void multiplicative_expression(CompilerContext context){

		}


		[GrammarDefinition("unary_expression","primary_expression","SUB unary_expression")]
		public static void unary_expression(CompilerContext context){

		}


		[GrammarDefinition("primary_expression","LP expression RP","ID","DOUBLE_LITERAL","STRING")]
		public static void primary_expression(CompilerContext context){

		}

	}
}