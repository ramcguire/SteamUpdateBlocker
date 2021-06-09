using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;

namespace Steamfiles {
    public static class Steamfile {
        // Rewrites ACF file to the manifest path
        // Used after modifying the internal Manifest OrderedDictionary
        public static void SaveACF(string path, OrderedDictionary acf) {
            using (StreamWriter file = new StreamWriter(path)) {
                file.Write(acf.ACFString());
            }
        }

        // Extension method on OrderedDictionary so we can directly call SaveACF on it
        public static void SaveACF(this OrderedDictionary acf, string path) {
            using (StreamWriter file = new StreamWriter(path)) {
                file.Write(acf.ACFString());
            }

        }
        // Creates a string representation of the deserialized ACF file
        // Used for writing back to a file
        private static string ACFString(this OrderedDictionary file) => string.Join("\n", SerializeDictionary(file, 0)) + "\n";


        // Serializes Manifest OrderedDictionary into a List<string> with proper formatting to write back out to an ACF file
        private static List<string> SerializeDictionary(OrderedDictionary data, int level) {
            List<string> lines = new List<string>();
            string indent = new string('\t', level);

            foreach (var key in data.Keys) {
                if (data[key].GetType() == typeof(OrderedDictionary)) {
                    string line = indent + "\"" + key + "\"\n" + indent + "{";
                    lines.Add(line);
                    lines.AddRange(SerializeDictionary((OrderedDictionary)data[key], level + 1));

                    lines.Add(indent + "}");
                } else {
                    lines.Add(indent + '"' + key + '"' + "\t\t" + '"' + data[key] + '"');
                }
            }
            return lines;
        }

        public static OrderedDictionary LoadACF(string path) {
            OrderedDictionary parsed = new OrderedDictionary();
            OrderedDictionary currentSection = parsed;
            List<string> sections = new List<string>();

            List<string> lines = new List<string>();

            string line;

            using (StreamReader file = new StreamReader(path)) {
                while ((line = file.ReadLine()) != null) {
                    lines.Add(line.Trim());
                }
            }

            // iterate all lines in the file
            foreach (string l in lines) {
                string key;
                string value;
                try {
                    string[] spl = l.Split('\t');
                    key = spl[0].Trim('"');
                    value = spl[2].Trim('"');
                }
                // IndexOutOfRangeException indicates we are on an open or close bracket
                catch (IndexOutOfRangeException) {
                    if (l == "{") {
                        // create a new section
                        currentSection = PrepareSubsection(ref parsed, ref sections);
                    } else if (l == "}") {
                        sections.RemoveAt(sections.Count() - 1);
                    } else {
                        sections.Add(l.Trim('"'));
                    }
                    continue;
                }
                currentSection[key] = value;
            }
            return parsed;
        }

        // Extension method of original LoadACF
        public static OrderedDictionary LoadACF(this OrderedDictionary parsed, string path) {
            OrderedDictionary currentSection = parsed;
            List<string> sections = new List<string>();
            List<string> lines = new List<string>();

            string line;

            using (StreamReader file = new StreamReader(path)) {
                while ((line = file.ReadLine()) != null) {
                    lines.Add(line.Trim());
                }
            }
            // iterate all lines in the file
            foreach (string l in lines) {
                string key;
                string value;
                try {
                    string[] spl = l.Split('\t');
                    key = spl[0].Trim('"');
                    value = spl[2].Trim('"');
                }
                // IndexOutOfRangeException indicates we are on an open or close bracket
                catch (IndexOutOfRangeException) {
                    if (l == "{") {
                        // create a new section
                        currentSection = PrepareSubsection(ref parsed, ref sections);
                    } else if (l == "}") {
                        sections.RemoveAt(sections.Count() - 1);
                    } else {
                        sections.Add(l.Trim('"'));
                    }
                    continue;
                }
                currentSection[key] = value;
            }
            return parsed;
        }

        private static OrderedDictionary PrepareSubsection(ref OrderedDictionary data, ref List<string> sections) {
            OrderedDictionary current = data;
            foreach (string i in sections.Take(sections.Count - 1)) {
                current = (OrderedDictionary)current[i];
            }
            current[sections.Last()] = new OrderedDictionary();
            return (OrderedDictionary)current[sections.Last()];
        }
    }
}
