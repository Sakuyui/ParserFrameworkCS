using YaccLexCS.ycomplier;
using YaccLexCS.ycomplier.attribution;

namespace YaccLexCS.config{
	[GrammarConfiguration]
	public static class GrammarExpression{

		[GrammarDefinition("expression",
			"terminal_expression","assign_expression","additive_expression","multiplicative_expression",
			"unary_expression","primary_expression",
			"relational_expression", "equality_expression","logical_or_expression", "logical_and_expression")]
		public static void expression(RuntimeContext context){

		}

		[GrammarDefinition("logical_and_expression", "equality_expression",
			"logical_and_expression LOGICAL_AND equality_expression")]
		public static void logical_and_expression(RuntimeContext context)
		{
		}
		[GrammarDefinition("logical_or_expression", "logical_and_expression", 
			"logical_or_expression LOGICAL_OR logical_and_expression")]
		public static void logical_or_expression(RuntimeContext context) { }
		[GrammarDefinition("relational_expression", "additive_expression", 
			"relational_expression GT additive_expression", "relational_expression GE additive_expression",
			"relational_expression LT additive_expression", "relational_expression LE additive_expression")]
		public static void relational_expression(RuntimeContext context) { 
		}

		[GrammarDefinition("equality_expression", "relational_expression", "equality_expression EQ relational_expression"
			, "equality_expression NE relational_expression")]
		public static void equality_expression(RuntimeContext context) { }

		[GrammarDefinition("assign_expression","ID ASSIGN expression")]
		public static void assign_expression(RuntimeContext context){

		}

		[GrammarDefinition("terminal_expression","ID","DOUBLE_LITERAL","STRING")]
		public static void terminal_expression(RuntimeContext context){

		}


		[GrammarDefinition("additive_expression","additive_expression ADD multiplicative_expression","multiplicative_expression")]
		public static void additive_expression(RuntimeContext context){

		}


		[GrammarDefinition("multiplicative_expression","unary_expression","multiplicative_expression MUL unary_expression","multiplicative_expression DIV unary_expression","multiplicative_expression MOD unary_expression")]
		public static void multiplicative_expression(RuntimeContext context){
            
		}


		[GrammarDefinition("unary_expression","primary_expression","SUB unary_expression")]
		public static void unary_expression(RuntimeContext context){

		}


		[GrammarDefinition("primary_expression","LP expression RP","ID","DOUBLE_LITERAL","STRING",
			"BREAK", "CONTINUE", "RETURN")]
		public static void primary_expression(RuntimeContext context){

		}

	}
}