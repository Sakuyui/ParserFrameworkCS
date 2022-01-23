
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using YaccLexCS.code.structure;
using YaccLexCS.runtime;
using YaccLexCS.runtime.types;

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

		public static dynamic CompileUnitNode(CompileUnitNode node, RuntimeContext context)
        {
			
            /*definition_or_statement*/
            if (node.Count() == 1
                     && node[0].GetType().IsAssignableFrom(typeof(DefinitionOrStatementNode)))
            {
				(node[0].Eval(context) as ASTNode).Eval(context);
            }
            /*compile_unit definition_or_statement*/
            if(node.Count() == 2 
								 && node[0].GetType().IsAssignableFrom(typeof(CompileUnitNode)) //返回一个list
								 && node[1].GetType().IsAssignableFrom(typeof(DefinitionOrStatementNode)))
            {
				var l = node[0];
				var r = node[1].Eval(context); //希望提前暴露定义
				if(r is DefinitionNode dNode)
                {
					context.RuntimeStatus |= RuntimeStatusCode.DefinGlobalFunction;
					dNode.Eval(context);
					context.RuntimeStatus &= ~RuntimeStatusCode.DefinGlobalFunction;
					return l.Eval(context);
                }
                else
                {
					l.Eval(context);
					var val = r.Eval(context);
					return val;
                }
            }
			return null;
        }
		

		public static dynamic DefinitionOrStatementNode(DefinitionOrStatementNode node, RuntimeContext context)
        {
			//throw new NotImplementedException();
			/*statement*/
			if (node.Count() == 1
					 && node[0].GetType().IsAssignableFrom(typeof(StatementNode)))
			{
				return node[0];
            }
			/*definition*/
			if (node.Count() == 1
				 && node[0].GetType().IsAssignableFrom(typeof(DefinitionNode)))
			{
				return node[0];
			}
			return null;
        }
		public static dynamic DefinitionNode(DefinitionNode node, RuntimeContext context)
		{
			/*function_definition*/
			if (node.Count() == 1
				 && node[0].GetType().IsAssignableFrom(typeof(FunctionDefinitionNode)))
			{
				
				return node[0].Eval(context);
			}
			return null;
		}
		public static dynamic FunctionDefinitionNode(FunctionDefinitionNode node, RuntimeContext context)
		{
			
			/*DYFN ID LP params_list RP block*/
			if (node.Count() == 6
				 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
				 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
				 && node[2].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
				 && node[3].GetType().IsAssignableFrom(typeof(ParamsListNode))
				 && node[4].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
				 && node[5].GetType().IsAssignableFrom(typeof(BlockNode)))
			{
				var id = (node[1] as ASTTerminalNode).Token.SourceText;
				var placeHolderList = node[3].Eval(context) as List<string>;
				$"try define function {id}({placeHolderList.Aggregate("",(a, b) => a + "," +b ).Trim(',')})".PrintToConsole();
				var func = new FunctionThunk(id, placeHolderList, node[5]!);
				if((context.RuntimeStatus & RuntimeStatusCode.DefinGlobalFunction) == 0)
                {
					$"define local function {id}".PrintCollectionToConsole();
					context.GetCurrentCommonFrame().SetLocalVar(id, func);
                }
                else
                {
					$"define global function {id}".PrintToConsole();
					context.DefineGlobalFunction(func);
				}
				
				return null;
			}
			return null;
		}
		public static dynamic StatementNode(StatementNode node, RuntimeContext context)
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
            if (node.Count() == 2
                     && node[0].GetType().IsAssignableFrom(typeof(ExpressionNode))
								 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode)))
            {
				
                return node[0].Eval(context);
			}
			/*block*/
			if (node.Count() == 1
					 && node[0].GetType().IsAssignableFrom(typeof(BlockNode)))
			{
				InterpreterHelper.EntreNewBlock(context);
				var val = node[0].Eval(context);
				InterpreterHelper.LeaveBlock(context);
				return val;
			}
			return null;
        }


		public static dynamic WhileStatementNode(WhileStatementNode node, RuntimeContext context)
        {
						$"begin while!".PrintToConsole();
			/*WHILE LP expression RP statement*/
			if (node.Count() == 5
					 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
					 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
					 && node[2].GetType().IsAssignableFrom(typeof(ExpressionNode))
					 && node[3].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
					 && node[4].GetType().IsAssignableFrom(typeof(StatementNode)))
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


		public static dynamic IfStatementNode(IfStatementNode node, RuntimeContext context)
		{
			"enter if stat".PrintToConsole();
			/*IF LP expression RP statement*/
			if (node.Count() == 5
					 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
					 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
					 && node[2].GetType().IsAssignableFrom(typeof(ExpressionNode))
					 && node[3].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
					 && node[4].GetType().IsAssignableFrom(typeof(StatementNode)))
			{
				var cond = node[2].Eval(context);
				if (cond == 0)
					return null;
				"if is true".PrintToConsole();
				return node[4].Eval(context);
			}
			/*IF LP expression RP statement ELSE statement*/
			if (node.Count() == 7
					 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
					 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
					 && node[2].GetType().IsAssignableFrom(typeof(ExpressionNode))
					 && node[3].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
					 && node[4].GetType().IsAssignableFrom(typeof(StatementNode))
					 && node[5].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
					 && node[6].GetType().IsAssignableFrom(typeof(StatementNode)))
			{
				var cond = node[2].Eval(context);
				if (cond != 0)
					return node[4].Eval(context);
				return node[6].Eval(context); 
			}
			/*IF LP expression RP statement elsif_list*/
			if (node.Count() == 6
					 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
					 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
					 && node[2].GetType().IsAssignableFrom(typeof(ExpressionNode))
					 && node[3].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
					 && node[4].GetType().IsAssignableFrom(typeof(StatementNode))
					 && node[5].GetType().IsAssignableFrom(typeof(ElsifListNode)))
			{
				var cond = node[2].Eval(context);
				if (cond != 0)
					return node[4].Eval(context);
				return node[5].Eval(context);
			}
			/*IF LP expression RP statement elsif_list ELSE statement*/
			if (node.Count() == 8
					 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
					 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
					 && node[2].GetType().IsAssignableFrom(typeof(ExpressionNode))
					 && node[3].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
					 && node[4].GetType().IsAssignableFrom(typeof(StatementNode))
					 && node[5].GetType().IsAssignableFrom(typeof(ElsifListNode))
					 && node[6].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
					 && node[7].GetType().IsAssignableFrom(typeof(StatementNode)))
			{
				var cond = node[2].Eval(context);
				if (cond != 0)
				{
					return node[3].Eval(context);
				}
				var elistVal = node[5].Eval(context); //elselist should return a tuple (int code, any val)
				if ((elistVal is SpecialValue v) && v == SpecialValue.NoMatch)
					return node[7].Eval(context);
				return elistVal;
			}
			

			
			return null;
		}

		public static dynamic ElsifListNode(ElsifListNode node, RuntimeContext context)
		{
			/*elsif*/
			if (node.Count() == 1
					 && node[0].GetType().IsAssignableFrom(typeof(ElsifNode)))
			{
				return node[0].Eval(context);
			}
			/*elsif_list elsif*/
			if (node.Count() == 2
					 && node[0].GetType().IsAssignableFrom(typeof(ElsifListNode))
					 && node[1].GetType().IsAssignableFrom(typeof(ElsifNode)))
			{
				var elistVal = node[0].Eval(context);
				if((elistVal is SpecialValue v) && v == SpecialValue.NoMatch)
					return node[1].Eval(context);
				return elistVal;
			}
			return null;
		}
		public static dynamic ElsifNode(ElsifNode node, RuntimeContext context)
		{
			/*ELSIF LP expression RP statement*/
			if (node.Count() == 5
					 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
					 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
					 && node[2].GetType().IsAssignableFrom(typeof(ExpressionNode))
					 && node[3].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
					 && node[4].GetType().IsAssignableFrom(typeof(StatementNode)))
			{
				var cond = node[2].Eval(context);
				if (cond == 0) return SpecialValue.NoMatch;
				return node[4].Eval(context);
			}
			return null;
		}
		public static dynamic StatementListNode(StatementListNode node, RuntimeContext context)
		{
            /*statement*/
            if (node.Count() == 1
                     && node[0].GetType().IsAssignableFrom(typeof(StatementNode)))
            {
								return node[0].Eval(context);
            }
            /*statement_list statement*/
            if (node.Count() == 2
                     && node[0].GetType().IsAssignableFrom(typeof(StatementListNode))
								 && node[1].GetType().IsAssignableFrom(typeof(StatementNode)))
            {
				var v = node[0].Eval(context);
				if (v is SpecialValue && (v == SpecialValue.BREAK || v == SpecialValue.CONTINUE)) 
					return v;
				if (v is ReturnVal rVal)
				{
					return rVal.Value;
				}
				if((context.RuntimeStatus & RuntimeStatusCode.Returning) != 0)
                {
					$"return {v}".PrintToConsole();
					context.RuntimeStatus &= ~RuntimeStatusCode.Returning;
					return v;
                }
				return node[1].Eval(context);
				
			}
			return null;
		}


		public static dynamic ExpressionNode(ExpressionNode node, RuntimeContext context)
        {
			/*assign_expression*/
			if (node.Count() == 1
				 && node[0].GetType().IsAssignableFrom(typeof(AssignExpressionNode)))
			{
				return node[0].Eval(context);
			}
			/*define_var_expression*/
			if (node.Count() == 1
				 && node[0].GetType().IsAssignableFrom(typeof(DefineVarExpressionNode)))
			{
				return node[0].Eval(context);
			}
			/*lambda_expression*/
			if (node.Count() == 1
				 && node[0].GetType().IsAssignableFrom(typeof(LambdaExpressionNode)))
			{
				return node[0].Eval(context);
			}
			/*RETURN expression*/
			if (node.Count() == 2
				 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
				 && node[1].GetType().IsAssignableFrom(typeof(ExpressionNode)))
			{
				var val = node[1].Eval(context);
				context.RuntimeStatus |= RuntimeStatusCode.Returning;
				return val;
			}
			return null;
		}


		public static dynamic AssignExpressionNode(AssignExpressionNode node, RuntimeContext context)
		{
            /*ID ASSIGN expression*/
            if (node.Count() == 3
                     && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
								 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
								 && node[2].GetType().IsAssignableFrom(typeof(ExpressionNode)))
            {
				var id = ((ASTTerminalNode)node[0]).Token.SourceText;
				var exp = node[2];
				var val = exp.Eval(context); //right side
														 //put variable to memory
				context.GetCurrentCommonFrame().FindAndSetVar(id, val);
				$"set {id} = {val}".PrintToConsole();
				return val;
            }
			/*logical_or_expression*/
			if (node.Count() == 1
				 && node[0].GetType().IsAssignableFrom(typeof(LogicalOrExpressionNode)))
			{
				return node[0].Eval(context);
			}
			return null;
        }


				public static dynamic TerminalExpressionNode(TerminalExpressionNode node, RuntimeContext context)
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


		public static dynamic AdditiveExpressionNode(AdditiveExpressionNode node, RuntimeContext context)
		{

			/*additive_expression ADD multiplicative_expression*/
			/*additive_expression SUB multiplicative_expression*/
            if(node.Count() == 3 
								 && node[0].GetType().IsAssignableFrom(typeof(AdditiveExpressionNode))
								 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
								 && node[2].GetType().IsAssignableFrom(typeof(MultiplicativeExpressionNode)))
			{
				var exp1 = node[0].Eval(context);
				var op = node[1] as ASTTerminalNode;
				var exp2 = node[2].Eval(context);
                switch (op.Token.Type)
                {
					case "ADD":
						return exp1 + exp2;
					case "SUB":
						return exp1 - exp2;
                }
				return null;
			}
            /*multiplicative_expression*/
            if(node.Count() == 1 
								 && node[0].GetType().IsAssignableFrom(typeof(MultiplicativeExpressionNode)))
            {
                return node[0].Eval(context);
            }
            return null;
		}


		public static dynamic MultiplicativeExpressionNode(MultiplicativeExpressionNode node, RuntimeContext context)
		{

            /*unary_expression*/
            if (node.Count() == 1
                     && node[0].GetType().IsAssignableFrom(typeof(UnaryExpressionNode)))
            {
				return node[0].Eval(context);
			}
            /*multiplicative_expression MUL unary_expression*/
            if (node.Count() == 3
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


		public static dynamic UnaryExpressionNode(UnaryExpressionNode node, RuntimeContext context)
		{
			/*access_expression*/
			if (node.Count() == 1
				 && node[0].GetType().IsAssignableFrom(typeof(AccessExpressionNode)))
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
		public static dynamic AccessExpressionNode(AccessExpressionNode node, RuntimeContext context)
		{
			/*primary_expression*/
			if (node.Count() == 1
				 && node[0].GetType().IsAssignableFrom(typeof(PrimaryExpressionNode)))
			{
				return node[0].Eval(context);
			}
			/*ID LP RP*/
			if (node.Count() == 3
				 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
				 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
				 && node[2].GetType().IsAssignableFrom(typeof(ASTTerminalNode)))
			{
				throw new NotImplementedException($"call {(node[0] as ASTTerminalNode).Token.SourceText}");
				return null;
			}
			/*ID LP augument_list RP*/
			if (node.Count() == 4
				 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
				 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
				 && node[2].GetType().IsAssignableFrom(typeof(AugumentListNode))
				 && node[3].GetType().IsAssignableFrom(typeof(ASTTerminalNode)))
			{
				var id = (node[0] as ASTTerminalNode).Token.SourceText;
				var list = node[2].Eval(context) as List<ASTNode>;
				
				dynamic func = InterpreterHelper.BasicTypesValueExtract(node[0] as ASTTerminalNode, context);
				if(func == null)
                {
					func = context.GetGlobalFunction(id, paramsCount: list.Count);
                }
				if (func == null)
				{
					throw new Exception("Not define variable or func " + (node[0] as ASTTerminalNode).Token.SourceText);
					return null;
				}
				

				if (func is LambdaExpressionType lambdaExp)
				{
					var aList = list.Select(x => x.Eval(context)).ToList();
					var val = lambdaExp.Eval(aList ,context);
					return val;
				}else if(func is FunctionThunk functionThunk)
                {
					var aList = list.Select(x => x.Eval(context)).ToList();
					return functionThunk.Eval(context, aList);
                }
                else
                {
					throw new Exception("Not define variable or func " + (node[0] as ASTTerminalNode).Token.SourceText);
				}

				return null;
			}
			/*LP expression RP*/
			if (node.Count() == 3
				 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
				 && node[1].GetType().IsAssignableFrom(typeof(ExpressionNode))
				 && node[2].GetType().IsAssignableFrom(typeof(ASTTerminalNode)))
			{
				return node[1].Eval(context);
			}
			return null;
		}

		public static dynamic PrimaryExpressionNode(PrimaryExpressionNode node, RuntimeContext context)
		{
			
			/*ID DOUBLE_LITERAL STRING BREAK CONTINUE RETURN ...*/
			if (node.Count() == 1
								 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode)))
			{
				
				return InterpreterHelper.BasicTypesValueExtract(node[0] as ASTTerminalNode, context);
				
			}
			return null;
		}


		public static dynamic BlockNode(BlockNode node, RuntimeContext context)
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


		public static dynamic LogicalAndExpressionNode(LogicalAndExpressionNode node, RuntimeContext context)
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


		public static dynamic LogicalOrExpressionNode(LogicalOrExpressionNode node, RuntimeContext context)
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


		public static dynamic RelationalExpressionNode(RelationalExpressionNode node, RuntimeContext context)
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


		public static dynamic EqualityExpressionNode(EqualityExpressionNode node, RuntimeContext context)
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
			/*FOR LP expression SEMICOLON expression SEMICOLON expression RP statement*/
			if (node.Count() == 9
					 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
					 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
					 && node[2].GetType().IsAssignableFrom(typeof(ExpressionNode))
					 && node[3].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
					 && node[4].GetType().IsAssignableFrom(typeof(ExpressionNode))
					 && node[5].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
					 && node[6].GetType().IsAssignableFrom(typeof(ExpressionNode))
					 && node[7].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
					 && node[8].GetType().IsAssignableFrom(typeof(StatementNode)))
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
		public static dynamic LambdaExpressionNode(LambdaExpressionNode node, RuntimeContext context)
		{
			/*LAMBDA LP params_list RP INTRODUCE statement*/
			if (node.Count() == 6
				 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
				 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
				 && node[2].GetType().IsAssignableFrom(typeof(ParamsListNode))
				 && node[3].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
				 && node[4].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
				 && node[5].GetType().IsAssignableFrom(typeof(StatementNode)))
			{
				var placeHolderList = node[2].Eval(context);
				var statement = node[5] as StatementNode;
				LambdaExpressionType ltype = new(placeHolderList, statement);

				return ltype;
			}
			return null;
		}

		public static dynamic AugumentListNode(AugumentListNode node, RuntimeContext context)
		{
			/*augument_list COMMA augument*/
			if (node.Count() == 3
				 && node[0].GetType().IsAssignableFrom(typeof(AugumentListNode))
				 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
				 && node[2].GetType().IsAssignableFrom(typeof(AugumentNode)))
			{
				var list = node[0].Eval(context) as List<ASTNode>; //Lazy style, just get ASTNode, not really eval
				list.Add(node[2].Eval(context));
				return list;
			}
			/*augument*/
			if (node.Count() == 1
				 && node[0].GetType().IsAssignableFrom(typeof(AugumentNode)))
			{
				var list = new List<ASTNode>() { node[0].Eval(context)};
				return list;
			}
			return null;
		}
		public static dynamic AugumentNode(AugumentNode node, RuntimeContext context)
		{
			/*expression*/
			if (node.Count() == 1
				 && node[0].GetType().IsAssignableFrom(typeof(ExpressionNode)))
			{
				return node[0];
			}
			return null;
		}
		
		public static dynamic ParamsListNode(ParamsListNode node, RuntimeContext context)
		{
			/*params_list COMMA param*/
			if (node.Count() == 3
				 && node[0].GetType().IsAssignableFrom(typeof(ParamsListNode))
				 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
				 && node[2].GetType().IsAssignableFrom(typeof(ParamNode)))
			{
				var list = node[0].Eval(context) as List<string>;
				list.Add(node[2].Eval(context));
				return list;
			}
			/*param*/
			if (node.Count() == 1
				 && node[0].GetType().IsAssignableFrom(typeof(ParamNode)))
			{
				return new List<string>() { node[0].Eval(context) + "" };
			}
			return null;
		}


		public static dynamic ParamNode(ParamNode node, RuntimeContext context)
		{
			/*typeless_param*/
			if (node.Count() == 1
				 && node[0].GetType().IsAssignableFrom(typeof(TypelessParamNode)))
			{
				return node[0].Eval(context);
			}
			/*typed_param*/
			if (node.Count() == 1
				 && node[0].GetType().IsAssignableFrom(typeof(TypedParamNode)))
			{
				return node[0].Eval(context);
			}
			return null;
		}


		public static dynamic TypelessParamNode(TypelessParamNode node, RuntimeContext context)
		{
			/*ID*/
			if (node.Count() == 1
				 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode)))
			{
				return (node[0] as ASTTerminalNode).Token.SourceText;
			}
			return null;
		}


		public static dynamic TypedParamNode(TypedParamNode node, RuntimeContext context)
		{
			throw new NotImplementedException();
			/*ID ID*/
			if (node.Count() == 2
				 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
				 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode)))
			{
				return null;
			}
		}

	}
}