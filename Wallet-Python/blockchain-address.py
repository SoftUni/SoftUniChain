from pycoin.ecdsa import generator_secp256k1
import hashlib, bitcoin


def ripemd160(msg) :
    hash_bytes = hashlib.new('ripemd160', msg.encode("utf8")).digest()
    return hash_bytes.hex()


def randomPrivateKeyToAddress():
    print("Random private key --> public key --> address")
    print("---------------------------------------------")

    private_key_hex = bitcoin.random_key()
    print("private key (hex):", private_key_hex)
    private_key = int(private_key_hex, 16)
    print("private key:", private_key)

    public_key = (generator_secp256k1 * private_key).pair()
    print("public key:", public_key)
    public_key_compressed = hex(public_key[0])[2:] + str(public_key[1] % 2)
    print("public key (compressed):", public_key_compressed)

    addr = ripemd160(public_key_compressed)
    print("blockchain address:", addr)


def existingPrivateKeyToAddress(private_key_hex):
    print("Existing private key --> public key --> address")
    print("-----------------------------------------------")

    print("private key (hex):", private_key_hex)
    private_key = int(private_key_hex, 16)
    print("private key:", private_key)

    public_key = (generator_secp256k1 * private_key).pair()
    print("public key:", public_key)
    public_key_compressed = hex(public_key[0])[2:] + str(public_key[1] % 2)
    print("public key (compressed):", public_key_compressed)

    addr = ripemd160(public_key_compressed)
    print("blockchain address:", addr)


randomPrivateKeyToAddress()

print()

existingPrivateKeyToAddress("a1d15353e7dba1c2271c68bd4ea58032af8b46ce93d5b2354587f5ce58139c8e")
