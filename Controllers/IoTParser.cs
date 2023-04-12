using UiIoT.Core;
using UiIoT.Models;

namespace UiIoT.Controllers
{
    public  class IoTParser
    {
        //[M:6:0:32:2574:1756:2231:1152:641:550
        //T:6:0:32:44:43:46:43:48:0
        //L:6:0:32:2041:2226:2088:2070:2080:2048]

        public IoT Parse(string data)
        {
            string[] subs = data.Replace("\n", "").Split('#');
            _DataHolder _DataHolder = new _DataHolder();
            foreach(string s in subs)
            {

                switch (s.Split(':')[0]) // ;;L;;:< check  6:0:32:2041:2226:2088:2070:2080:2048]
                {
                    case "M":
                            _DataHolder.M = s.Split(":");
                        break;
                    case "T":
                        _DataHolder.M = s.Split(":");
                        break;
                    case "L":
                        _DataHolder.M = s.Split(":");
                        break;

                }
               
            }

            Random dr = new Random();
            int v = dr.Next(0, 5);
            IoT model = new IoT(v.ToString(), v.ToString(), v.ToString(), 12,_DataHolder);
            return model;

            


        }
        public IoT Parse(string data,string conf)
        {
            string[] subs = data.Replace("\n", "").Split('#');
            _DataHolder _DataHolder = new _DataHolder();
            foreach (string s in subs)
            {

                switch (s.Split(':')[0]) // ;;L;;:< check  6:0:32:2041:2226:2088:2070:2080:2048]
                {
                    case "M":
                        _DataHolder.M = s.Split(":");
                        break;
                    case "T":
                        _DataHolder.M = s.Split(":");
                        break;
                    case "L":
                        _DataHolder.M = s.Split(":");
                        break;

                }

            }
            Random dr = new Random();
            int v = dr.Next(0, 5);
            IoT model = new IoT(v.ToString(), v.ToString(), v.ToString(), 12, _DataHolder);
            return model;







        }
    }
}
   
