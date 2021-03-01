
namespace PathfindingTutorial.Data_Structures
{
    public interface IGraphSearcher<T>
    {
        public void Add(T item);
        public T Remove();
        public bool IsEmpty();

    }
}
