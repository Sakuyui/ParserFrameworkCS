
namespace YaccLexCS.runtime.structures.task_builder
{
    internal class TaskGraphVertexes
    {
        List<TaskGraphVertex> vertexes = new List<TaskGraphVertex>();
        public TaskGraphVertexes() { 
        
        }

        public void AddVertex(TaskGraphVertex node)
        {
            vertexes.Add(node);
        }

        internal int Count()
        {
            return vertexes.Count;
        }
    }
}