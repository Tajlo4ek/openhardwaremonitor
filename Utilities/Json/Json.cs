using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenHardwareMonitor.Utilities.Json
{
    internal class Json
    {
        private readonly Dictionary<string, string> stringValues;

        private readonly Dictionary<string, Json> jsonValues;

        private readonly Dictionary<string, IEnumerable<Json>> jsonArrayValues;

        public Json()
        {
            stringValues = new Dictionary<string, string>();
            jsonValues = new Dictionary<string, Json>();
            jsonArrayValues = new Dictionary<string, IEnumerable<Json>>();
        }

        public void AddValue(string name, string value)
        {
            if (stringValues.ContainsKey(name))
            {
                throw new ArgumentException("value exist |" + name);
            }

            stringValues.Add(name, value);
        }

        public void AddValue(string name, Json json)
        {
            jsonValues.Add(name, json);
        }

        public void AddValue(string name, IEnumerable<Json> jsons)
        {
            if (jsonArrayValues.ContainsKey(name))
            {
                throw new ArgumentException("value exist |" + name);
            }

            jsonArrayValues.Add(name, jsons);
        }



        public String Build()
        {
            StringBuilder json = new StringBuilder("{");

            foreach (var pair in stringValues)
            {
                json.Append(string.Format("\"{0}\":\"{1}\",", pair.Key, pair.Value));
            }

            foreach (var pair in jsonValues)
            {
                json.Append(string.Format("\"{0}\":{1},", pair.Key, pair.Value));
            }

            foreach (var pair in jsonArrayValues)
            {
                StringBuilder subJson = new StringBuilder("[");

                foreach (var data in pair.Value)
                {
                    subJson.Append(data.Build());
                    subJson.Append(",");
                }
                subJson.Remove(subJson.Length - 1, 1);
                subJson.Append("]");

                json.Append(string.Format("\"{0}\":{1},", pair.Key, subJson.ToString()));
            }

            json.Remove(json.Length - 1, 1);
            json.Append("}");

            return json.ToString();
        }

        public override string ToString()
        {
            return this.Build();
        }

    }
}
