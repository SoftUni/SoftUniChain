using Faucet.Web.ViewModels;
using System.Web.Mvc;

namespace Faucet.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(TransactionViewModel transaction)
        {
            /*   TODO:
             *   validate
             *   sign transaction
             *   
             *   -Add PK in config (it should be stored in the genesis block)
             *   -gen PK eliptic cirve 251 
             *   -gen address hesh sha 251 one way
             *   
             *   send send responce to the node - post new transaction
             *   
             *   */
            return View(transaction);
        }
    }
}