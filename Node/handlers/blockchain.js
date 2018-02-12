/**
 * Created by milenradkov on 2/2/18.
 */
let Block = require("../model/block")
let Transaction = require("../model/transaction")
let MiningJob = require("../model/miningJob")
let CryptoJS = require("crypto-js");
let main = require('../index');

module.exports.calculateHash = (index, transactions, prevBlockHash, dateCreated, nonce) => {

    let precomputedData = index +''+ CryptoJS.SHA256(transactions) +''+ prevBlockHash;
    let data = precomputedData + '' + dateCreated + '' + nonce;
    let computedHash = CryptoJS.SHA256(data).toString();

    return computedHash;
}

module.exports.calculateHashForBlock = (block) => {
        return this.calculateHash(block.index, block.transactions, block.prevBlockHash, block.dateCreated, block.nonce);
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

        if (this.calculateHashForBlock(newBlock).toUpperCase() !== newBlock.blockHash.toUpperCase()) {
            console.log(typeof (newBlock.blockHash) + ' ' + typeof this.calculateHashForBlock(newBlock));
            console.log('Invalid hash: ' + this.calculateHashForBlock(newBlock) + ' ' + newBlock.blockHash);
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
module.exports.postPOW = (req, res) => {

    let minerAddress = req.params['address'];
    let pow = req.body;
    let previousBlock = this.getLatestBlock();
    let newBlockIndex = previousBlock.index + 1;
    let newBlock = new Block(
        newBlockIndex,
        main.miningJobs[minerAddress.toString()].transactions,
        main.miningJobs[minerAddress.toString()].difficulty,
        main.miningJobs[minerAddress.toString()].prevBlockHash,
        minerAddress,
        main.miningJobs[minerAddress.toString()].transactionsHash,
        pow.nonce,
        pow.dateCreated,
        pow.blockHash
    );

    if (this.isValidNewBlock(newBlock, previousBlock)){
        main.miningJobs[minerAddress.toString()].transactions.forEach((transaction)=> {
            main.pendingTransactions = main.pendingTransactions.filter(function( tran ) {
                return tran.index !== transaction.index;
            });
        })

        main.blockchain.push(newBlock);
        main.miningJobs[minerAddress.toString()] = '';
    }

    return pow;
}

module.exports.getLatestBlock = () => main.blockchain[main.blockchain.length - 1];