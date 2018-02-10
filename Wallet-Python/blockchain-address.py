from pycoin.ecdsa import generator_secp256k1, sign
import hashlib, bitcoin, json


def ripemd160(msg: str) -> str:
    hash_bytes = hashlib.new('ripemd160', msg.encode("utf8")).digest()
    return hash_bytes.hex()


def sha256(msg: str) -> int:
    hash_bytes = hashlib.sha256(msg.encode("utf8")).digest()
    return int.from_bytes(hash_bytes, byteorder="big")


def random_private_key_to_address():
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


def existing_private_key_to_address(private_key_hex):
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


def sign_transaction(recipient_address: str, value: int, sender_priv_key_hex: str):
    print("Generate and sign a transaction")
    print("-------------------------------")

    sender_priv_key = int(sender_priv_key_hex, 16)
    print("sender private key:", sender_priv_key_hex)
    sender_pub_key = (generator_secp256k1 * sender_priv_key).pair()
    pub_key_compressed = hex(sender_pub_key[0])[2:] + str(sender_pub_key[1] % 2)
    print("sender public key:", pub_key_compressed)
    sender_address = ripemd160(pub_key_compressed)
    print("sender address:", sender_address)

    tran = {'from': sender_address, 'to': recipient_address,
            'value': value, 'senderPubKey': pub_key_compressed}
    json_encoder = json.JSONEncoder(separators=(',',':'))
    tran_json = json_encoder.encode(tran)
    print("transaction (json):", tran_json)
    tran_hash = sha256(tran_json)
    print("transaction hash (sha256):", hex(tran_hash)[2:])
    tran_signature = sign(generator_secp256k1, sender_priv_key, tran_hash)
    print("transaction signature:", tran_signature)
    tran['senderSignature'] = [hex(tran_signature[0])[2:], hex(tran_signature[1])[2:]]
    print("signed transaction:")
    print(json.JSONEncoder(indent=2).encode(tran))


random_private_key_to_address()

print()

existing_private_key_to_address(
    "7e4670ae70c98d24f3662c172dc510a085578b9ccc717e6c2f4e547edd960a34")

print()

sign_transaction(
    recipient_address="f51362b7351ef62253a227a77751ad9b2302f911", value=25,
    sender_priv_key_hex="7e4670ae70c98d24f3662c172dc510a085578b9ccc717e6c2f4e547edd960a34")
