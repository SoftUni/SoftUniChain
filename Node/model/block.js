/**
 * Created by milenradkov on 2/2/18.
 */
export class Block {
    constructor(index, transactions, difficulty, prevBlockHash, minedBy, blockDataHash, nonce, dateCreated, blockHash) {
        // Index: number
        this.index = index;

        // Transactions: Transaction[]
        this.transactions = transactions;

        // PrevBlockHash: hex_number
        this.prevBlockHash = prevBlockHash.toString();

        // BlockDataHash: address
        this.blockDataHash = blockDataHash.toString();

        // MinedBy: address
        this.minedBy = minedBy;

        // Nonce: number
        this.nonce = nonce;

        // Difficulty: number
        this.difficulty = difficulty;

        // DateCreated: timestamp
        this.dateCreated = dateCreated;

        // BlockHash: hex_number
        this.blockHash = blockHash;
    }
}
