
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using YaccLexCS.code.structure;
namespace YaccLexCS.ycomplier.code.structure
{
	public enum SpecialValue
	{
		BREAK,
		CONTINUE
	}
	public static class InterpreterHelper
    {
		
		public static dynamic BasicTypesValueExtract(ASTTerminalNode terminalNode, RuntimeContext context)
        {
            switch (terminalNode.Token.Type)
            {
				case "ID":
					return context.GetCurrentCommonFrame().GetLocalVar(terminalNode.Token.SourceText);
				case "STRING":
					return terminalNode.Token.SourceText;
				case "DOUBLE_LITERAL":
					return double.Parse(terminalNode.Token.SourceText);
				case "TRUE_T":
					return 1;
				case "FALSE_T":
					return 0;
				case "BREAK":
					return SpecialValue.BREAK;
				case "CONTINUE":
					return SpecialValue.CONTINUE;
			}
			return null;
        }
	}
		public static class EvaluationConfiguration
		{
				public static readonly Dictionary<string, MethodInfo> ClassNameMapping
						= new Lazy<Dictionary<string, MethodInfo>>(() =>
						{
								 var methods = typeof(EvaluationConfiguration).GetMethods();
								 var result = methods
								 .Where(m => m.IsStatic)
								 .ToDictionary(kv => kv.Name, kv => kv);
								return result;
						}).Value;

				public static dynamic CompileUnitNode(CompileUnitNode node, ycomplier.RuntimeContext context)
				{
						/*definition_or_statement*/
						if(node.Count() == 1 
								 && node[0].GetType().IsAssignableFrom(typeof(DefinitionOrStatementNode)))
						{
								return node[0].Eval(context);
						}
						/*compile_unit definition_or_statement*/
						if(node.Count() == 2 
								 && node[0].GetType().IsAssignableFrom(typeof(CompileUnitNode))
								 && node[1].GetType().IsAssignableFrom(typeof(DefinitionOrStatementNode)))
						{
								node[0].Eval(context);
								return node[1].Eval(context);
						}
						return null;
				}


				public static dynamic DefinitionOrStatementNode(DefinitionOrStatementNode node, ycomplier.RuntimeContext context)
				{
						//throw new NotImplementedException();
						/*statement*/
						if(node.Count() == 1 
								 && node[0].GetType().IsAssignableFrom(typeof(StatementNode)))
						{
								return node[0].Eval(context);
						}
			return null;
				}


				public static dynamic StatementNode(StatementNode node, ycomplier.RuntimeContext context)
				{

			/*if_statement*/
			if (node.Count() == 1
					 && node[0].GetType().IsAssignableFrom(typeof(IfStatementNode)))
            {
				return node[0].Eval(context);
            }

			/*for_statement*/
			if (node.Count() == 1
					 && node[0].GetType().IsAssignableFrom(typeof(ForStatementNode)))
			{
				return node[0].Eval(context);
			}
			/*while_statement*/
			if (node.Count() == 1 
								 && node[0].GetType().IsAssignableFrom(typeof(WhileStatementNode)))
						{
								return node[0].Eval(context);
						}
						/*expression SEMICOLON*/
						if(node.Count() == 2 
								 && node[0].GetType().IsAssignableFrom(typeof(ExpressionNode))
								 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode)))
						{
				$"expression;".PrintToConsole();
								
								return node[0].Eval(context);
			}
			return null;
				}


				public static dynamic WhileStatementNode(WhileStatementNode node, ycomplier.RuntimeContext context)
				{
						$"begin while!".PrintToConsole();
						
						/*WHILE LP expression RP block*/
						if(node.Count() == 5 
								 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
								 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
								 && node[2].GetType().IsAssignableFrom(typeof(ExpressionNode))
								 && node[3].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
								 && node[4].GetType().IsAssignableFrom(typeof(BlockNode)))
						{
				var condExp = node[2];
				var block = node[4];
				var condVal = condExp.Eval(context);
				$"now cond = {condVal}".PrintToConsole();
				if (condVal == null || condVal == 0) return null;
				var blockVal = block.Eval(context);
				if ((blockVal is SpecialValue v) && (v == SpecialValue.BREAK))
				{
					$"while break;".PrintToConsole();
					return null;
				}
				$"while next".PrintToConsole();
				return WhileStatementNode(node, context);
						}
			return null;
				}


				public static dynamic IfStatementNode(IfStatementNode node, ycomplier.RuntimeContext context)
				{
			"enter if stat".PrintToConsole();
						/*IF LP expression RP block*/
						if(node.Count() == 5 
								 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
								 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
								 && node[2].GetType().IsAssignableFrom(typeof(ExpressionNode))
								 && node[3].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
								 && node[4].GetType().IsAssignableFrom(typeof(BlockNode)))
            {
				var cond = node[2].Eval(context);
				if (cond == 0)
					return null;
				"if is true".PrintToConsole();
				return node[4].Eval(context);
            }
			return null;
		}


				public static dynamic StatementListNode(StatementListNode node, ycomplier.RuntimeContext context)
				{
					
						/*statement*/
						if(node.Count() == 1 
								 && node[0].GetType().IsAssignableFrom(typeof(StatementNode)))
						{
								return node[0].Eval(context);
						}
						/*statement_list statement*/
						if(node.Count() == 2 
								 && node[0].GetType().IsAssignableFrom(typeof(StatementListNode))
								 && node[1].GetType().IsAssignableFrom(typeof(StatementNode)))
						{
				var v = node[0].Eval(context);
				if (v is SpecialValue && (v == SpecialValue.BREAK || v == SpecialValue.CONTINUE)) return v;
 								return node[1].Eval(context);
						}
			return null;
				}


				public static dynamic ExpressionNode(ExpressionNode node, ycomplier.RuntimeContext context)
				{
			/*relational_expression*/
			if (node.Count() == 1
					 && node[0].GetType().IsAssignableFrom(typeof(RelationalExpressionNode)))
			{
				return node.Eval(context);
			}
			/*equality_expression*/
			if (node.Count() == 1
					 && node[0].GetType().IsAssignableFrom(typeof(EqualityExpressionNode)))
			{
				return node.Eval(context);
			}
			/*logical_or_expression*/
			if (node.Count() == 1
					 && node[0].GetType().IsAssignableFrom(typeof(LogicalOrExpressionNode)))
			{
				return node[0].Eval(context);
			}
			/*logical_and_expression*/
			if (node.Count() == 1
					 && node[0].GetType().IsAssignableFrom(typeof(LogicalAndExpressionNode)))
			{
				return node[0].Eval(context);
			}
			/*terminal_expression*/
			if (node.Count() == 1 
								 && node[0].GetType().IsAssignableFrom(typeof(TerminalExpressionNode)))
			{
				return node[0].Eval(context);
			}
						/*assign_expression*/
						if(node.Count() == 1 
								 && node[0].GetType().IsAssignableFrom(typeof(AssignExpressionNode)))
						{
				                return node[0].Eval(context);
						}
						/*additive_expression*/
						if(node.Count() == 1 
								 && node[0].GetType().IsAssignableFrom(typeof(AdditiveExpressionNode)))
						{
								return node[0].Eval(context);
						}
						/*multiplicative_expression*/
						if(node.Count() == 1 
								 && node[0].GetType().IsAssignableFrom(typeof(MultiplicativeExpressionNode)))
						{
				return node[0].Eval(context);
						}
						/*unary_expression*/
						if(node.Count() == 1 
								 && node[0].GetType().IsAssignableFrom(typeof(UnaryExpressionNode)))
						{
				return node[0].Eval(context);
						}
            /*primary_expression*/
            if (node.Count() == 1
                     && node[0].GetType().IsAssignableFrom(typeof(PrimaryExpressionNode)))
            {
							return node[0].Eval(context);
            }
			/*define_var_expression*/
			if(node.Count() == 1 && node[0].GetType().IsAssignableFrom(typeof(DefineVarExpressionNode)))
            {
				return node[0].Eval(context);
            }
			return null;
				}


				public static dynamic AssignExpressionNode(AssignExpressionNode node, ycomplier.RuntimeContext context)
				{
						/*ID ASSIGN expression*/
						if(node.Count() == 3 
								 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
								 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
								 && node[2].GetType().IsAssignableFrom(typeof(ExpressionNode)))
						{
							var id = ((ASTTerminalNode)node[0]).Token.SourceText;
							var exp = node[2];
							var val = exp.Eval(context); //right side
														 //put variable to memory
				context.GetCurrentCommonFrame().SetLocalVar(id, val);
							$"set {id} = {val}".PrintToConsole();
							return val;
						}
			return null;
				}


				public static dynamic TerminalExpressionNode(TerminalExpressionNode node, ycomplier.RuntimeContext context)
				{
						/*ID*/
						if(node.Count() == 1 
								 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode)))
						{
								return null;
						}
						/*DOUBLE_LITERAL*/
						if(node.Count() == 1 
								 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode)))
						{
								return null;
						}
						/*STRING*/
						if(node.Count() == 1 
								 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode)))
						{
								return null;
						}
			return null;
				}


				public static dynamic AdditiveExpressionNode(AdditiveExpressionNode node, ycomplier.RuntimeContext context)
				{
						
						/*additive_expression ADD multiplicative_expression*/
						if(node.Count() == 3 
								 && node[0].GetType().IsAssignableFrom(typeof(AdditiveExpressionNode))
								 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
								 && node[2].GetType().IsAssignableFrom(typeof(MultiplicativeExpressionNode)))
						{
				var exp1 = node[0].Eval(context);
				var exp2 = node[2].Eval(context);
				return exp1 + exp2;
			}
						/*multiplicative_expression*/
						if(node.Count() == 1 
								 && node[0].GetType().IsAssignableFrom(typeof(MultiplicativeExpressionNode)))
						{
								return node[0].Eval(context);
						}
						return null;
				}


				public static dynamic MultiplicativeExpressionNode(MultiplicativeExpressionNode node, ycomplier.RuntimeContext context)
				{
						
						/*unary_expression*/
						if(node.Count() == 1 
								 && node[0].GetType().IsAssignableFrom(typeof(UnaryExpressionNode)))
						{
				return node[0].Eval(context);
						}
						/*multiplicative_expression MUL unary_expression*/
						if(node.Count() == 3 
								 && node[0].GetType().IsAssignableFrom(typeof(MultiplicativeExpressionNode))
								 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
								 && node[2].GetType().IsAssignableFrom(typeof(UnaryExpressionNode)))
						{
				var exp1 = node[0].Eval(context);
				var exp2 = node[2].Eval(context);
				return exp1 * exp2;
            }
            /*multiplicative_expression DIV unary_expression*/
            if (node.Count() == 3
                     && node[0].GetType().IsAssignableFrom(typeof(MultiplicativeExpressionNode))
								 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
								 && node[2].GetType().IsAssignableFrom(typeof(UnaryExpressionNode)))
			{
				var exp1 = node[0].Eval(context);
				var exp2 = node[2].Eval(context);
				return exp1 / exp2;
			}
            /*multiplicative_expression MOD unary_expression*/
            if (node.Count() == 3
                     && node[0].GetType().IsAssignableFrom(typeof(MultiplicativeExpressionNode))
								 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
								 && node[2].GetType().IsAssignableFrom(typeof(UnaryExpressionNode)))
            {
				var exp1 = node[0].Eval(context);
				var exp2 = node[2].Eval(context);
				return exp1 % exp2;
			}
			return null;
        }


		public static dynamic UnaryExpressionNode(UnaryExpressionNode node, ycomplier.RuntimeContext context)
		{

            /*primary_expression*/
            if (node.Count() == 1
                     && node[0].GetType().IsAssignableFrom(typeof(PrimaryExpressionNode)))
            {
								return node[0].Eval(context);
            }
			/*SUB unary_expression*/
			if (node.Count() == 2
					 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
								 && node[1].GetType().IsAssignableFrom(typeof(UnaryExpressionNode)))
			{
								return -node[1].Eval(context);
			}
			return null;
		}


		public static dynamic PrimaryExpressionNode(PrimaryExpressionNode node, ycomplier.RuntimeContext context)
		{
			/*LP expression RP*/
			if (node.Count() == 3
					 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
					 && node[1].GetType().IsAssignableFrom(typeof(ExpressionNode))
					 && node[2].GetType().IsAssignableFrom(typeof(ASTTerminalNode)))
			{
				return node[1].Eval(context);
			}
			/*ID DOUBLE_LITERAL STRING BREAK CONTINUE RETURN ...*/
			if (node.Count() == 1
								 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode)))
			{
				
				return InterpreterHelper.BasicTypesValueExtract(node[0] as ASTTerminalNode, context);
				
			}
			return null;
		}


		public static dynamic BlockNode(BlockNode node, ycomplier.RuntimeContext context)
		{
			/*LC statement_list RC*/
			if(node.Count() == 3 
								 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
								 && node[1].GetType().IsAssignableFrom(typeof(StatementListNode))
								 && node[2].GetType().IsAssignableFrom(typeof(ASTTerminalNode)))
			{
				var bVal = node[1].Eval(context); ;
				return bVal;
            }
            /*LC RC*/
            if (node.Count() == 2
                     && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
								 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode)))
            {
								return null;
            }
			return null;
        }


		public static dynamic LogicalAndExpressionNode(LogicalAndExpressionNode node, ycomplier.RuntimeContext context)
		{
			/*equality_expression*/
			if (node.Count() == 1
					 && node[0].GetType().IsAssignableFrom(typeof(EqualityExpressionNode)))
			{
				return node[0].Eval(context);
			}
			/*logical_and_expression LOGICAL_AND equality_expression*/
			if (node.Count() == 3
					 && node[0].GetType().IsAssignableFrom(typeof(LogicalAndExpressionNode))
					 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
					 && node[2].GetType().IsAssignableFrom(typeof(EqualityExpressionNode)))
			{
				return (node[0].Eval(context) == 1) && (node[2].Eval(context) == 1);
			}
			return null;
		}


		public static dynamic LogicalOrExpressionNode(LogicalOrExpressionNode node, ycomplier.RuntimeContext context)
		{
			/*logical_and_expression*/
			if (node.Count() == 1
					 && node[0].GetType().IsAssignableFrom(typeof(LogicalAndExpressionNode)))
			{
				return node[0].Eval(context);
			}
			/*logical_or_expression LOGICAL_OR logical_and_expression*/
			if (node.Count() == 3
					 && node[0].GetType().IsAssignableFrom(typeof(LogicalOrExpressionNode))
					 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
					 && node[2].GetType().IsAssignableFrom(typeof(LogicalAndExpressionNode)))
			{
				return (node[0].Eval(context) == 1) || (node[2].Eval(context) == 1);
			}
			return null;
		}


		public static dynamic RelationalExpressionNode(RelationalExpressionNode node, ycomplier.RuntimeContext context)
		{

			/*additive_expression*/
			if (node.Count() == 1
					 && node[0].GetType().IsAssignableFrom(typeof(AdditiveExpressionNode)))
			{
				return node[0].Eval(context);
			}

			/*relational_expression GT additive_expression*/
			/*relational_expression GE additive_expression*/
			/*relational_expression LT additive_expression*/
			/*relational_expression LE additive_expression*/

			if (node.Count() == 3
					 && node[0].GetType().IsAssignableFrom(typeof(RelationalExpressionNode))
					 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode)) 
					 && node[2].GetType().IsAssignableFrom(typeof(AdditiveExpressionNode)))
			{
				switch ((node[1] as ASTTerminalNode).Token.Type)
                {
					case "GT":
						return (node[0].Eval(context) > node[2].Eval(context)) ? 1 : 0;
					case "GE":
						return (node[0].Eval(context) >= node[2].Eval(context)) ? 1 : 0;
					case "LT":
						return (node[0].Eval(context) < node[2].Eval(context)) ? 1 : 0;
					case "LE":
						return (node[0].Eval(context) <= node[2].Eval(context)) ? 1 : 0;
				}
				
			}
			
			return null;
		}


		public static dynamic EqualityExpressionNode(EqualityExpressionNode node, ycomplier.RuntimeContext context)
		{
			/*relational_expression*/
			if (node.Count() == 1
					 && node[0].GetType().IsAssignableFrom(typeof(RelationalExpressionNode)))
			{
				return node[0].Eval(context);
			}
			/*equality_expression EQ relational_expression*/
			if (node.Count() == 3
					 && node[0].GetType().IsAssignableFrom(typeof(EqualityExpressionNode))
					 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
					 && node[2].GetType().IsAssignableFrom(typeof(RelationalExpressionNode)))
			{
				return (node[0].Eval(context) == node[2].Eval(context)) ? 1 : 0;
			}
			/*equality_expression NE relational_expression*/
			if (node.Count() == 3
					 && node[0].GetType().IsAssignableFrom(typeof(EqualityExpressionNode))
					 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
					 && node[2].GetType().IsAssignableFrom(typeof(RelationalExpressionNode)))
            {
				return (node[0].Eval(context) != node[2].Eval(context)) ? 1 : 0;
			}
			return null;
		}

		public static dynamic DefineVarExpressionNode(DefineVarExpressionNode node, RuntimeContext context)
		{
			
			/*LET ID ASSIGN expression*/
			/*VAR ID ASSIGN expression*/
			if (node.Count() == 4
					 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
					 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
					 && node[2].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
					 && node[3].GetType().IsAssignableFrom(typeof(ExpressionNode)))
			{
				//still need to complete
				
				var id = (node[1] as ASTTerminalNode).Token.SourceText;
				var expVal = node[3].Eval(context);
				$"create {id} = {expVal}".PrintToConsole();
				context.GetCurrentCommonFrame().SetLocalVar(id, expVal);
				return expVal;
			}
			return null;
			
		}
		public static dynamic ForStatementNode(ForStatementNode node, RuntimeContext context)
		{
			/*FOR LP expression SEMICOLON expression SEMICOLON expression RP block*/
			if (node.Count() == 9
					 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
					 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
					 && node[2].GetType().IsAssignableFrom(typeof(ExpressionNode))
					 && node[3].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
					 && node[4].GetType().IsAssignableFrom(typeof(ExpressionNode))
					 && node[5].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
					 && node[6].GetType().IsAssignableFrom(typeof(ExpressionNode))
					 && node[7].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
					 && node[8].GetType().IsAssignableFrom(typeof(BlockNode)))
			{
				$"entre for stat".PrintToConsole();
				var initExp = node[2];
				var condNode = node[4];
				var next = node[6];
				var body = node[8];
				initExp.Eval(context); //init first
				var cond = condNode.Eval(context);
				object res = null;
				while(cond != 0)
                {
					var bodyVal = body.Eval(context);
					if((bodyVal is SpecialValue v))
                    {
						if (v == SpecialValue.BREAK) break;
						if (v == SpecialValue.CONTINUE) continue;
                    }
					next.Eval(context);
                }
				return res;
			}
			return null;
		}

	}
}