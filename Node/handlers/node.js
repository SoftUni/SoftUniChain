/**
 * Created by milenradkov on 2/2/18.
 */
let blockchain = require('./blockchain');
let main = require('../index');
let CryptoJS = require("crypto-js");

let EC = require('elliptic').ec;
let ec = new EC('secp256k1');

//done
module.exports.index = (req, res) => {
    res.send('SoftUni Chain Blockchain Node')
}

// TODO: confirmed transactions count, commulativedifficulty
module.exports.getNodeInfo = (req, res) => {
    res.setHeader('Content-Type', 'application/json');
    res.send(
        {
            "about": "SoftUniChain/0.9-csharp",
            "nodeName": "Sofia-01",
            "peers": main.peers,
            "difficulty": main.difficulty,
            "blocks": main.blockchain.length,
            "cummulativeDifficulty": 127,
            "confirmedTransactions": 208,
            "pendingTransactions": main.pendingTransactions.length,
        }
    )
}

//done
module.exports.getNodeBlocks = (req,res) => {
    res.setHeader('Content-Type', 'application/json');
    res.send(main.blockchain)
}

//done
module.exports.getNodeBlockByIndex = (req,res) => {
    let index = req.params.index;
    res.setHeader('Content-Type', 'application/json');
    res.send(main.blockchain[index]);
}

// TODO: correct balance data by address
module.exports.getNodeBalanceByAddress = (req,res) => {
    let address = req.params['address'];
    let confirmCount = parseInt(req.params['confirmCount']);

    // TODO: send correct data

    res.setHeader('Content-Type', 'application/json');
    res.send(
        {
            "address": address,
            "confirmedBalance": {"confirmations": confirmCount, "balance": 120.00},
            "lastMinedBalance": {"confirmations": 1, "balance": 115.00},
            "pendingBalance": {"confirmations": 0, "balance": 170.20}
        }
    )
}

module.exports.postNewTransaction = (req,res) => {
    let newTransaction = req.body;

    // TODO : VERSION 2 VERIFY TRANSACTION SIGNATURE
    // let key = ec.keyFromPublic(newTransaction.senderPubKey, 'hex');
    // let verify = key.verify(newTransaction.transactionHash, newTransaction.senderSignature);

    let timestamp = new Date(newTransaction.dateCreated).getTime();
    let transactionHash = CryptoJS.SHA256(newTransaction.from + newTransaction.to + newTransaction.value + newTransaction.senderPubKey + newTransaction.senderSignature + newTransaction.timestamp).toString();

    main.pendingTransactions.push(newTransaction);

    res.setHeader('Content-Type', 'application/json');
    res.send(
        {
            "dateReceived": new Date().toString(),
            "transactionHash": transactionHash
        }
    )
}

//done
module.exports.getMiningBlock = (req, res) => {
    let minerAddress = req.params['address'];
    let miningJob = blockchain.miningJob(minerAddress);

    res.setHeader('Content-Type', 'application/json');
    res.send(miningJob)
}

// TODO: send correct data
module.exports.getTransactionInfo = (req,res) => {
    let tranHash = req.params['tranHash'];
    res.setHeader('Content-Type', 'application/json');
    res.send(
        {
            "from": "44fe0696beb6e24541cc0e8728276c9ec3af2675",
            "to": "9a9f082f37270ff54c5ca4204a0e4da6951fe917",
            "value": 25.00,
            "senderPubKey": "2a1d79fb8743d0a4a8501e0028079bcf82a4feae1",
            "senderSignature": ["e20ca3c29d3370f79f", "cf920acd0c132ffe56"],
            "transactionHash": tranHash,
            "paid": true,
            "dateReceived": "2018-02-01T07:47:51.982Z",
            "minedInBlockIndex": 7
        }
    )
}

//done
module.exports.newBlockNotify = (req,res) => {
    let blockIndex = parseInt(req.body.index);
    res.setHeader('Content-Type', 'application/json');
    res.send(
        {
            "message": "Thank you!"
        }
    )
}

//done
module.exports.getAllPeers = (req,res) => {

    // Return all known peers
    res.setHeader('Content-Type', 'application/json');
    res.send(main.peers);
}

//done
module.exports.postNewPeer = (req,res) => {
    let nodeUrl = req.body.url;

    // Add new peer.
    main.peers.push(nodeUrl);

    res.setHeader('Content-Type', 'application/json');
    res.send(
        {
            "message": "Added peer: " + nodeUrl
        }
    )
}

//done
module.exports.postPOW = (req,res) => {
    //Receive mining job done from miner and assert it
    console.log(req.body);
    let jobDone = blockchain.postPOW(req, res);
    res.setHeader('Content-Type', 'application/json');
    res.send(jobDone);
}

//done
module.exports.getMiningJobs = (req, res) => {
    res.setHeader('Content-Type', 'application/json');
    res.send(main.miningJobs);
}

//done
module.exports.getPendingTransactiosn = (req, res) => {
    res.setHeader('Content-Type', 'application/json');
    res.send(main.pendingTransactions);
}

