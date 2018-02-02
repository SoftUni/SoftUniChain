/**
 * Created by milenradkov on 2/2/18.
 */
class Node {
    constructor(peers, blocks, pendingTransactions, balances, difficulty, miningJobs){
        // Peers: URL[]
        this.peers = peers;

        // Blocks: Block[]
        this.blocks = blocks;

        // PendingTransactions: Transaction[]
        this.pendingTransactions = pendingTransactions;

        // Balances: map(address => number)
        this.balances = balances;

        // Difficulty: number
        this.difficulty = difficulty;

        // MiningJobs: map(address => Block)
        this.miningJobs = miningJobs;
    }
}



