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

public class WalletCSharp
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

    static void Main()
    {
        RandomPrivateKeyToAddress();

        Console.WriteLine();
        ExistingPrivateKeyToAddress("a1d15353e7dba1c2271c68bd4ea58032af8b46ce93d5b2354587f5ce58139c8e");
    }
}
