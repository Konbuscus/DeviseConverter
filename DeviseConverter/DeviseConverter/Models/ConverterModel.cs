using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeviseConverter.Models
{
    public class ConverterModel
    {
        public string amount { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string offline { get; set; }
    }
}