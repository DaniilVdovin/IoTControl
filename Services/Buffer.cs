using System.Globalization;
using System.Security.Cryptography;

namespace UiIoT.Services
{
    public class Buffer<T> : Queue<T>
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
    public static class BufferExtensions
    {
        public static Point AddNewRandomPoint(this Buffer<Point> buffer)
        {
            var now = DateTime.Now.AddMonths(buffer.TotalItemsAddedCount);
            var year = now.Year;
            var monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(now.Month);
            var point = new Point($"{now.Year} ({now.Month})", RandomNumberGenerator.GetInt32(1, 11));
            buffer.Add(point);
            return point;
        }
    }
}
