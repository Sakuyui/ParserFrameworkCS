
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using YaccLexCS.code.structure;
namespace YaccLexCS.ycomplier.code.structure
{
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

				public static dynamic CompileUnitNode(CompileUnitNode node, CompilerContext context)
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


				public static dynamic DefinitionOrStatementNode(DefinitionOrStatementNode node, CompilerContext context)
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


				public static dynamic StatementNode(StatementNode node, CompilerContext context)
				{
					
						/*if_statement*/
						if(node.Count() == 1 
								 && node[0].GetType().IsAssignableFrom(typeof(IfStatementNode)))
						{
								return null;
						}
						/*while_statement*/
						if(node.Count() == 1 
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
								node[0].Eval(context);
								return null;
						}
			return null;
				}


				public static dynamic WhileStatementNode(WhileStatementNode node, CompilerContext context)
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
				block.Eval(context);
				$"while next".PrintToConsole();
				return WhileStatementNode(node, context);
						}
			return null;
				}


				public static dynamic IfStatementNode(IfStatementNode node, CompilerContext context)
				{
						throw new NotImplementedException();
						/*IF LP expression RP block*/
						if(node.Count() == 5 
								 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
								 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
								 && node[2].GetType().IsAssignableFrom(typeof(ExpressionNode))
								 && node[3].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
								 && node[4].GetType().IsAssignableFrom(typeof(BlockNode)))
						{
								return null;
						}
				}


				public static dynamic StatementListNode(StatementListNode node, CompilerContext context)
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
				node[0].Eval(context);
								return node[1].Eval(context);
						}
			return null;
				}


				public static dynamic ExpressionNode(ExpressionNode node, CompilerContext context)
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
						if(node.Count() == 1 
								 && node[0].GetType().IsAssignableFrom(typeof(PrimaryExpressionNode)))
						{
							return node[0].Eval(context);
						}
			return null;
				}


				public static dynamic AssignExpressionNode(AssignExpressionNode node, CompilerContext context)
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
							context[id] = val;
							$"set {id} = {val}".PrintToConsole();
							return val;
						}
			return null;
				}


				public static dynamic TerminalExpressionNode(TerminalExpressionNode node, CompilerContext context)
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


				public static dynamic AdditiveExpressionNode(AdditiveExpressionNode node, CompilerContext context)
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


				public static dynamic MultiplicativeExpressionNode(MultiplicativeExpressionNode node, CompilerContext context)
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
						if(node.Count() == 3 
								 && node[0].GetType().IsAssignableFrom(typeof(MultiplicativeExpressionNode))
								 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
								 && node[2].GetType().IsAssignableFrom(typeof(UnaryExpressionNode)))
						{
				var exp1 = node[0].Eval(context);
				var exp2 = node[2].Eval(context);
				return exp1 / exp2;
			}
						/*multiplicative_expression MOD unary_expression*/
						if(node.Count() == 3 
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


				public static dynamic UnaryExpressionNode(UnaryExpressionNode node, CompilerContext context)
				{
						
						/*primary_expression*/
						if(node.Count() == 1 
								 && node[0].GetType().IsAssignableFrom(typeof(PrimaryExpressionNode)))
						{
								return node[0].Eval(context);
						}
						/*SUB unary_expression*/
						if(node.Count() == 2 
								 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
								 && node[1].GetType().IsAssignableFrom(typeof(UnaryExpressionNode)))
						{
								return -node[1].Eval(context);
						}
			return null;
				}


		public static dynamic PrimaryExpressionNode(PrimaryExpressionNode node, CompilerContext context)
		{
			/*LP expression RP*/
			if (node.Count() == 3
					 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
					 && node[1].GetType().IsAssignableFrom(typeof(ExpressionNode))
					 && node[2].GetType().IsAssignableFrom(typeof(ASTTerminalNode)))
			{
				return node[1].Eval(context);
			}
			/*ID DOUBLE_LITERAL STRING*/
			if (node.Count() == 1
								 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode)))
			{
				var t = (node[0] as ASTTerminalNode).Token;
				switch (t.Type)
				{
					case "DOUBLE_LITERAL":
						return double.Parse(t.SourceText);
						break;
					case "ID":
						return context[t.SourceText];
						break;
					case "STRING":
						return t.SourceText;

				}

				
			}
			return null;
		}


				public static dynamic BlockNode(BlockNode node, CompilerContext context)
				{	
						/*LC statement_list RC*/
						if(node.Count() == 3 
								 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
								 && node[1].GetType().IsAssignableFrom(typeof(StatementListNode))
								 && node[2].GetType().IsAssignableFrom(typeof(ASTTerminalNode)))
						{
				return node[1].Eval(context);
						}
						/*LC RC*/
						if(node.Count() == 2 
								 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
								 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode)))
						{
								return null;
						}
			return null;
				}


		public static dynamic LogicalAndExpressionNode(LogicalAndExpressionNode node, CompilerContext context)
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


		public static dynamic LogicalOrExpressionNode(LogicalOrExpressionNode node, CompilerContext context)
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


		public static dynamic RelationalExpressionNode(RelationalExpressionNode node, CompilerContext context)
		{

			/*additive_expression*/
			if (node.Count() == 1
					 && node[0].GetType().IsAssignableFrom(typeof(AdditiveExpressionNode)))
			{
				return node[0].Eval(context);
			}
			/*relational_expression GT additive_expression*/
			if (node.Count() == 3
					 && node[0].GetType().IsAssignableFrom(typeof(RelationalExpressionNode))
					 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
					 && node[2].GetType().IsAssignableFrom(typeof(AdditiveExpressionNode)))
			{
				return node[0].Eval(context) > node[2].Eval(context);
			}
			/*relational_expression GE additive_expression*/
			if (node.Count() == 3
					 && node[0].GetType().IsAssignableFrom(typeof(RelationalExpressionNode))
					 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
					 && node[2].GetType().IsAssignableFrom(typeof(AdditiveExpressionNode)))
			{
				return node[0].Eval(context) >= node[2].Eval(context);
			}
			/*relational_expression LT additive_expression*/
			if (node.Count() == 3
					 && node[0].GetType().IsAssignableFrom(typeof(RelationalExpressionNode))
					 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
					 && node[2].GetType().IsAssignableFrom(typeof(AdditiveExpressionNode)))
			{
				return node[0].Eval(context) < node[2].Eval(context);
			}
			/*relational_expression LE additive_expression*/
			if (node.Count() == 3
					 && node[0].GetType().IsAssignableFrom(typeof(RelationalExpressionNode))
					 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
					 && node[2].GetType().IsAssignableFrom(typeof(AdditiveExpressionNode)))
			{
				return node[0].Eval(context) <= node[2].Eval(context);
			}
			return null;
		}


		public static dynamic EqualityExpressionNode(EqualityExpressionNode node, CompilerContext context)
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
				return node[0].Eval(context) == node[2].Eval(context);
			}
			/*equality_expression NE relational_expression*/
			if (node.Count() == 3
					 && node[0].GetType().IsAssignableFrom(typeof(EqualityExpressionNode))
					 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
					 && node[2].GetType().IsAssignableFrom(typeof(RelationalExpressionNode)))
			{
				return node[0].Eval(context) != node[2].Eval(context);
			}
			return null;
		}


	}
}