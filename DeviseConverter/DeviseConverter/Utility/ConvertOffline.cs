using DeviseConverter.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace DeviseConverter.Utility
{
    public static class ConvertOffline
    {
        public static string resultConvertOffline(DeviseConverter.Models.ConverterModel cm)
        {
            //On effectue l'opération pour la convertio en fonction du taux de change
            string request = string.Format("SELECT EXCHANGE_RATE FROM offline_mode " +
                "WHERE CURRENCY_FROM = '{0}' AND CURRENCY_TARGET = '{1}'", cm.From, cm.To);

            ExchangeRateOffline ero = new ExchangeRateOffline();
            DB database = new DB();
            MySqlCommand msc = new MySqlCommand(request);
            DataTable dt = database.ExecuteSql(msc);
            string rate  = DB.GetMapping(dt.Rows[0], "EXCHANGE_RATE", string.Empty);

            var regex = new Regex(Regex.Escape("."));
            var filteredAmount = regex.Replace(cm.amount, "", 1);

            var amountToConvert = int.Parse(filteredAmount, System.Globalization.NumberStyles.AllowDecimalPoint);
            string filtreredRate = regex.Replace(rate, ",", 1);
            //Nous disposons du taux de change et de la devise à convertir
            //On fait l'opération
            var amountConverted = amountToConvert * double.Parse(filtreredRate, System.Globalization.NumberStyles.AllowDecimalPoint);
            
            //retour du montant convertit
            return amountConverted.ToString();

        }


    }
}