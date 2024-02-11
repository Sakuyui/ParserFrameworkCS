
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using YaccLexCS.code.structure;
using YaccLexCS.runtime;
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

        public static dynamic CompilationUnitNode(CompilationUnitNode node, RuntimeContext context)
        {
            throw new NotImplementedException();
            /*definition_or_comment compilation_unit*/
            if(node.Count() == 2 
                 && node[0].GetType().IsAssignableFrom(typeof(DefinitionOrCommentNode))
                 && node[1].GetType().IsAssignableFrom(typeof(CompilationUnitNode)))
            {
                return null;
            }
        }


        public static dynamic DefinitionOrCommentNode(DefinitionOrCommentNode node, RuntimeContext context)
        {
            throw new NotImplementedException();
            /*task_definition_statement*/
            if(node.Count() == 1 
                 && node[0].GetType().IsAssignableFrom(typeof(TaskDefinitionStatementNode)))
            {
                return null;
            }
            /*comment*/
            if(node.Count() == 1 
                 && node[0].GetType().IsAssignableFrom(typeof(CommentNode)))
            {
                return null;
            }
        }


        public static dynamic CommentNode(CommentNode node, RuntimeContext context)
        {
            throw new NotImplementedException();
            /*SINGLE_LINE_COMMENT*/
            if(node.Count() == 1 
                 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode)))
            {
                return null;
            }
        }


        public static dynamic TaskDefinitionStatementNode(TaskDefinitionStatementNode node, RuntimeContext context)
        {
            throw new NotImplementedException();
            /*DEF_TASK IDENTIFIER LP task_param_list RP RIGHT_ARROW typeT block*/
            if(node.Count() == 8 
                 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[2].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[3].GetType().IsAssignableFrom(typeof(TaskParamListNode))
                 && node[4].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[5].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[6].GetType().IsAssignableFrom(typeof(TypetNode))
                 && node[7].GetType().IsAssignableFrom(typeof(BlockNode)))
            {
                return null;
            }
        }


        public static dynamic TaskParamListNode(TaskParamListNode node, RuntimeContext context)
        {
            throw new NotImplementedException();
            /*task_param*/
            if(node.Count() == 1 
                 && node[0].GetType().IsAssignableFrom(typeof(TaskParamNode)))
            {
                return null;
            }
            /*task_param_list task_param*/
            if(node.Count() == 2 
                 && node[0].GetType().IsAssignableFrom(typeof(TaskParamListNode))
                 && node[1].GetType().IsAssignableFrom(typeof(TaskParamNode)))
            {
                return null;
            }
        }


        public static dynamic TaskParamNode(TaskParamNode node, RuntimeContext context)
        {
            throw new NotImplementedException();
            /*typeT IDENTIFIER*/
            if(node.Count() == 2 
                 && node[0].GetType().IsAssignableFrom(typeof(TypetNode))
                 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode)))
            {
                return null;
            }
        }


        public static dynamic TypetNode(TypetNode node, RuntimeContext context)
        {
            throw new NotImplementedException();
            /*host_type*/
            if(node.Count() == 1 
                 && node[0].GetType().IsAssignableFrom(typeof(HostTypeNode)))
            {
                return null;
            }
            /*dsl_type*/
            if(node.Count() == 1 
                 && node[0].GetType().IsAssignableFrom(typeof(DslTypeNode)))
            {
                return null;
            }
        }


        public static dynamic HostTypeNode(HostTypeNode node, RuntimeContext context)
        {
            throw new NotImplementedException();
            /*HOST_TYPE*/
            if(node.Count() == 1 
                 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode)))
            {
                return null;
            }
        }


        public static dynamic DslTypeNode(DslTypeNode node, RuntimeContext context)
        {
            throw new NotImplementedException();
            /*DSL_ACCESSOR IDENTIFIER*/
            if(node.Count() == 2 
                 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode)))
            {
                return null;
            }
        }


        public static dynamic BlockNode(BlockNode node, RuntimeContext context)
        {
            throw new NotImplementedException();
            /*LB statements RB*/
            if(node.Count() == 3 
                 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[1].GetType().IsAssignableFrom(typeof(StatementsNode))
                 && node[2].GetType().IsAssignableFrom(typeof(ASTTerminalNode)))
            {
                return null;
            }
        }


        public static dynamic StatementsNode(StatementsNode node, RuntimeContext context)
        {
            throw new NotImplementedException();
            /*statement statements*/
            if(node.Count() == 2 
                 && node[0].GetType().IsAssignableFrom(typeof(StatementNode))
                 && node[1].GetType().IsAssignableFrom(typeof(StatementsNode)))
            {
                return null;
            }
        }


        public static dynamic StatementNode(StatementNode node, RuntimeContext context)
        {
            throw new NotImplementedException();
            /*expression SEMICOLON*/
            if(node.Count() == 2 
                 && node[0].GetType().IsAssignableFrom(typeof(ExpressionNode))
                 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode)))
            {
                return null;
            }
        }


        public static dynamic ExpressionNode(ExpressionNode node, RuntimeContext context)
        {
            throw new NotImplementedException();
            /*bracket_expression*/
            if(node.Count() == 1 
                 && node[0].GetType().IsAssignableFrom(typeof(BracketExpressionNode)))
            {
                return null;
            }
            /*query_expression*/
            if(node.Count() == 1 
                 && node[0].GetType().IsAssignableFrom(typeof(QueryExpressionNode)))
            {
                return null;
            }
            /*host_expression*/
            if(node.Count() == 1 
                 && node[0].GetType().IsAssignableFrom(typeof(HostExpressionNode)))
            {
                return null;
            }
        }


        public static dynamic BracketExpressionNode(BracketExpressionNode node, RuntimeContext context)
        {
            throw new NotImplementedException();
            /*LP expression RP*/
            if(node.Count() == 3 
                 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[1].GetType().IsAssignableFrom(typeof(ExpressionNode))
                 && node[2].GetType().IsAssignableFrom(typeof(ASTTerminalNode)))
            {
                return null;
            }
        }


        public static dynamic HostExpressionNode(HostExpressionNode node, RuntimeContext context)
        {
            throw new NotImplementedException();
            /*HOST_EXPRESSION*/
            if(node.Count() == 1 
                 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode)))
            {
                return null;
            }
        }


        public static dynamic QueryExpressionNode(QueryExpressionNode node, RuntimeContext context)
        {
            throw new NotImplementedException();
            /*FROM IDENTIFIER IS typeT IN IDENTIFIER WHERE expression SELECT expression*/
            if(node.Count() == 10 
                 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[2].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[3].GetType().IsAssignableFrom(typeof(TypetNode))
                 && node[4].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[5].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[6].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[7].GetType().IsAssignableFrom(typeof(ExpressionNode))
                 && node[8].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[9].GetType().IsAssignableFrom(typeof(ExpressionNode)))
            {
                return null;
            }
            /*FROM IDENTIFIER IS typeT IN IDENTIFIER SELECT expression*/
            if(node.Count() == 8 
                 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[2].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[3].GetType().IsAssignableFrom(typeof(TypetNode))
                 && node[4].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[5].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[6].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[7].GetType().IsAssignableFrom(typeof(ExpressionNode)))
            {
                return null;
            }
            /*FROM IDENTIFIER IN IDENTIFIER WHERE expression SELECT expression*/
            if(node.Count() == 8 
                 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[2].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[3].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[4].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[5].GetType().IsAssignableFrom(typeof(ExpressionNode))
                 && node[6].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[7].GetType().IsAssignableFrom(typeof(ExpressionNode)))
            {
                return null;
            }
            /*FROM IDENTIFIER IN IDENTIFIER SELECT expression*/
            if(node.Count() == 6 
                 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[2].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[3].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[4].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[5].GetType().IsAssignableFrom(typeof(ExpressionNode)))
            {
                return null;
            }
        }


        public static dynamic HostBlockNode(HostBlockNode node, RuntimeContext context)
        {
            throw new NotImplementedException();
            /*HOST_BLOCK*/
            if(node.Count() == 1 
                 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode)))
            {
                return null;
            }
        }

    }
}