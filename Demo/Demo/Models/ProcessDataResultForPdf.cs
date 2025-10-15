using Demo.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Models
{
    public class ProcessDataResultForPdf
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
                var item = sourceText.Replace("\"", "'");
                transformedData.Add(item);
            }
            return transformedData;
        }
    }
}