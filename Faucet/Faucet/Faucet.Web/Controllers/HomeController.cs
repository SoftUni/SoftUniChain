using Org.BouncyCastle.Security;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Crypto.Digests;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Crypto.Signers;
using System.Web.Mvc;
using Faucet.Web.ViewModels;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Crypto.Parameters;
using System.Text;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Asn1.Sec;
using System.Linq;
using System;
using System.Net;
using System.IO;

namespace Faucet.Web.Controllers
{
    public class HomeController : Controller
    {
        static readonly X9ECParameters curve = SecNamedCurves.GetByName("secp256k1");

        public ActionResult Index()
        {
            return View();
        }

        /***********  approach 1 using Bouncy Castle    *************/
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
            string privateKeyStr = "7e4670ae70c98d24f3662c172dc510a085578b9ccc717e6c2f4e547edd960a34";

            string signedTransObject = CreateAndSignTransaction(transaction.To, 3, "2018-02-10T17:53:48.972Z", privateKeyStr);
            ///* post data template
            // * {
            // * "from": "44fe0696beb6e24541cc0e8728276c9ec3af2675",
            // * "to": "9a9f082f37270ff54c5ca4204a0e4da6951fe917",
            // * "value": 25.00,
            // * "senderPubKey": "2a1d79fb8743d0a4a8501e0028079bcf82a4f…eae1",
            // * "senderSignature": ["e20c…a3c29d3370f79f", "cf92…0acd0c132ffe56"]
            // * }
            // */

            try
            {
                WebResponse response = null;
                HttpStatusCode statusCode = HttpStatusCode.RequestTimeout;

                WebRequest request = WebRequest.Create(nodeIpAddress + newTransactionPostUrl);
                statusCode = HttpStatusCode.RequestTimeout;

                request.Method = "POST";
                request.Timeout = 3000;
                request.ContentType = "application/json; charset=utf-8";

                Byte[] trans = Encoding.UTF8.GetBytes(signedTransObject);

                Stream dataStream = request.GetRequestStream();
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

                transaction.MessageResponse = statusCode.ToString() + ": " + ((HttpWebResponse)response).StatusDescription;
                transaction.TransactionHash = "TransactionHash: pdasd3334t45fffdllozn44mja99qjjiuo2ygfytf4551ug44eqrtf2";// todo: display the transaction hash from the response
            }
            catch (Exception ex)
            {
                transaction.MessageResponse = ex.Message;
            }

            return View(transaction);

        }

        private string CreateAndSignTransaction(string recipientAddress, int value,
            string iso8601datetime, string senderPrivKeyHex)
        {
            BigInteger privateKey = new BigInteger(senderPrivKeyHex, 16);

            ECPoint pubKey = GetPublicKeyFromPrivateKey(privateKey);
            string senderPubKeyCompressed = EncodeECPointHexCompressed(pubKey);

            string senderAddress = CalcRipeMD160(senderPubKeyCompressed);

            var tran = new
            {
                from = senderAddress,
                to = recipientAddress,
                value = value,
                dateCreated = iso8601datetime,
                senderPubKey = senderPubKeyCompressed,
            };
            string tranJson = JsonConvert.SerializeObject(tran);

            byte[] tranHash = CalcSHA256(tranJson);

            BigInteger[] tranSignature = SignData(privateKey, tranHash);

            var tranSigned = new
            {
                from = senderAddress,
                to = recipientAddress,
                value = value,
                dateCreated = iso8601datetime,
                senderPubKey = senderPubKeyCompressed,
                senderSignature = new string[]
                {
                tranSignature[0].ToString(16),
                tranSignature[1].ToString(16)
                }
            };

            return JsonConvert.SerializeObject(tranSigned, Formatting.Indented);
        }

        public static ECPoint GetPublicKeyFromPrivateKey(BigInteger privKey)
        {
            ECPoint pubKey = curve.G.Multiply(privKey).Normalize();
            return pubKey;
        }

        public static string EncodeECPointHexCompressed(ECPoint point)
        {
            BigInteger x = point.XCoord.ToBigInteger();
            return x.ToString(16) + Convert.ToInt32(!x.TestBit(0));
        }

        private static string CalcRipeMD160(string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            RipeMD160Digest digest = new RipeMD160Digest();
            digest.BlockUpdate(bytes, 0, bytes.Length);
            byte[] result = new byte[digest.GetDigestSize()];
            digest.DoFinal(result, 0);
            return BytesToHex(result);
        }

        public static string BytesToHex(byte[] bytes)
        {
            return string.Concat(bytes.Select(b => b.ToString("x2")));
        }

        /// <summary>
        /// Calculates deterministic ECDSA signature (with HMAC-SHA256), based on secp256k1 and RFC-6979.
        /// </summary>
        private static BigInteger[] SignData(BigInteger privateKey, byte[] data)
        {
            ECDomainParameters ecSpec = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H);
            ECPrivateKeyParameters keyParameters = new ECPrivateKeyParameters(privateKey, ecSpec);
            IDsaKCalculator kCalculator = new HMacDsaKCalculator(new Sha256Digest());
            ECDsaSigner signer = new ECDsaSigner(kCalculator);
            signer.Init(true, keyParameters);
            BigInteger[] signature = signer.GenerateSignature(data);
            return signature;
        }

        private static byte[] CalcSHA256(string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            Sha256Digest digest = new Sha256Digest();
            digest.BlockUpdate(bytes, 0, bytes.Length);
            byte[] result = new byte[digest.GetDigestSize()];
            digest.DoFinal(result, 0);
            return result;
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

        /*              *********** approach 2 USING Secp256k1 Library ***********
         * 
         * 
         * 
        //[HttpPost]
        //public ActionResult Index(TransactionViewModel transaction)
        //{
        //    if (!ValidInputs(transaction))
        //    {
        //        transaction.MessageResponse = "Invalid transaction details. Please check your details and try again.";
        //        return View(transaction);
        //    }

        //    string nodeIpAddress = "127.0.0.1:5000";
        //    string newTransactionPostUrl = "/transactions/new";
        //    string privateKeyStr = "cUgAvxseC9hpHXMuZkW1yMkTdijEcDTeLeMgmhaTd1eGnbMLdPpz";

        //    //source: https://github.com/TangibleCryptography/Secp256k1

        //    string privKeyHex = SHA1HashStringForUTF8String(privateKeyStr);

        //    BigInteger privateKey = Hex.HexToBigInteger(privKeyHex);
        //    Secp256k1.ECPoint publicKey = Secp256k1.Secp256k1.G.Multiply(privateKey);

        //    string bitcoinAddressCompressed = publicKey.GetBitcoinAddress(compressed: true);
        //    uint value = 268435263;

        //    var varInt = new VarInt(value); 
        //    value = value ^ 2;

        //    byte[] varIntBytes = VarInt.Encode(value);

        //    // encryption
        //    ECEncryption encryption = new ECEncryption();
        //    const string message = "This is my encrypted message";
        //    byte[] encrypted = encryption.Encrypt(publicKey, message);
        //    byte[] decrypted = encryption.Decrypt(privateKey, encrypted);
        //    string decryptedMessage = Encoding.UTF8.GetString(decrypted);

        //    // signing
        //    MessageSignerVerifier messageSigner = new MessageSignerVerifier();
        //    SignedMessage signedMessage = messageSigner.Sign(privateKey, "Test Message to sign, you can verify this on http://brainwallet.org/#verify");
        //    bool verified = messageSigner.Verify(signedMessage);

        //    post data template
        //     * {
        //     * "from": "44fe0696beb6e24541cc0e8728276c9ec3af2675",
        //     * "to": "9a9f082f37270ff54c5ca4204a0e4da6951fe917",
        //     * "value": 25.00,
        //     * "senderPubKey": "2a1d79fb8743d0a4a8501e0028079bcf82a4f…eae1",
        //     * "senderSignature": ["e20c…a3c29d3370f79f", "cf92…0acd0c132ffe56"]
        //     * }
        //     

        //    WebResponse response = null;
        //    HttpStatusCode statusCode = HttpStatusCode.RequestTimeout;

        //    WebRequest request = WebRequest.Create(nodeIpAddress + newTransactionPostUrl);
        //    statusCode = HttpStatusCode.RequestTimeout;

        //    request.Method = "POST";
        //    request.Timeout = 3000;
        //    request.ContentType = "application/json; charset=utf-8";

        //    JObject body = JObject.FromObject(new
        //    {
        //        from = bitcoinAddressCompressed,  
        //        to = transaction.To,
        //        value = transaction.Value,
        //        senderPublicKey = publicKey,
        //        senderSignature = signedMessage.Signature,
        //        nonce = 0
        //    });

        //    Byte[] trans = Encoding.UTF8.GetBytes(body.ToString());

        //    Stream dataStream =request.GetRequestStream();
        //    dataStream = request.GetRequestStream();
        //    dataStream.Write(trans, 0, trans.Length);
        //    dataStream.Close();

        //    response = request.GetResponse();
        //    statusCode = ((HttpWebResponse)response).StatusCode;

        //     response data template
        //     * {
        //     * "dateReceived": "2018-02-01T23:17:02.744Z",
        //     * "transactionHash": "cd8d9a345bb208c6f9b8acd6b8eefe6…20c8a"
        //     *  }
        //     

        //    transaction.MessageResponse = statusCode.ToString() + ": " +  ((HttpWebResponse)response).StatusDescription;
        //    transaction.TransactionHash = "pdasd3334t45fffdllozn44mja99qjjiuo2ygfytf4551ug44eqrtf2";// todo: display the transaction hash from the response

        //    return View(transaction);
        //}

        ///// <summary>
        ///// Compute hash for string encoded as UTF8
        ///// </summary>
        ///// <param name="s">String to be hashed</param>
        ///// <returns>40-character hex string</returns>
        //public string SHA1HashStringForUTF8String(string s)
        //{
        //    byte[] bytes = Encoding.UTF8.GetBytes(s);

        //    var sha1 = SHA1.Create();
        //    byte[] hashBytes = sha1.ComputeHash(bytes);

        //    return HexStringFromBytes(hashBytes);
        //}

        ///// <summary>
        ///// Convert an array of bytes to a string of hex digits
        ///// </summary>
        ///// <param name="bytes">array of bytes</param>
        ///// <returns>String of hex digits</returns>
        //public static string HexStringFromBytes(byte[] bytes)
        //{
        //    var sb = new StringBuilder();
        //    foreach (byte b in bytes)
        //    {
        //        var hex = b.ToString("x2");
        //        sb.Append(hex);
        //    }
        //    return sb.ToString();
        //}
        */
    }
}