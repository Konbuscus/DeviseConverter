using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeviseConverter.Models;
using DeviseConverter.Utility;
using MySql.Data.MySqlClient;
using System.Data;

namespace DeviseConverter.Helper
{
    public class QueryDBHelper
    {

        /// <summary>
        /// Obtient la liste des devises disponibles en base
        /// </summary>
        /// <returns></returns>
        public static List<CurrencyModel> getConverterModelFromDB()
        {
            string request = $"SELECT * FROM osef.currencies";
            DB database = new DB();
            MySqlCommand command = new MySqlCommand(request);
            DataTable listCurrencies = database.ExecuteSql(command);
            List<CurrencyModel> backingList = new List<CurrencyModel>();

            if (listCurrencies != null)
            {
                foreach (DataRow row in listCurrencies.Rows)
                {
                    CurrencyModel cm = new CurrencyModel();
                    cm.Name = DB.GetMapping(row, "idCURRENCIES", string.Empty);
                    cm.Description = DB.GetMapping(row, "Description", string.Empty);
                    backingList.Add(cm);
                }
                return backingList;
            }
            return backingList;
        }

        /// <summary>
        /// Récupère les 7 devises les plus utilisés pour le mode offline
        /// </summary>
        /// <returns></returns>
        public static List<CurrencyModel> GetMostUsedForOfflineMode()
        {
            string request = $"SELECT * FROM osef.currencies where idCURRENCIES = 'FRA' OR " +
                $"idCURRENCIES = 'GBP' OR idCURRENCIES = 'JPY' OR idCURRENCIES = 'USD' OR idCURRENCIES= 'AUD' OR " +
                $"idCURRENCIES = 'CNY' OR idCURRENCIES = 'CAD' ";

            DB database = new DB();
            MySqlCommand msc = new MySqlCommand(request);
            DataTable dt = database.ExecuteSql(msc);
            List<CurrencyModel> cmList = new List<CurrencyModel>();

            foreach(DataRow row in dt.Rows)
            {
                CurrencyModel cm = new CurrencyModel();
                cm.Name = DB.GetMapping(row, "idCURRENCIES", string.Empty);
                cm.Description = DB.GetMapping(row, "Description", string.Empty);
                cmList.Add(cm);
            }
            return cmList;
        }

        /// <summary>
        /// Insertion des taux d'échanges pour les 7 devises les plus utilisées
        /// </summary>
        /// <param name="ero"></param>
        /// <returns></returns>
        public static bool InsertRatesInDB(List<ExchangeRateOffline> ero)
        {
            DB database = new DB();
            string OfflineMode = "OFFLINE_MODE";
            string finalRequest = null;

            foreach(var item in ero)
            {
                foreach (var rate in item.Rates)
                {
                    string request = string.Format("INSERT INTO " + OfflineMode + " (CURRENCY_FROM, CURRENCY_TARGET, EXCHANGE_RATE) " +
                   "VALUES ('{0}', '{1}', '{2}');", item.Base, rate.Key, rate.Value);
                    finalRequest += request;
                }
            }
            MySqlCommand command = new MySqlCommand(finalRequest);

            try
            {
                database.ExecuteScalar(command);
            }
            catch(Exception e)
            {
                e.GetBaseException();
            }
            
            return true;
        }

        /// <summary>
        /// Vérifie que la table des taux d"échanges est vide true, si oui
        /// </summary>
        /// <returns></returns>
        public static bool isExchangeEmpty()
        {
            string request = "select * from offline_mode";
            DB database = new DB();
            MySqlCommand mysql = new MySqlCommand(request);
            DataTable dt = database.ExecuteSql(mysql);

            if(dt.Rows.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }


    }
}