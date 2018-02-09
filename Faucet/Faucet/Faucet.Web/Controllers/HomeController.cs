using Faucet.Web.ViewModels;
using Newtonsoft.Json.Linq;
using Secp256k1;
using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
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
            if (!ValidInputs(transaction))
            {
                transaction.MessageResponse = "Invalid transaction details. Please check your details and try again.";
                return View(transaction);
            }

            string nodeIpAddress = "127.0.0.1:5000";
            string newTransactionPostUrl = "/transactions/new";
            string privateKeyStr = "cUgAvxseC9hpHXMuZkW1yMkTdijEcDTeLeMgmhaTd1eGnbMLdPpz";

            //source: https://github.com/TangibleCryptography/Secp256k1

            string privKeyHex = SHA1HashStringForUTF8String(privateKeyStr);

            BigInteger privateKey = Hex.HexToBigInteger(privKeyHex);
            Secp256k1.ECPoint publicKey = Secp256k1.Secp256k1.G.Multiply(privateKey);

            string bitcoinAddressCompressed = publicKey.GetBitcoinAddress(compressed: true);
            uint value = 268435263;

            var varInt = new VarInt(value); 
            value = value ^ 2;

            byte[] varIntBytes = VarInt.Encode(value);

            // encryption
            ECEncryption encryption = new ECEncryption();
            const string message = "This is my encrypted message";
            byte[] encrypted = encryption.Encrypt(publicKey, message);
            byte[] decrypted = encryption.Decrypt(privateKey, encrypted);
            string decryptedMessage = Encoding.UTF8.GetString(decrypted);

            // signing
            MessageSignerVerifier messageSigner = new MessageSignerVerifier();
            SignedMessage signedMessage = messageSigner.Sign(privateKey, "Test Message to sign, you can verify this on http://brainwallet.org/#verify");
            bool verified = messageSigner.Verify(signedMessage);

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
                from = bitcoinAddressCompressed,  
                to = transaction.To,
                value = transaction.Value,
                senderPublicKey = publicKey,
                senderSignature = signedMessage.Signature,
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

        /// <summary>
        /// Compute hash for string encoded as UTF8
        /// </summary>
        /// <param name="s">String to be hashed</param>
        /// <returns>40-character hex string</returns>
        public string SHA1HashStringForUTF8String(string s)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(s);

            var sha1 = SHA1.Create();
            byte[] hashBytes = sha1.ComputeHash(bytes);

            return HexStringFromBytes(hashBytes);
        }

        /// <summary>
        /// Convert an array of bytes to a string of hex digits
        /// </summary>
        /// <param name="bytes">array of bytes</param>
        /// <returns>String of hex digits</returns>
        public static string HexStringFromBytes(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }
            return sb.ToString();
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