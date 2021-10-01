using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenHardwareMonitor.Hardware;

namespace OpenHardwareMonitor.Utilities.Json
{
    internal class GPU : ISerializableHardware
    {
        public string Name { get; private set; }

        private readonly ISensor tempSensor;
        private readonly ISensor clockSensor;
        private readonly ISensor loadMemSensor;

        private readonly ISensor totalMemSensor;
        private readonly ISensor fanPrSensor;
        private readonly ISensor fanRPMSensor;

        public GPU(IHardware gpu)
        {
            Name = gpu.Name;
            Console.WriteLine(gpu.Name);
            foreach (var sensor in gpu.Sensors)
            {
                Console.WriteLine(sensor.Name + " | " + sensor.SensorType + " | " + sensor.Value);
            }
            Console.WriteLine();

            tempSensor = gpu.Sensors.FindByType(SensorType.Temperature).FindByName("GPU Core").First();
            clockSensor = gpu.Sensors.FindByType(SensorType.Clock).FindByName("GPU Core").First();
            loadMemSensor = gpu.Sensors.FindByType(SensorType.Load).FindByName("GPU Memory").First();
            totalMemSensor = gpu.Sensors.FindByType(SensorType.SmallData).FindByName("GPU Memory Total").First();

            fanRPMSensor = gpu.Sensors.FindByType(SensorType.Fan).FindByName("GPU").First();
            fanPrSensor = gpu.Sensors.FindByType(SensorType.Control).FindByName("GPU Fan").First();
        }

        public Json GetJson()
        {
            var json = new Json();
            json.AddValue("name", Name);

            json.AddValue("temp", (tempSensor?.Value ?? -1).ToString());
            json.AddValue("clock", (clockSensor?.Value ?? -1).ToString());
            json.AddValue("loadMem", Math.Round(loadMemSensor?.Value ?? -1, 2).ToString());
            json.AddValue("fanRpm", (fanRPMSensor?.Value ?? -1).ToString());
            json.AddValue("fanPr", (fanPrSensor?.Value ?? -1).ToString());
            json.AddValue("totalMem", (totalMemSensor?.Value ?? -1).ToString());


            return json;
        }
    }
}
