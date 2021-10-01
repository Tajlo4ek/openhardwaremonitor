using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenHardwareMonitor.Utilities.Json
{
    internal interface ISerializableHardware
    {
        String Name { get; }

        Json GetJson();
    }
}
