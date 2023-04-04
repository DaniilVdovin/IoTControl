namespace UiIoT.Models
{
    public class Buffer<T>:Queue<T>
    {
        public int? MaxCapacity { get; }
        public Buffer(int capacity) { MaxCapacity = capacity; }
        public int TotalItemsAddedCount { get; private set; }

        public void Add(T newElement)
        {
            if (Count == (MaxCapacity ?? -1)) Dequeue();
            Enqueue(newElement);
            TotalItemsAddedCount++;
        }
    }
}
