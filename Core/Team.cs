﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace UiIoT.Core
{
    public class Team
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public List<IoT> IoTs { get; set; }

        public Team() {
            IoTs = new List<IoT>();
        }
    }
}
