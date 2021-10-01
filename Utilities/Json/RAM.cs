using OpenHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenHardwareMonitor.Utilities.Json
{
    internal class RAM : ISerializableHardware
    {
        private readonly ISensor loadSensor;
        private readonly ISensor usedSensor;
        private readonly ISensor availableMemSensor;

        public string Name { get; private set; }

        public RAM(IHardware ram)
        {
            Name = ram.Name;
            loadSensor = ram.Sensors.FindByName("Memory").First();
            usedSensor = ram.Sensors.FindByName("Used Memory").First();
            availableMemSensor = ram.Sensors.FindByName("Available Memory").First();
        }

        public Json GetJson()
        {
            var json = new Json();
            json.AddValue("name", Name);
            json.AddValue("usedPr", Math.Round(loadSensor?.Value ?? -1, 2).ToString());

            var toralMem = Math.Round((usedSensor?.Value + availableMemSensor?.Value) ?? -1, 1);
            json.AddValue("total", toralMem.ToString());

            return json;
        }
    }
}
