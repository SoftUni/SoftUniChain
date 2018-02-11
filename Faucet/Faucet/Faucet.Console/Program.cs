

namespace Faucet.Console
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System;
    using System.Text;
    using System.Linq;
    using Org.BouncyCastle.Asn1.X9;
    using Org.BouncyCastle.Asn1.Sec;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Generators;
    using Org.BouncyCastle.Security;
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Crypto.Digests;
    using Newtonsoft.Json;
    using Org.BouncyCastle.Asn1.Ocsp;
    using Org.BouncyCastle.Crypto.Signers;
    class Program
    {
        static readonly X9ECParameters curve = SecNamedCurves.GetByName("secp256k1");

        public static ECPoint GetPublicKeyFromPrivateKey(BigInteger privKey)
        {
            ECPoint pubKey = curve.G.Multiply(privKey).Normalize();
            return pubKey;
        }

        public static AsymmetricCipherKeyPair GenerateRandomKeys(int keySize = 256)
        {
            ECKeyPairGenerator gen = new ECKeyPairGenerator();
            SecureRandom secureRandom = new SecureRandom();
            KeyGenerationParameters keyGenParam =
                new KeyGenerationParameters(secureRandom, keySize);
            gen.Init(keyGenParam);
            return gen.GenerateKeyPair();
        }

        public static string BytesToHex(byte[] bytes)
        {
            return string.Concat(bytes.Select(b => b.ToString("x2")));
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

        private static byte[] CalcSHA256(string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            Sha256Digest digest = new Sha256Digest();
            digest.BlockUpdate(bytes, 0, bytes.Length);
            byte[] result = new byte[digest.GetDigestSize()];
            digest.DoFinal(result, 0);
            return result;
        }

        private static void RandomPrivateKeyToAddress()
        {
            Console.WriteLine("Random private key --> public key --> address");
            Console.WriteLine("---------------------------------------------");

            var keyPair = GenerateRandomKeys();

            BigInteger privateKey = ((ECPrivateKeyParameters)keyPair.Private).D;
            Console.WriteLine("Private key (hex): " + privateKey.ToString(16));
            Console.WriteLine("Private key: " + privateKey.ToString(10));

            ECPoint pubKey = ((ECPublicKeyParameters)keyPair.Public).Q;
            Console.WriteLine("Public key: ({0}, {1})",
                pubKey.XCoord.ToBigInteger().ToString(10),
                pubKey.YCoord.ToBigInteger().ToString(10));

            string pubKeyCompressed = EncodeECPointHexCompressed(pubKey);
            Console.WriteLine("Public key (compressed): " + pubKeyCompressed);

            string addr = CalcRipeMD160(pubKeyCompressed);
            Console.WriteLine("Blockchain address: " + addr);
        }

        private static void ExistingPrivateKeyToAddress(string privKeyHex)
        {
            Console.WriteLine("Existing private key --> public key --> address");
            Console.WriteLine("-----------------------------------------------");

            BigInteger privateKey = new BigInteger(privKeyHex, 16);
            Console.WriteLine("Private key (hex): " + privateKey.ToString(16));
            Console.WriteLine("Private key: " + privateKey.ToString(10));

            ECPoint pubKey = GetPublicKeyFromPrivateKey(privateKey);
            Console.WriteLine("Public key: ({0}, {1})",
                pubKey.XCoord.ToBigInteger().ToString(10),
                pubKey.YCoord.ToBigInteger().ToString(10));

            string pubKeyCompressed = EncodeECPointHexCompressed(pubKey);
            Console.WriteLine("Public key (compressed): " + pubKeyCompressed);

            string addr = CalcRipeMD160(pubKeyCompressed);
            Console.WriteLine("Blockchain address: " + addr);
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

        private static void CreateAndSignTransaction(string recipientAddress, int value,
            string iso8601datetime, string senderPrivKeyHex)
        {
            Console.WriteLine("Generate and sign a transaction");
            Console.WriteLine("-------------------------------");

            Console.WriteLine("Sender private key:", senderPrivKeyHex);
            BigInteger privateKey = new BigInteger(senderPrivKeyHex, 16);

            ECPoint pubKey = GetPublicKeyFromPrivateKey(privateKey);
            string senderPubKeyCompressed = EncodeECPointHexCompressed(pubKey);
            Console.WriteLine("Public key (compressed): " + senderPubKeyCompressed);

            string senderAddress = CalcRipeMD160(senderPubKeyCompressed);
            Console.WriteLine("Blockchain address: " + senderAddress);

            var tran = new
            {
                from = senderAddress,
                to = recipientAddress,
                value = value,
                dateCreated = iso8601datetime,
                senderPubKey = senderPubKeyCompressed,
            };
            string tranJson = JsonConvert.SerializeObject(tran);
            Console.WriteLine("Transaction (JSON): {0}", tranJson);

            byte[] tranHash = CalcSHA256(tranJson);
            Console.WriteLine("Transaction hash(sha256): {0}", BytesToHex(tranHash));

            BigInteger[] tranSignature = SignData(privateKey, tranHash);
            Console.WriteLine("Transaction signature: [{0}, {1}]",
                tranSignature[0].ToString(16), tranSignature[1].ToString(16));

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

            string signedTranJson = JsonConvert.SerializeObject(tranSigned, Formatting.Indented);
            Console.WriteLine("Signed transaction (JSON):");
            Console.WriteLine(signedTranJson);
        }


        static void Main(string[] args)
        {
            RandomPrivateKeyToAddress();

            Console.WriteLine();
            ExistingPrivateKeyToAddress("a1d15353e7dba1c2271c68bd4ea58032af8b46ce93d5b2354587f5ce58139c8e");

            CreateAndSignTransaction(
                recipientAddress: "f51362b7351ef62253a227a77751ad9b2302f911", value: 25,
                iso8601datetime: "2018-02-10T17:53:48.972Z",
                senderPrivKeyHex: "7e4670ae70c98d24f3662c172dc510a085578b9ccc717e6c2f4e547edd960a34");
        }
    }
}

