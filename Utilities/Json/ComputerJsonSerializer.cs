using OpenHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenHardwareMonitor.Utilities.Json
{
    public static class ComputerJsonSerializer
    {
        private static bool isInit = false;
        private static List<ISerializableHardware> CPUs;
        private static List<ISerializableHardware> HDDs;
        private static List<ISerializableHardware> GPUs;
        private static List<ISerializableHardware> RAMs;

        public static String Serialize(Computer computer)
        {
            if (isInit == false)
            {
                CPUs = new List<ISerializableHardware>();
                HDDs = new List<ISerializableHardware>();
                GPUs = new List<ISerializableHardware>();
                RAMs = new List<ISerializableHardware>();

                foreach (var hardware in computer.Hardware)
                {
                    if (hardware.Sensors.Length == 0) { continue; }

                    switch (hardware.HardwareType)
                    {
                        case HardwareType.CPU:
                            CPUs.Add(new CPU(hardware));
                            break;
                        case HardwareType.RAM:
                            RAMs.Add(new RAM(hardware));
                            break;
                        case HardwareType.HDD:
                            HDDs.Add(new HDD(hardware));
                            break;
                        case HardwareType.GpuNvidia:
                        case HardwareType.GpuAti:
                            GPUs.Add(new GPU(hardware));
                            break;
                    }
                }

                isInit = true;
            }

            var json = new Json();
            AddJson(ref json, "cpu", GetJsons(CPUs));
            AddJson(ref json, "hdd", GetJsons(HDDs));
            AddJson(ref json, "gpu", GetJsons(GPUs));
            AddJson(ref json, "ram", GetJsons(RAMs));
            return json.Build();
        }

        private static void AddJson(ref Json json, string name, List<Json> jsons)
        {
            json.AddValue(name + "Count", jsons.Count.ToString());
            json.AddValue(name, jsons);
        }

        private static List<Json> GetJsons(IEnumerable<ISerializableHardware> hardwares)
        {
            var list = new List<Json>();

            foreach (var hardware in hardwares)
            {
                list.Add(hardware.GetJson());
            }

            return list;
        }

    }
}
