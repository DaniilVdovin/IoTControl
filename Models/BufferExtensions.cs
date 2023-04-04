using System.Drawing;
using System.Globalization;
using System.Security.Cryptography;

namespace UiIoT.Models
{
    public static class BufferExtensions
    {
        public record Point(string Label, int Value);
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
