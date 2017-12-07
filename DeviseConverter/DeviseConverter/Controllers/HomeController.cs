using DeviseConverter.Helper;
using DeviseConverter.Models;
using DeviseConverter.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DeviseConverter.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            //Récupération de la liste des devises dispo en base
            List<CurrencyModel> currenciesList = QueryDBHelper.getConverterModelFromDB();
            ViewData["CurrenciesList"] = currenciesList;

            //On vérifie si la table est vide, si oui on la remplit pour l'utilisation du mode offline

            if(QueryDBHelper.isExchangeEmpty())
            {
                //Pour l'utilisation du mode offline on ajoute en base les taux de changes des 7 monnaies les plus utilisées
                APIQuery.storeAllCurrenciesPobisiblitiesInDB();
            }
            return View();
        }

        public ActionResult IsOffline(bool offline)
        {
            if(!offline)
            {
                return RedirectToAction(Url.Action("Index"));
            }
            else
            {
                ViewData["CurrenciesList"] = QueryDBHelper.GetMostUsedForOfflineMode();
                return View();
            }
        }
        

        /// <summary>
        /// Conversion d'une devise
        /// </summary>
        /// <param name="cm"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ConvertShit(ConverterModel cm)
        {
            ConverterModel cm2 = new ConverterModel();
            if (cm.offline)
            {
                cm2.amount = Utility.ConvertOffline.resultConvertOffline(cm);
            }
            else
            {
                cm2 = Utility.APIQuery.GetCurrency(cm);
            }
            return Json(cm2.amount);
        }


        

        
    }
}