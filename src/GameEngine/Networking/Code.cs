﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Networking
{
    enum Code : int
    {
        Connecting = 1,
        Connected = 2,
        Ping = 3,
        Pong = 4
    }
}
