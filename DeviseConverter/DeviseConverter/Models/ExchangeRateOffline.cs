using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
namespace DeviseConverter.Models
{
    public class ExchangeRateOffline
    {
        public string Base { get; set; }

        public Dictionary<string, string> Rates { get; set; }

        public string Date { get; set; }


    }
}