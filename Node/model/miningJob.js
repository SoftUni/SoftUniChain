/**
 * Created by milenradkov on 2/2/18.
 */
class MiningJob {
    constructor(index, expectedReward, transactions, transactionsHash, prevBlockHash, difficulty){
        this.index = index;
        this.reward = expectedReward;
        this.transactions = transactions;
        this.transactionsHash = transactionsHash;
        this.prevBlockHash = prevBlockHash;
        this.difficulty = difficulty;
    }
}

module.exports = MiningJob