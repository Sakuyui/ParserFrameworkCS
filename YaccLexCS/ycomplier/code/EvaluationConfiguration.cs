
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using YaccLexCS.code.structure;
using YaccLexCS.runtime;
using YaccLexCS.runtime.structures.task_builder;
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
            /*compilation_unit definition_or_comment*/
            if(node.Count() == 2 
                 && node[0].GetType().IsAssignableFrom(typeof(CompilationUnitNode))
                 && node[1].GetType().IsAssignableFrom(typeof(DefinitionOrCommentNode)))
            {
                node[0].Eval(context);
                return node[1].Eval(context);
            }

            /* definition_or_comment */
            if(node.Count() == 1 
                 && node[0].GetType().IsAssignableFrom(typeof(DefinitionOrCommentNode)))
            {
                return node[0].Eval(context);
            }
            return null;
        }

        public static dynamic DefinitionOrCommentNode(DefinitionOrCommentNode node, RuntimeContext context)
        {
            /*task_definition_statement*/
            if(node.Count() == 1 
                 && node[0].GetType().IsAssignableFrom(typeof(TaskDefinitionStatementNode)))
            {
                return node[0].Eval(context);
            }
            /*comment*/
            if(node.Count() == 1 
                 && node[0].GetType().IsAssignableFrom(typeof(CommentNode)))
            {
                return null;
            }
            return null;
        }

        public static dynamic CommentNode(CommentNode node, RuntimeContext context)
        {
            /*SINGLE_LINE_COMMENT*/
            if(node.Count() == 1 
                 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode)))
            {
                return null;
            }
            return null;
        }

        public static dynamic TaskDefinitionStatementNode(TaskDefinitionStatementNode node, RuntimeContext context)
        {
            /* DEF_TASK IDENTIFIER LP task_param_list RP RIGHT_ARROW typeT block */
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
                List<BindVariablePrototype> variables = new List<BindVariablePrototype>();
                (node[3] as TaskParamListNode).EvalForGetBindingVariables(variables);
                HybridTask task = 
                    BuildNamingTask(context, node[1].GetSourceText(), node[7] as BlockNode, variables, node[6] as TypetNode);
                return task;
            }
            return null;
        }

        private static HybridTask BuildNamingTask(RuntimeContext rc, String taskName, BlockNode? blockNode, List<BindVariablePrototype> variables, TypetNode? typetNode)
        {
            HybridTask task = new HybridTask() { taskName = taskName };
            TaskGraph tg = (TaskGraph) task["task_graph"];
            void buildBindArea()
            {
                for (int i = 0; i < variables.Count; i++)
                {
                    task.AppendBindVariable(TaskVariableBindType.IN, 
                        variables[i].name.GetSourceText(), 
                        variables[i].type.GetSourceText());
                }
            }

            void buildTaskGraph(ASTNode node)
            {
                rc["state"] = "building_task";
                rc["eval_node"] = node;
                rc["task_stack"] = new Stack<HybridTask>();
                (rc["task_stack"] as Stack<HybridTask>).Push(task);
                tg.AddVertex(new TGInVertex((task["bind"] as TaskBindingVariables).Select(e => (e as TaskBindItem).register).ToArray()));
                node.Eval(rc);
                rc["state"] = "";
                rc["eval_node"] = null;
            }

            buildBindArea();
            buildTaskGraph(blockNode);

            return task;
        }

        public static dynamic TaskParamListNode(TaskParamListNode node, RuntimeContext context)
        {
            throw new NotImplementedException();
            /*task_param_list COMMA task_param*/
            if(node.Count() == 3 
                 && node[0].GetType().IsAssignableFrom(typeof(TaskParamListNode))
                 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[2].GetType().IsAssignableFrom(typeof(TaskParamNode)))
            {
                return null;
            }
            /*task_param*/
            if(node.Count() == 1 
                 && node[0].GetType().IsAssignableFrom(typeof(TaskParamNode)))
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
            /*LB statements RB*/
            if(node.Count() == 3 
                 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[1].GetType().IsAssignableFrom(typeof(StatementsNode))
                 && node[2].GetType().IsAssignableFrom(typeof(ASTTerminalNode)))
            {
                return node[1].Eval(context);
            }

            /*LB RB*/
            if(node.Count() == 2 
                 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode)))
            {
                return null;
            }
            return null;
        }

        public static dynamic StatementsNode(StatementsNode node, RuntimeContext context)
        {
            /*statements statement*/
            if(node.Count() == 2 
                 && node[0].GetType().IsAssignableFrom(typeof(StatementsNode))
                 && node[1].GetType().IsAssignableFrom(typeof(StatementNode)))
            {
                node[0].Eval(context);
                return node[1].Eval(context);
            }

            if(node.Count() == 1
                && node[0].GetType().IsAssignableFrom(typeof(StatementNode))
                )
            {
                return node[0].Eval(context);
            }
            return null;
        }

        public static dynamic StatementNode(StatementNode node, RuntimeContext context)
        {
            /*expression SEMICOLON*/
            if(node.Count() == 2 
                 && node[0].GetType().IsAssignableFrom(typeof(ExpressionNode))
                 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode)))
            {
                return node[0].Eval(context);
            }
            return null;
        }

        public static dynamic ExpressionNode(ExpressionNode node, RuntimeContext context)
        {
            /*bracket_expression*/
            if(node.Count() == 1 
                 && node[0].GetType().IsAssignableFrom(typeof(BracketExpressionNode)))
            {
                return node[0].Eval(context);
            }
            /*query_expression*/
            if(node.Count() == 1 
                 && node[0].GetType().IsAssignableFrom(typeof(QueryExpressionNode)))
            {
                return node[0].Eval(context);
            }
            /*host_expression*/
            if(node.Count() == 1 
                 && node[0].GetType().IsAssignableFrom(typeof(HostExpressionNode)))
            {
                return node[0].Eval(context);
            }
            /*dsl_func_call*/
            if(node.Count() == 1 
                 && node[0].GetType().IsAssignableFrom(typeof(DslFuncCallNode)))
            {
                return node[0].Eval(context);
            }
            /*primitive_expression*/
            if(node.Count() == 1 
                 && node[0].GetType().IsAssignableFrom(typeof(PrimitiveExpressionNode)))
            {
                return node[0].Eval(context);
            }
            /*var_definition*/
            if(node.Count() == 1 
                 && node[0].GetType().IsAssignableFrom(typeof(VarDefinitionNode)))
            {
                return node[0].Eval(context);
            }
            return null;
        }


        public static dynamic VarDefinitionNode(VarDefinitionNode node, RuntimeContext context)
        {
            /*typeT IDENTIFIER*/
            if(node.Count() == 2 
                 && node[0].GetType().IsAssignableFrom(typeof(TypetNode))
                 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode)))
            {
                throw new NotImplementedException();
            }

            /*typeT IDENTIFIER var_initializer*/
            if(node.Count() == 3 
                 && node[0].GetType().IsAssignableFrom(typeof(TypetNode))
                 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[2].GetType().IsAssignableFrom(typeof(VarInitializerNode)))
            {
                if (context.RuntimeMemoryContains("state"))
                {
                    var task = (context["task_stack"] as Stack<HybridTask>).Peek();
                    var task_graph = task["task_graph"] as TaskGraph;
                    var var_reg = task.AllocateNewRegister(TaskRegisterKind.V);
                    task.nameMap.Add(node[1].GetSourceText(), var_reg);
                    task_graph.AddVertex(new TGNewVariableNode(node[0].GetSourceText(), var_reg));
                    task_graph.AddEdge(task_graph.VertexCount - 1, task_graph.VertexCount);
                    TaskRegister reg = node[2].Eval(context);
                    task_graph.AddVertex(new TGSetNode(var_reg, reg));
                    task_graph.AddEdge(task_graph.VertexCount - 1, task_graph.VertexCount);
                }
                return null;
            }
            throw new NotImplementedException();
        }


        public static dynamic VarInitializerNode(VarInitializerNode node, RuntimeContext context)
        {
            /*ASSIGN expression*/
            if(node.Count() == 2 
                 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[1].GetType().IsAssignableFrom(typeof(ExpressionNode)))
            {
                return node[1].Eval(context);
            }
            throw new NotImplementedException();
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


        public static dynamic DslFuncCallNode(DslFuncCallNode node, RuntimeContext context)
        {
            /*DSL_ACCESSOR IDENTIFIER LP call_params RP*/
            if(node.Count() == 5 
                 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[2].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[3].GetType().IsAssignableFrom(typeof(CallParamsNode))
                 && node[4].GetType().IsAssignableFrom(typeof(ASTTerminalNode)))
            {
                var task = (context["task_stack"] as Stack<HybridTask>).Peek();
                var task_graph = task["task_graph"] as TaskGraph;
                context["params_list"] = new List<TaskRegister>();
                List<TaskRegister> regs = node[3].Eval(context);
                var resultReg = task.AllocateNewRegister(TaskRegisterKind.V);
                task_graph.AddVertex(new TGDSLPrimitiveCallNode(node[1].GetSourceText(), resultReg, regs.ToArray()));
                task_graph.AddEdge(task_graph.VertexCount - 1, task_graph.VertexCount);

                context["params_list"] = null;
                return resultReg;
            }

            /*DSL_ACCESSOR IDENTIFIER LP RP*/
            if (node.Count() == 4 
                 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[2].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[3].GetType().IsAssignableFrom(typeof(ASTTerminalNode)))
            {
                if (context.RuntimeMemoryContains("state"))
                {
                    var task = (context["task_stack"] as Stack<HybridTask>).Peek();
                    var task_graph = task["task_graph"] as TaskGraph;
                    var resultReg = task.AllocateNewRegister(TaskRegisterKind.V);
                    task_graph.AddVertex(new TGDSLPrimitiveCallNode(node[1].GetSourceText(), resultReg));
                    task_graph.AddEdge(task_graph.VertexCount - 1, task_graph.VertexCount);

                    return resultReg;
                }
            }
            throw new NotImplementedException();
        }


        public static dynamic CallParamsNode(CallParamsNode node, RuntimeContext context)
        {
            /*call_params COMMA call_param*/
            if(node.Count() == 3 
                 && node[0].GetType().IsAssignableFrom(typeof(CallParamsNode))
                 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[2].GetType().IsAssignableFrom(typeof(CallParamNode)))
            {
                node[0].Eval(context);
                List<TaskRegister> registers = (List<TaskRegister>)context["params_list"];
                TaskRegister reg = node[2].Eval(context);
                registers.Add(reg);
                return registers;
            }
            /*call_param*/
            if(node.Count() == 1 
                 && node[0].GetType().IsAssignableFrom(typeof(CallParamNode)))
            {
                List<TaskRegister> registers = (List<TaskRegister>)context["params_list"];
                TaskRegister reg = node[0].Eval(context);
                registers.Add(reg);
                return registers;
            }
            throw new NotImplementedException();
        }


        public static dynamic CallParamNode(CallParamNode node, RuntimeContext context)
        {
            /*expression*/
            if(node.Count() == 1 
                 && node[0].GetType().IsAssignableFrom(typeof(ExpressionNode)))
            {

                return node[0].Eval(context);
            }
            throw new NotImplementedException();
        }


        public static dynamic PrimitiveExpressionNode(PrimitiveExpressionNode node, RuntimeContext context)
        {
            /*IDENTIFIER*/
            if(node.Count() == 1 
                 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode)))
            {
                if (context.RuntimeMemoryContains("building_task"))
                {
                    var task = (context["task_stack"] as Stack<HybridTask>).Peek();
                    if (task.nameMap.ContainsKey(node[0].GetSourceText()))
                    {
                        return task.nameMap[node[0].GetSourceText()];
                    }
                }
                return null;
            }
            /*member_access*/
            if(node.Count() == 1 
                 && node[0].GetType().IsAssignableFrom(typeof(MemberAccessNode)))
            {
                return node[0].Eval(context);
            }
            /*return primitive_express*/
            if (node.Count() == 2
                 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[1].GetType().IsAssignableFrom(typeof(PrimitiveExpressionNode))
                 )
            {
                TaskRegister reg = node[1].Eval(context);
                var task = (context["task_stack"] as Stack<HybridTask>).Peek();
                var task_graph = task["task_graph"] as TaskGraph;
                task_graph.AddVertex(new TGOutNode(reg));
                task_graph.AddEdge(task_graph.VertexCount - 1, task_graph.VertexCount);
                return reg;
            }
            throw new NotImplementedException();
        }


        public static dynamic MemberAccessNode(MemberAccessNode node, RuntimeContext context)
        {
            throw new NotImplementedException();
            /*IDENTIFIER DOT IDENTIFIER*/
            if(node.Count() == 3 
                 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[2].GetType().IsAssignableFrom(typeof(ASTTerminalNode)))
            {
                return null;
            }
            /*IDENTIFIER DOT IDENTIFIER LP RP*/
            if(node.Count() == 5 
                 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[2].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[3].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[4].GetType().IsAssignableFrom(typeof(ASTTerminalNode)))
            {
                return null;
            }
            /*IDENTIFIER DOT IDENTIFIER LP call_params RP*/
            if(node.Count() == 6 
                 && node[0].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[2].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[3].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[4].GetType().IsAssignableFrom(typeof(CallParamsNode))
                 && node[5].GetType().IsAssignableFrom(typeof(ASTTerminalNode)))
            {
                return null;
            }
        }


        public static dynamic QueryExpressionNode(QueryExpressionNode node, RuntimeContext context)
        {
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
                if (context["state"].Equals("building_task"))
                {
                    var task = (context["task_stack"] as Stack<HybridTask>).Peek();
                    var task_graph = task["task_graph"] as TaskGraph;
                    /* build anomyous task */
                    TaskRegister reg = task.AllocateNewRegister(TaskRegisterKind.T);

                    

                    /* TODO: build dependencies task*/
                    
                    return reg;
                }
                
                return null;
            }
            /*cmp_expression*/
                    if (node.Count() == 1 
                 && node[0].GetType().IsAssignableFrom(typeof(CmpExpressionNode)))
            {
                return null;
            }
            throw new NotImplementedException();
        }


        public static dynamic CmpExpressionNode(CmpExpressionNode node, RuntimeContext context)
        {
            throw new NotImplementedException();
            /*cmp_expression EQ expression*/
            if(node.Count() == 3 
                 && node[0].GetType().IsAssignableFrom(typeof(CmpExpressionNode))
                 && node[1].GetType().IsAssignableFrom(typeof(ASTTerminalNode))
                 && node[2].GetType().IsAssignableFrom(typeof(ExpressionNode)))
            {
                return null;
            }
            /*expression*/
            if(node.Count() == 1 
                 && node[0].GetType().IsAssignableFrom(typeof(ExpressionNode)))
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

    internal enum TaskVariableBindType
    {
        IN,
        OUT
    }

    internal class BindVariablePrototype
    {
        public ASTNode type;
        public ASTNode name;
        public ASTNode this[string arg]
        {
            get
            {
                switch (arg)
                {
                    case "type":
                        return this.type;
                        break;
                    case "name":
                        return this.name;
                        break;
                }
                return null;
            }
            set
            {
                switch (arg) {
                    case "type":
                        this.type = value;
                        break;
                    case "name":
                        this.name = value;
                        break;
                }
            }
        }
    }

    internal class HybridTask
    {
        TaskBindingVariables bindingVariables = new TaskBindingVariables();
        TaskDependencies dependencies = new TaskDependencies();
        Dictionary<TaskRegisterKind, int> allocatedRegisterCount = new Dictionary<TaskRegisterKind, int>();

        TaskGraph taskGraph = new TaskGraph();
        public string taskName = "";

        public Dictionary<string, TaskRegister> nameMap = new Dictionary<string, TaskRegister>();

        void ResetAllocatedRegisters()
        {
            foreach(var x in Enum.GetValues(typeof(TaskRegisterKind)).Cast<TaskRegisterKind>())
            {
                allocatedRegisterCount[x] = 0;
            }
        }
        public int GetAllocatedRegisterCount(TaskRegisterKind registerKind)
            => allocatedRegisterCount[registerKind];
        
        public HybridTask()
        {
            ResetAllocatedRegisters();
            components.Add("bind", bindingVariables);
            components.Add("task_graph", taskGraph);
            components.Add("dependencies", dependencies);
        }

        public void AppendBindVariable(TaskVariableBindType bindType, string data_type, string name)
        {
            TaskRegister register = 
                this.AllocateNewRegister(TaskRegisterKind.V);
            this.nameMap.Add(name, register);
            this["bind"].
                Append(new TaskBindItem(bindType, data_type, name, register));
        }

        Dictionary<string, TaskComponent> components = new ();
        public TaskComponent this[string name]
        {
            get => components[name];
            set => components[name] = value;
        }

        public TaskRegister AllocateNewRegister(TaskRegisterKind kind)
        {
            return new (kind, allocatedRegisterCount[kind]);
        }


    }
}