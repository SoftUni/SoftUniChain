using Faucet.Web.ViewModels;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Security;
using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net;
using System.Numerics;
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

            //wif = it`s your privat sign key
          


            string nodeIpAddress = "127.0.0.1:5000";
            string newTransactionPostUrl = "/transactions/new";
            string privateKey = "cUgAvxseC9hpHXMuZkW1yMkTdijEcDTeLeMgmhaTd1eGnbMLdPpz";
            string publicKey = "13a36ccccff928be2ee380978b60a4a012cdc2934d8b90fa9b4721ba857751lk";

            //1. get public key with Eliptic Curve
            byte[] PubKey = ToPublicKey(Encoding.UTF8.GetBytes(privateKey));
            byte[] PubKeySha = Sha256(PubKey);
            byte[] PubKeyShaRIPE = RipeMD160(PubKeySha);
            byte[] PreHashWNetwork = AppendBitcoinNetwork(PubKeyShaRIPE, 0);
            byte[] PublicHash = Sha256(PreHashWNetwork);
            byte[] PublicHashHash = Sha256(PublicHash);

            byte[] Address = ConcatAddress(PreHashWNetwork, PublicHashHash);
            string addressFrom = Base58Encode(Address);// human readable / todo hex

            string signature = Sign("", "", "");

            //2. Sign

            //2. 

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

        private static X9ECParameters curve = SecNamedCurves.GetByName("secp256k1");
        private static ECDomainParameters domain = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H);

        public static byte[] ToPublicKey(byte[] privateKey)
        {
            Org.BouncyCastle.Math.BigInteger d = new Org.BouncyCastle.Math.BigInteger(privateKey);
            Org.BouncyCastle.Math.EC.ECPoint q = domain.G.Multiply(d);

            var publicParams = new ECPublicKeyParameters(q, domain);
            return publicParams.Q.GetEncoded();
        }

        //RsaSha1Signing
        private RsaKeyParameters MakeKey(String modulusHexString, String exponentHexString, bool isPrivateKey)
        {
            var modulus = new Org.BouncyCastle.Math.BigInteger(modulusHexString, 16);
            var exponent = new Org.BouncyCastle.Math.BigInteger(exponentHexString, 16);

            return new RsaKeyParameters(isPrivateKey, modulus, exponent);
        }

        public String Sign(String data, String privateModulusHexString, String privateExponentHexString)
        {
            /* Make the key */
            RsaKeyParameters key = MakeKey(privateModulusHexString, privateExponentHexString, true);

            /* Init alg */
            ISigner sig = SignerUtilities.GetSigner("SHA1withRSA");

            /* Populate key */
            sig.Init(true, key);

            /* Get the bytes to be signed from the string */
            var bytes = Encoding.UTF8.GetBytes(data);

            /* Calc the signature */
            sig.BlockUpdate(bytes, 0, bytes.Length);
            byte[] signature = sig.GenerateSignature();

            /* Base 64 encode the sig so its 8-bit clean */
            var signedString = Convert.ToBase64String(signature);

            return signedString;
        }

        public static byte[] HexToByte(string HexString)
        {
            if (HexString.Length % 2 != 0)
            {
                throw new Exception("Invalid HEX");
            }

            byte[] retArray = new byte[HexString.Length / 2];

            for (int i = 0; i < retArray.Length; ++i)
            {
                retArray[i] = byte.Parse(HexString.Substring(i * 2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }

            return retArray;
        }

        public static byte[] Sha256(byte[] array)
        {
            SHA256Managed hashstring = new SHA256Managed();
            return hashstring.ComputeHash(array);
        }

        public static byte[] RipeMD160(byte[] array)
        {
            RIPEMD160Managed hashstring = new RIPEMD160Managed();
            return hashstring.ComputeHash(array);
        }

        public static byte[] AppendBitcoinNetwork(byte[] RipeHash, byte Network)
        {
            byte[] extended = new byte[RipeHash.Length + 1];
            extended[0] = (byte)Network;
            Array.Copy(RipeHash, 0, extended, 1, RipeHash.Length);
            return extended;
        }

        public static byte[] ConcatAddress(byte[] RipeHash, byte[] Checksum)
        {
            byte[] ret = new byte[RipeHash.Length + 4];
            Array.Copy(RipeHash, ret, RipeHash.Length);
            Array.Copy(Checksum, 0, ret, RipeHash.Length, 4);
            return ret;
        }

        private static string ByteToHex(byte[] pubKeySha)
        {
            byte[] data = pubKeySha;
            string hex = BitConverter.ToString(data);
            return hex;
        }

        public static string Base58Encode(byte[] array)
        {
            const string ALPHABET = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
            string retString = string.Empty;
            System.Numerics.BigInteger encodeSize = ALPHABET.Length;
            System.Numerics.BigInteger arrayToInt = 0;
            for (int i = 0; i < array.Length; ++i)
            {
                arrayToInt = arrayToInt * 256 + array[i];
            }
            while (arrayToInt > 0)
            {
                int rem = (int)(arrayToInt % encodeSize);
                arrayToInt /= encodeSize;
                retString = ALPHABET[rem] + retString;
            }
            for (int i = 0; i < array.Length && array[i] == 0; ++i)
                retString = ALPHABET[0] + retString;

            return retString;
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