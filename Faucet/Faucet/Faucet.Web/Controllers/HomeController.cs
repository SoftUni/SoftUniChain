using Faucet.Web.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
//using Cryptography.ECDSA;
//using System.Security.Cryptography;
//using Org.BouncyCastle.Math.EC;
//using Org.BouncyCastle.Crypto.EC;
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
             *   -gen address hesh sha 251 one way in order to sign the transaction
             *   
             *   send response to the node - post new transaction
             *   */

            if (!ValidInputs(transaction))
            {
                transaction.MessageResponse = "Invalid transaction details. Please check your details and try again.";
                return View(transaction);
            }

            //todo: add settings in config file
            //string nodeIpAddress = ConfigurationManager.AppSettings["nodeIpAddress"];
            //string newTransactionPostUrl = ConfigurationManager.AppSettings["newTransactionPostUrl"]; 
            //string privateKey = ConfigurationManager.AppSettings["privatekey"]; //todo: think how to secure faucet private key


            string nodeIpAddress = "127.0.0.1:5000";
            string newTransactionPostUrl = "/transactions/new";
            string privateKey = "13a36ccccff928be2ee380978b60a4a012cdc2934d8b90fa9b4721ba857751lk";


            string addressFrom = string.Empty; //todo: one way hash on public key with sha256

            //source: https://blogs.msdn.microsoft.com/shawnfa/2007/01/18/elliptic-curve-dsa/
            ECDsaCng dsa = new ECDsaCng();

            byte[] pkBytes = Encoding.UTF8.GetBytes(privateKey);
            byte[] pubKeyData = dsa.SignHash(pkBytes);
            string publicKey = Encoding.UTF8.GetString(pubKeyData);


            dsa.HashAlgorithm = CngAlgorithm.Sha256;
            byte[] signature = dsa.SignData(pubKeyData);

            /* post data template
             * {
             * "from": "44fe0696beb6e24541cc0e8728276c9ec3af2675",
             * "to": "9a9f082f37270ff54c5ca4204a0e4da6951fe917",
             * "value": 25.00,
             * "senderPubKey": "2a1d79fb8743d0a4a8501e0028079bcf82a4f…eae1",
             * "senderSignature": ["e20c…a3c29d3370f79f", "cf92…0acd0c132ffe56"]
             * }
             */

            WebResponse response = null;
            HttpStatusCode statusCode = HttpStatusCode.RequestTimeout;

            WebRequest request = WebRequest.Create(nodeIpAddress + newTransactionPostUrl);
            statusCode = HttpStatusCode.RequestTimeout;

            request.Method = "POST";
            request.Timeout = 3000;
            request.ContentType = "application/json; charset=utf-8";

            JObject body = JObject.FromObject(new
            {
                from = addressFrom,
                to = transaction.To,
                value = transaction.Value,
                senderPublicKey = publicKey,
                senderSignature = signature,
                nonce = 0
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

            //todo: validate balance from node 

            return true;
        }
    }
}