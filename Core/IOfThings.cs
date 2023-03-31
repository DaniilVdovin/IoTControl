namespace UiIoT.Core
{
    public interface IOfThings
    {
        public abstract string type { get; set; }
        public abstract string name { get; set; }
        public abstract string hostname { get; set; }
        public abstract int port { get; set; }
        public abstract Thread thread { get; set; }
        public abstract UDP UDP { get; set; }
    }
}
