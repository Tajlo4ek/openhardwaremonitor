using OpenHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenHardwareMonitor.Utilities.Json
{
    static class SensorExtension
    {
        public static List<ISensor> FindByName(this IEnumerable<ISensor> sensors, string name)
        {
            var list = new List<ISensor>();

            foreach (var sensor in sensors)
            {
                if (sensor.Name == name)
                {
                    list.Add(sensor);
                }
            }

            return list;
        }

        public static List<ISensor> FindByType(this IEnumerable<ISensor> sensors, SensorType type)
        {
            var list = new List<ISensor>();

            foreach (var sensor in sensors)
            {
                if (sensor.SensorType == type)
                {
                    list.Add(sensor);
                }
            }

            return list;
        }

        public static float GetAvgValue(this IEnumerable<ISensor> sensors)
        {
            float count = 0;
            float avg = 0;

            foreach (var sensor in sensors)
            {
                var value = sensor.Value;
                var floatValue = (float)value;
                if (value != null && float.IsNaN(floatValue) == false && float.IsInfinity(floatValue) == false)
                {
                    count++;
                    avg += floatValue;
                }
            }

            return count == 0 ? -1 : avg / count;

        }

        public static ISensor First(this IEnumerable<ISensor> sensors)
        {
            if (sensors == null)
            {
                return null;
            }

            foreach (var sensor in sensors)
            {
                return sensor;
            }

            return null;
        }

    }
}
