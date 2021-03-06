﻿using Json.Net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;

namespace AccessPong.Data.Logic
{
    public class Names
    {
        public List<string> ConvertNames()
        {
            try
            {
                List<string> names = new List<string>();

                string path = Directory.GetCurrentDirectory();

                // Extract file path to appsettings
                string filePath = Path.GetFullPath(Path.Combine(path, @"C:\Users\Max.Goddard\Desktop\AccessPong-BE\AccessPong.Data\Data\names.json"));
                Console.WriteLine(filePath);

                using (StreamReader r = new StreamReader(filePath))
                {
                    string json = r.ReadToEnd();
                    var arr = JArray.Parse(json);

                    foreach (var name in arr)
                    {
                        names.Add(name.ToString());
                    }
                }

                return names;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
