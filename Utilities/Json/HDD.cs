using OpenHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenHardwareMonitor.Utilities.Json
{
    internal class HDD : ISerializableHardware
    {
        public string Name { get; private set; }

        private readonly List<ISensor> tempSensors;
        private readonly ISensor usedSensor;
        private readonly ISensor totalWrittenSensor;

        public HDD(IHardware hdd)
        {
            Name = hdd.Name;

            tempSensors = hdd.Sensors.FindByName("Temperature");
            usedSensor = hdd.Sensors.FindByName("Used Space").First();
            totalWrittenSensor = hdd.Sensors.FindByName("Total Bytes Written").First();
        }

        public Json GetJson()
        {
            var json = new Json();
            json.AddValue("name", Name);
            json.AddValue("temp", Math.Round(tempSensors?.GetAvgValue() ?? -1).ToString());
            json.AddValue("used", Math.Round(usedSensor?.Value ?? -1, 1).ToString());
            json.AddValue("written", Math.Round(totalWrittenSensor?.Value ?? -1).ToString());

            return json;
        }

    }
}
