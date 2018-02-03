/**
 * Created by milenradkov on 2/2/18.
 */
class Transaction {
        constructor(fromAddress,
                    toAddress,
                    transactionValue,
                    senderPubKey,
                    senderSignature,
                    transactionHash,
                    dateReceived,
                    minedInBlockIndex,
                    paid)
        {
        // From: address
        this.fromAddress = fromAddress;

        // To: address
        this.toAddress = toAddress;

        // Value: number
        this.value = transactionValue;

        // SenderPubKey: hex_number
        this.senderPubKey = senderPubKey;

        // SenderSignature: hex_number[2]
        this.senderSignature = senderSignature;

        // TransactionHash: hex_number
        this.transactionHash = transactionHash;

        // DateReceived: timestamp
        this.dateReceived = dateReceived;

        // MinedInBlockIndex: number
        this.minedInBlockIndex = minedInBlockIndex;

        // Paid: bool
        this.paid = paid;
    }
}

module.exports = Transaction