using Demo.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Models
{
    public class ProcessDataResultForPdf
    {
        public string MimeType => "pdf";

        private List<string> data;

        public List<string> Data
        {
            get => data.IsNull() ? null : data;
            set => data = value.IsNull() ? null : EncodeData(value);
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