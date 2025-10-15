using Demo.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Models
{
    public class ProcessDataResultForXml
    {
        private List<string> _data;

        public List<string> Data
        {
            get => _data.IsNull() ? null : _data;
            set => _data = value.IsNull() ? null : EncodeData(value);
        }

        public string Code { get; set; }

        private static List<string> EncodeData(List<string> data)
        {
            var transformedData = new List<string>();
            foreach (var sourceText in data)
            {
                transformedData.Add(ConvertStringToBase64(sourceText));
            }
            return transformedData;
        }

        private static string ConvertStringToBase64(string input)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(byteArray);
        }
    }
}