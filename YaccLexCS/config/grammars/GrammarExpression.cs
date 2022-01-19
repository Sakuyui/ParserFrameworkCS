
using YaccLexCS.ycomplier;
using YaccLexCS.ycomplier.attribution;

namespace YaccLexCS.config{
	[GrammarConfiguration]
	public static class GrammarExpression{

		[GrammarDefinition("expression",
			"terminal_expression","assign_expression","additive_expression","multiplicative_expression",
			"unary_expression","primary_expression",
			"relational_expression", "equality_expression","logical_or_expression", "logical_and_expression",
			"define_var_expression", "lambda_expression"
			)]
		public static void expression(CompilerContext context){

		}

		[GrammarDefinition("define_var_expression", "LET ID ASSIGN expression", "VAR ID ASSIGN expression")]
		public static void define_var_expression(CompilerContext context)
		{
		}

		[GrammarDefinition("logical_and_expression", "equality_expression",
			"logical_and_expression LOGICAL_AND equality_expression")]
		public static void logical_and_expression(CompilerContext context) { }

		[GrammarDefinition("logical_or_expression", "logical_and_expression", 
			"logical_or_expression LOGICAL_OR logical_and_expression")]
		public static void logical_or_expression(CompilerContext context) { }

		[GrammarDefinition("relational_expression", "additive_expression", 
			"relational_expression GT additive_expression", "relational_expression GE additive_expression",
			"relational_expression LT additive_expression", "relational_expression LE additive_expression")]
		public static void relational_expression(CompilerContext context) { 
		}

		[GrammarDefinition("equality_expression", "relational_expression", "equality_expression EQ relational_expression"
			, "equality_expression NE relational_expression")]
		public static void equality_expression(CompilerContext context) { }

		[GrammarDefinition("assign_expression","ID ASSIGN expression")]
		public static void assign_expression(CompilerContext context){

		}

		[GrammarDefinition("terminal_expression","ID","DOUBLE_LITERAL","STRING")]
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


		[GrammarDefinition("primary_expression","LP expression RP","ID","DOUBLE_LITERAL","STRING",
			"BREAK", "CONTINUE", "RETURN")]
		public static void primary_expression(CompilerContext context){

		}

		[GrammarDefinition("lambda_expression", "LAMBDA LP params_list RP INTRODUCE statement")]
		public static void lambda_expression()
		{
		}

	}
}