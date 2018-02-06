/**
 * Created by milenradkov on 2/2/18.
 */
let blockchain = require('./blockchain');
let main = require('../index');
let CryptoJS = require("crypto-js");


module.exports.index = (req, res) => {
    res.send('SoftUni Chain Blockchain Node')
}

module.exports.getNodeInfo = (req, res) => {
    res.setHeader('Content-Type', 'application/json');
    res.send(
        {
            "about": "SoftUniChain/0.9-csharp",
            "name": "Sofia-01",
            "peers": 2,
            "blocks": 25,
            "confirmedTransactions": 208,
            "pendingTransactions": 7,
            "addresses": 12,
            "coins": 18000000
        }
    )
}

module.exports.getNodeBlocks = (req,res) => {
    res.setHeader('Content-Type', 'application/json');
    res.send([
        {
            "index": 0,
            "transactions": [
                {
                    "from": "44fe0696beb6e24541cc0e8728276c9ec3af2675",
                    "to": "9a9f082f37270ff54c5ca4204a0e4da6951fe917",
                    "value": 25.00,
                    "senderPubKey": "2a1d79fb8743d0a4a8501e0028079bcf82a4f�eae1",
                    "senderSignature": ["e20c�a3c29d3370f79f", "cf92�0acd0c132ffe56"],
                    "transactionHash": "4dfc3e0ef89ed603ed54e47435a18b836b�176a",
                    "paid": true,
                    "dateReceived": "2018-02-01T07:47:51.982Z",
                    "minedInBlockIndex": 7
                },
                {
                    "from": "44fe0696beb6e24541cc0e8728276c9ec3af2675",
                    "to": "9a9f082f37270ff54c5ca4204a0e4da6951fe917",
                    "value": 25.00,
                    "senderPubKey": "2a1d79fb8743d0a4a8501e0028079bcf82a4f�eae1",
                    "senderSignature": ["e20c�a3c29d3370f79f", "cf92�0acd0c132ffe56"],
                    "transactionHash": "4dfc3e0ef89ed603ed54e47435a18b836b�176a",
                    "paid": true,
                    "dateReceived": "2018-02-01T07:47:51.982Z",
                    "minedInBlockIndex": 7
                }
            ],
            "difficulty": 5,
            "prevBlockHash": "d279fa6a31ae4fb07cfd9cf7f35cc01f�3cf20a",
            "minedBy": "f582d57711a618e69d588ce93895d749858fa95b",
            "blockDataHash": "5d845cddcd4404ecfd5476fd6b1cf0e5�a80cd3",
            "nonce": 2455432,
            "dateCreated": "2018-02-01T23:23:56.337Z",
            "blockHash": "00000abf2f3d86d5c000c0aa7a425a6a4a65�cf4c"
        },
        {
            "index": 1,
            "transactions": [],
            "difficulty": 5,
            "prevBlockHash": "d279fa6a31ae4fb07cfd9cf7f35cc01f�3cf20a",
            "minedBy": "f582d57711a618e69d588ce93895d749858fa95b",
            "blockDataHash": "5d845cddcd4404ecfd5476fd6b1cf0e5�a80cd3",
            "nonce": 2455432,
            "dateCreated": "2018-02-01T23:23:56.337Z",
            "blockHash": "00000abf2f3d86d5c000c0aa7a425a6a4a65�cf4c"
        }
    ])
}

module.exports.getNodeBlockByIndex = (req,res) => {
    let index = req.params.index;
    res.setHeader('Content-Type', 'application/json');
    res.send(
        {
            "index":index,
            "transactions": [
                {
                    "from": "44fe0696beb6e24541cc0e8728276c9ec3af2675",
                    "to": "9a9f082f37270ff54c5ca4204a0e4da6951fe917",
                    "value": 25.00,
                    "senderPubKey": "2a1d79fb8743d0a4a8501e0028079bcf82a4f�eae1",
                    "senderSignature": ["e20c�a3c29d3370f79f", "cf92�0acd0c132ffe56"],
                    "transactionHash": "4dfc3e0ef89ed603ed54e47435a18b836b�176a",
                    "paid": true,
                    "dateReceived": "2018-02-01T07:47:51.982Z",
                    "minedInBlockIndex": 7
                },
                {
                    "from": "44fe0696beb6e24541cc0e8728276c9ec3af2675",
                    "to": "9a9f082f37270ff54c5ca4204a0e4da6951fe917",
                    "value": 25.00,
                    "senderPubKey": "2a1d79fb8743d0a4a8501e0028079bcf82a4f�eae1",
                    "senderSignature": ["e20c�a3c29d3370f79f", "cf92�0acd0c132ffe56"],
                    "transactionHash": "4dfc3e0ef89ed603ed54e47435a18b836b�176a",
                    "paid": true,
                    "dateReceived": "2018-02-01T07:47:51.982Z",
                    "minedInBlockIndex": 7
                }
            ],
            "difficulty": 5,
            "prevBlockHash": "d279fa6a31ae4fb07cfd9cf7f35cc01f�3cf20a",
            "minedBy": "f582d57711a618e69d588ce93895d749858fa95b",
            "blockDataHash": "5d845cddcd4404ecfd5476fd6b1cf0e5�a80cd3",
            "nonce": 2455432,
            "dateCreated": "2018-02-01T23:23:56.337Z",
            "blockHash": "00000abf2f3d86d5c000c0aa7a425a6a4a65�cf4c"
        }
    )
}

module.exports.getNodeBalanceByAddress = (req,res) => {
    let address = req.params['address'];
    let confirmCount = parseInt(req.params['confirmCount']);
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
    let transactionHash = CryptoJS.SHA256(newTransaction.from + newTransaction.to + newTransaction.value + newTransaction.senderPubKey + newTransaction.senderSignature + newTransaction.timestamp).toString();

    main.pendingTransactions.push(newTransaction);

    res.setHeader('Content-Type', 'application/json');
    res.send(
        {
            "dateReceived": Date.now(),
            "transactionHash": transactionHash
        }
    )
}

module.exports.getMiningBlock = (req, res) => {
    let minerAddress = req.params['address'];
    let miningJob = blockchain.miningJob(minerAddress);

    res.setHeader('Content-Type', 'application/json');
    res.send(miningJob)
}

module.exports.getTransactionInfo = (req,res) => {
    let tranHash = req.params['tranHash'];
    res.setHeader('Content-Type', 'application/json');
    res.send(
        {
            "from": "44fe0696beb6e24541cc0e8728276c9ec3af2675",
            "to": "9a9f082f37270ff54c5ca4204a0e4da6951fe917",
            "value": 25.00,
            "senderPubKey": "2a1d79fb8743d0a4a8501e0028079bcf82a4f�eae1",
            "senderSignature": ["e20c�a3c29d3370f79f", "cf92�0acd0c132ffe56"],
            "transactionHash": tranHash,
            "paid": true,
            "dateReceived": "2018-02-01T07:47:51.982Z",
            "minedInBlockIndex": 7
        }
    )
}

module.exports.newBlockNotify = (req,res) => {
    let blockIndex = parseInt(req.body.index);
    res.setHeader('Content-Type', 'application/json');
    res.send(
        {
            "message": "Thank you!"
        }
    )
}

module.exports.getAllPeers = (req,res) => {
    res.setHeader('Content-Type', 'application/json');
    res.send(
        [
            "http://212.50.11.109:5555",
            "http://af6c7a.ngrok.org:5555"
        ]
    )
}

module.exports.postNewPeer = (req,res) => {
    let nodeUrl = req.body.url;
    res.setHeader('Content-Type', 'application/json');
    res.send(
        {
            "message": "Added peer: " + nodeUrl
        }
    )
}

//Receive mining job done from miner
module.exports.postPOW = (req,res) => {
    let jobDone = blockchain.postPOW(req.body);
    res.setHeader('Content-Type', 'application/json');
    res.send(jobDone)
}

