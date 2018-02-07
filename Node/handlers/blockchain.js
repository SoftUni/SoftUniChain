/**
 * Created by milenradkov on 2/2/18.
 */
let Block = require("../model/block")
let Transaction = require("../model/transaction")
let MiningJob = require("../model/miningJob")
let CryptoJS = require("crypto-js");
let main = require('../index');

module.exports.calculateHash = (index, prevBlockHash, dateCreated, transactions, nonce) => {
        return CryptoJS.SHA256(index + prevBlockHash + dateCreated + transactions + nonce).toString();
}

module.exports.calculateHashForBlock = (block) => {
        return this.calculateHash(block.index, block.prevBlockHash, block.dateCreated, block.transactions, block.nonce);
}

module.exports.addBlock = (newBlock) => {
    if (this.isValidNewBlock(newBlock, this.getLatestBlock())) {
        main.blockchain.push(newBlock);
    }
}

module.exports.isValidNewBlock = (newBlock, previousBlock) => {
        if (previousBlock.index + 1 !== newBlock.index) {
            console.log('Invalid index!');
            return false;
        }

        if (previousBlock.hash !== newBlock.previousHash) {
            console.log('Invalid previous block hash!');
            return false;
        }

        if (this.calculateHashForBlock(newBlock) !== newBlock.hash) {
            console.log(typeof (newBlock.hash) + ' ' + typeof this.calculateHashForBlock(newBlock));
            console.log('Invalid hash: ' + this.calculateHashForBlock(newBlock) + ' ' + newBlock.hash);
            return false;
        }
        return true;
}

module.exports.generateNextBlock = (transactions) => {
        let previousBlock = this.getLatestBlock();
        let nextIndex = previousBlock.index + 1;
        let nextTimestamp = new Date().getTime() / 1000;
        let nextHash = this.calculateHash(nextIndex, previousBlock.blockHash, nextTimestamp, transactions);
        return new Block(nextIndex, previousBlock.blockHash, nextTimestamp, transactions, nextHash);
}

module.exports.miningJob = (minerAddress) => {
	
    let expectedReward = 25;
    let index = this.getLatestBlock().index + 1;

    let coinBaseTransaction = new Transaction(
        "0x0",          // fromAddress
        minerAddress,   // toAddress
        expectedReward, // transactionValue,
        "",             // senderPubKey
        "",             //  senderSignature,
        "",             // transactionHash,
        Date.now(),     // dateReceived,
        index,          // minedInBlockIndex,
        false           // paid
    );

    let pendingTransactions = main.pendingTransactions;
    pendingTransactions.push(coinBaseTransaction);

    let transactions = pendingTransactions;
    let transactionsHash = CryptoJS.SHA256(transactions).toString();
    let prevBlockHash = this.calculateHashForBlock(this.getLatestBlock());

    let jobForMining = new MiningJob(index, expectedReward, transactions, transactionsHash, prevBlockHash , main.difficulty);
    main.miningJobs[minerAddress] = jobForMining;

    return jobForMining;
}

module.exports.calculateHash = (index, previousHash, timestamp, data, nonce) => {
    return CryptoJS.SHA256(index + previousHash + timestamp + data + nonce).toString();
};

module.exports.isValidPOW = (pow) => {
    let miningJob = main.miningJobs[pow.minedBy];
    let validHash = this.calculateHash(miningJob.index, miningJob.prevBlockHash, pow.timestamp, miningJob.transactionsHash, pow.nonce)

    return pow.blockHash === validHash && this.powDetect(pow.blockHash) ;
}

module.exports.powDetect = (hash) => {
        for (var i = 0, b = hash.length; i < b; i ++) {
            if (hash[i] !== '0') {
                break;
            }
        }
        return i === main.difficulty;
}

//Verify job received from miner and add it to blockchain if valid
module.exports.postPOW = (pow) => {

    // {
    //     "index": 1,
    //     "transactionsHash": "4ea5c508a6566e76240543f8feb06fd457777be39549c4016436afda65d2330e",
    //     "prevBlockHash": "b67e5802e3bcd13a30d4e303534e4fee623415da9652704ea53de0d7109f183e",
    //     "minedBy": "0x00",
    //     "dateCreated": 12312893,
    //     "blockHash": "1980dacd198dffe9d17df1267beb918f76012381203",
    //     "nonce": 123
    // }

    //check block
    let newBlock = new Block(
        main.miningJobs[pow.minedBy].index,
        main.miningJobs[pow.minedBy].transactions,
        main.miningJobs[pow.minedBy].difficulty,
        main.miningJobs[pow.minedBy].prevBlockHash,
        pow.minedBy,
        main.miningJobs[pow.minedBy].transactionsHash,
        pow.nonce,
        pow.dateCreated,
        pow.blockHash
    );

    let previousBlock = this.getLatestBlock();

    if (this.isValidNewBlock(newBlock, previousBlock)){
        main.miningJobs[pow.minedBy].transactions.forEach((transaction)=> {
            main.pendingTransactions = main.pendingTransactions.filter(function( tran ) {
                return tran.index !== transaction.index;
            });
        })

        main.blockchain.push(newBlock);
        main.miningJobs[pow.minedBy] = '';
    }



    return pow;
}

module.exports.getLatestBlock = () => main.blockchain[main.blockchain.length - 1];