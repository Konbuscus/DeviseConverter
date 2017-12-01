using DeviseConverter.Helper;
using DeviseConverter.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;

namespace DeviseConverter.Utility
{
    public static class APIQuery
    {
        public const string API_URL = "https://www.amdoren.com/api/currency.php?api_key=";
        public const string API_RATE = "https://api.fixer.io/latest";
        public const string API_KEY = "DEPuUXpmi2PSd7hS8nun2ne8gsPyR9";

        /// <summary>
        /// Appel de l'API d'un webservice afin de convertir un montant d'une devise à une autre
        /// </summary>
        /// <param name="cm"></param>
        /// <returns></returns>
        public static ConverterModel GetCurrency(ConverterModel cm)
        {
            //Appel de l'API
            var client = new WebClient();
            string query = API_URL + API_KEY + "&from="+cm.From+"&to="+cm.To+"&amount="+cm.amount ;
            //On force le point à la place de la première virgule dans l'url
            //var regex = new Regex(Regex.Escape(","));
            //var filteredQuery = regex.Replace(query, ".", 1);

            //Reponse de l'api
            string response = client.DownloadString(query);

            var cm2 = new ConverterModel();
            var obj = JObject.Parse(response);
            cm2.amount = (string) obj["amount"];
            cm2.From = cm.From;
            cm2.To = cm.To;
            //Retour du model à envoyer à la vue
            return cm2;
        }

        /// <summary>
        /// Récupère et stock toutes les valeurs possibles d'échanges
        /// </summary>
        /// <returns></returns>
        public static bool storeAllCurrenciesPobisiblitiesInDB()
        {
            //Création du client pour les opérations de GET
            WebClient wb = new WebClient();
            List<ExchangeRateOffline> offlineRateList = new List<ExchangeRateOffline>();

            string responseForEUR = wb.DownloadString(API_RATE);
            string responseForUSD = wb.DownloadString(API_RATE + "?base=USD");
            string responseForGBP = wb.DownloadString(API_RATE + "?base=GBP");
            string responseForJPY = wb.DownloadString(API_RATE + "?base=JPY");
            string responseForCAD = wb.DownloadString(API_RATE + "?base=CAD");
            string responseForAUD = wb.DownloadString(API_RATE + "?base=AUD");
            string responseFORCNY = wb.DownloadString(API_RATE + "?base=CNY");


            List<string> allUrlsToGet = new List<string>(){responseForEUR, responseFORCNY, responseForCAD, responseForJPY,
            responseForUSD, responseForGBP, responseForAUD};
            
            foreach( var url in allUrlsToGet)
            {
                ExchangeRateOffline rate = JsonConvert.DeserializeObject<ExchangeRateOffline>(url);
                offlineRateList.Add(rate);
            }

            //Insertion en base de donnés
            bool isInserted = QueryDBHelper.InsertRatesInDB(offlineRateList);

            return isInserted;

        }

        



        




    }
}