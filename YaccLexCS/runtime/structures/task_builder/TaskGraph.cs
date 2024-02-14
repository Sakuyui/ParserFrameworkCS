namespace YaccLexCS.runtime.structures.task_builder
{
    internal class TaskGraph : TaskComponent
    {
        TaskGraphVertexes _vertexes = new TaskGraphVertexes();
        TaskGraphEdges _edges = new TaskGraphEdges();

        public TaskGraph() : base("task_graph")
        {

        }

        public int VertexCount => _vertexes.Count();
        public int EdgesCount => _edges.Count();

        public void AddVertex(TaskGraphVertex vertex)
        {
            _vertexes.AddVertex(vertex);
        }

        public void AddEdge(int fromNode, int toNode)
        {
            _edges.AddEdge(fromNode, toNode);
        }
    }
}