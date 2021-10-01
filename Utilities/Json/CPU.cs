using OpenHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenHardwareMonitor.Utilities.Json
{
    internal class CPU : ISerializableHardware
    {
        private class Core
        {
            public int CoreNum;
            public IEnumerable<ISensor> TempSensors;
            public ISensor LoadSensor;
            public ISensor ClockSensor;
        }
        private readonly List<Core> cores;

        public string Name { get; private set; }

        public CPU(IHardware cpu)
        {
            cores = new List<Core>();
            Name = cpu.Name;

            int coreCount = 0;
            while (true)
            {
                var sensors = cpu.Sensors.FindByName("CPU Core #" + (coreCount + 1));

                if (sensors.Count != 0)
                {
                    cores.Add(new Core
                    {
                        CoreNum = coreCount + 1,
                        TempSensors = sensors.FindByType(SensorType.Temperature),
                        LoadSensor = sensors.FindByType(SensorType.Load).First(),
                        ClockSensor = sensors.FindByType(SensorType.Clock).First(),
                    });

                    coreCount++;
                }
                else
                {
                    break;
                }
            }
        }

        public Json GetJson()
        {
            var json = new Json();
            json.AddValue("name", Name);
            json.AddValue("coreCount", cores.Count.ToString());

            List<Json> coreJsons = new List<Json>();
            foreach (var core in cores)
            {
                var coreJson = new Json();
                coreJson.AddValue("temp", Math.Round(core.TempSensors.GetAvgValue()).ToString());
                coreJson.AddValue("load", Math.Round(core.LoadSensor?.Value ?? -1).ToString());
                coreJson.AddValue("clock", Math.Round(core.ClockSensor?.Value ?? -1).ToString());
                coreJson.AddValue("num", core.CoreNum.ToString());
                coreJsons.Add(coreJson);
            }

            json.AddValue("cores", coreJsons);

            return json;
        }
    }
}
