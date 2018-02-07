using Faucet.Web.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Mvc;
//using System.Net.Http;

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
             *   -gen PK elliptic curve 251 
             *   -gen address hesh sha 251 one way
             *   
             *   send response to the node - post new transaction
             *   */

            if (!ValidInputs(transaction))
            {
                transaction.MessageResponse = "Invalid transaction details. Please check your details and try again.";
                return View(transaction);
            }

            string nodeIpAddress = ConfigurationManager.AppSettings["nodeIpAddress"];
            string newTransactionPostUrl = ConfigurationManager.AppSettings["newTransactionPostUrl"]; 
            string privateKey = ConfigurationManager.AppSettings["privatekey"]; //todo: think how to secure faucet private key
            string publicKey = string.Empty; //todo: priveteKey apply eliptic curve 251
            string addressFrom = string.Empty; //todo: one way hash on public key with sha256
            string to = transaction.To;
            decimal value = transaction.Value;
            string senderSignature = string.Empty;//
            int nonce = 0;
            String timestamp = DateTime.UtcNow.ToString("o");
            
            WebResponse response = null;
            HttpStatusCode statusCode = HttpStatusCode.RequestTimeout;

            WebRequest request = WebRequest.Create(nodeIpAddress + newTransactionPostUrl);
            statusCode = HttpStatusCode.RequestTimeout;

            request.Method = "POST";
            request.Timeout = 3000;
            request.ContentType = "application/json; charset=utf-8";


            /* post data template
             * {
             * "from": "44fe0696beb6e24541cc0e8728276c9ec3af2675",
             * "to": "9a9f082f37270ff54c5ca4204a0e4da6951fe917",
             * "value": 25.00,
             * "senderPubKey": "2a1d79fb8743d0a4a8501e0028079bcf82a4f…eae1",
             * "senderSignature": ["e20c…a3c29d3370f79f", "cf92…0acd0c132ffe56"]
             * }
             */

            JObject body = JObject.FromObject(new
            {
                from = addressFrom,
                to = to,
                value = value,
                senderPublicKey = publicKey,
                senderSignature = senderSignature,
                nonce = nonce
            });

            Byte[] trans = Encoding.UTF8.GetBytes(body.ToString());
            
            Stream dataStream =request.GetRequestStream();
            dataStream = request.GetRequestStream();
            dataStream.Write(trans, 0, trans.Length);
            dataStream.Close();

            response = request.GetResponse();
            statusCode = ((HttpWebResponse)response).StatusCode;

            /* response data template
             * {
             * "dateReceived": "2018-02-01T23:17:02.744Z",
             * "transactionHash": "cd8d9a345bb208c6f9b8acd6b8eefe6…20c8a"
             *  }
             */
            transaction.MessageResponse = statusCode.ToString() + ": " +  ((HttpWebResponse)response).StatusDescription;
            transaction.TransactionHash = "pdasd3334t45fffdllozn44mja99qjjiuo2ygfytf4551ug44eqrtf2";// todo: display the transaction hash from the response

            return View(transaction);
        }

        private bool ValidInputs(TransactionViewModel transaction)
        {
            //todo: add validation for is address correct etc.
            if (string.IsNullOrEmpty(transaction.To))
            {
                return false;
            }
            return true;
        }
    }
}