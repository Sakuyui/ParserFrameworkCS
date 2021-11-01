using YaccLexCS.ycomplier.attribution;

namespace YaccLexCS.config.grammars
{
    [GrammarConfiguration]
    public class GRMExpression
    {
        [GrammarDefinition("expression",  "terminal_expression", "assign_expression", "additive_expression",
            "multiplicative_expression", "unary_expression", "primary_expression")]
        public static void Expression()
        {
            
        }
        
        
        
        
        
        [GrammarDefinition("assign_expression",  "ID ASSIGN expression")]
        public static void AssignExpression()
        {
            
        }

        
        [GrammarDefinition("terminal_expression",  "ID", "DOUBLE_LITERAL", "STRING")]
        public static void TerminalExpression()
        {
            
        }
        
        [GrammarDefinition("additive_expression",  "additive_expression ADD multiplicative_expression","multiplicative_expression")]
        public static void AdditiveExpression()
        {
            
        }
        
        [GrammarDefinition("multiplicative_expression",  "unary_expression", "multiplicative_expression MUL unary_expression",
            "multiplicative_expression DIV unary_expression", "multiplicative_expression MOD unary_expression")]
        public static void MultiplicativeExpression()
        {
            
        }
        
        /*//一元表达式
unary_expression
: primary_expression
| SUB unary_expression
{
	$$ = crb_create_minus_experssion($2);
}*/
        [GrammarDefinition("unary_expression",  "primary_expression", "SUB unary_expression")]
        public static void UnaryExpression()
        {
            
        }
        
        [GrammarDefinition("primary_expression",  "LP expression RP", "ID", "DOUBLE_LITERAL", "STRING")]
        public static void PrimaryExpression()
        {
            
        }
        
    }
}