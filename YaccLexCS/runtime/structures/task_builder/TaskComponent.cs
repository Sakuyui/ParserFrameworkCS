using System.Collections;

namespace YaccLexCS.runtime.structures.task_builder
{
    internal class TaskComponent : IEnumerable<TaskComponent>
    {
        string name;
        List<TaskComponent> items = new List<TaskComponent>();
        public TaskComponent(string name)
        {
            this.name = name;
        }

        public void Append(TaskComponent item) { 
            items.Add(item);
        }

        public IEnumerator<TaskComponent> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator ();
        }
    }
}