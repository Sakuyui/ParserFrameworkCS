
using YaccLexCS.ycomplier;
using YaccLexCS.ycomplier.attribution;

namespace YaccLexCS.config{
	[GrammarConfiguration]
	public static class GrammarExpression{

		[GrammarDefinition("expression",
			"assign_expression",
			"define_var_expression", "lambda_expression", "RETURN expression"	
			)]
		public static void expression(CompilerContext context){

		}


		//p0: = == -= throw <<= >>= *= &= /= |= ^=  ?:
		[GrammarDefinition("assign_expression", "ID ASSIGN expression", "logical_or_expression")]
		public static void assign_expression(CompilerContext context)
		{

		}

		//p1: ||
		[GrammarDefinition("logical_or_expression", "logical_and_expression",
		"logical_or_expression LOGICAL_OR logical_and_expression")]
		public static void logical_or_expression(CompilerContext context) { }

		//p2: &&
		[GrammarDefinition("logical_and_expression", "equality_expression",
			"logical_and_expression LOGICAL_AND equality_expression")]
		public static void logical_and_expression(CompilerContext context) { }


		//p6: == !=
		[GrammarDefinition("equality_expression", "relational_expression", "equality_expression EQ relational_expression"
			, "equality_expression NE relational_expression")]
		public static void equality_expression(CompilerContext context) { }

		//p7: <= <  >= >
		[GrammarDefinition("relational_expression", "additive_expression",
			"relational_expression GT additive_expression", "relational_expression GE additive_expression",
			"relational_expression LT additive_expression", "relational_expression LE additive_expression")]
		public static void relational_expression(CompilerContext context)
		{
		}

		//p9: + -
		[GrammarDefinition("additive_expression", "additive_expression ADD multiplicative_expression", "additive_expression SUB multiplicative_expression", "multiplicative_expression")]
		public static void additive_expression(CompilerContext context)
		{

		}
		//p10: * / %
		[GrammarDefinition("multiplicative_expression", "unary_expression", "multiplicative_expression MUL unary_expression", "multiplicative_expression DIV unary_expression", "multiplicative_expression MOD unary_expression")]
		public static void multiplicative_expression(CompilerContext context)
		{

		}

		//p11: + - ++ --(front) ! &*Ö¸Õë new

		[GrammarDefinition("unary_expression", "access_expression",  "SUB unary_expression")]
		public static void unary_expression(CompilerContext context)
		{

		}

		//p12: (call) (grupy) [] . -> ++ --ºós
		[GrammarDefinition("access_expression", "primary_expression",
			"ID LP RP", "ID LP augument_list RP", "LP expression RP", "native_expression")]
		public static void access_expression(CompilerContext context)
		{

		}


		//p13- final
		[GrammarDefinition("primary_expression", "DOUBLE_LITERAL", "STRING",
			"BREAK", "CONTINUE", "FALSE_T", "TRUE_T", "NULL_T", "ID")]
		public static void primary_expression(CompilerContext context)
		{
			
		}
		[GrammarDefinition("define_var_expression", "LET ID ASSIGN expression", "VAR ID ASSIGN expression")]
		public static void define_var_expression(CompilerContext context)
		{
		}


		[GrammarDefinition("native_expression", "SHARP access_list LP augument_list RP", "SHARP access_list LP RP")]
		public static void native_expression(CompilerContext context)
		{
		}




		[GrammarDefinition("lambda_expression", "LAMBDA LP params_list RP INTRODUCE statement")]
		public static void lambda_expression()
		{
		}

	}
}