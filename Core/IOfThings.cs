namespace UiIoT.Core
{
    public interface IOfThings
    {
        public string type { get; set; }
        public  string name { get; set; }
        public  string hostname { get; set; }
        public  int port { get; set; }
        public  Thread thread { get; set; }
        public  UDP UDP { get; set; }
    }
}
