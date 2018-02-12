const express = require("express")
const bodyParser = require("body-parser")

let CryptoJS = require("crypto-js");
let handlers = require('./handlers')

let Block = require("./model/block")
let Transaction = require("./model/transaction")

const port = 5555
const app = express()

app.use(bodyParser.json())

app.use(function(req, res, next) {
    res.header("Access-Control-Allow-Origin", "*");
    res.header("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
    next();
});

let faucetAddress = "a1d15353e7dba1c2271c68bd4ea58032af8b46ce93d5b2354587f5ce58139c8e";
let getGenesisBlock = () => {
    return new Block(
        0, //index
        [
            new Transaction(
            "0x0",          // fromAddress
            faucetAddress,   // toAddress
            1000000000000, // transactionValue,
            "",             // senderPubKey
            "",             //  senderSignature,
            "",             // transactionHash,
            Date.now(),     // dateReceived,
            0,          // minedInBlockIndex,
            true           // paid
            )
        ], // transactions array
        5,  // difficulty
        "d279fa6a31ae4fb07cfd9cf7f35cc013cf20a",  // previous block hash
        "f582d57711a618e69d588ce93895d749858fa95b", // mined by
        "5d845cddcd4404ecfd5476fd6b1cf0ea80cd3",  // block data hash
        2455432,  // nonce
        "2018-02-01T23:23:56.337Z", // date created
        '816534932c2b7154836da6afc367695e6337db8a921823784c14378abed4f7d7'); // block hash
};

module.exports.blockchain = [getGenesisBlock()];
module.exports.pendingTransactions = [];
module.exports.miningJobs = [];
module.exports.difficulty = 5;
module.exports.peers = [];
balances = [];
balances[faucetAddress.toString()] = 12398178923123;
module.exports.balances = balances;

module.exports.reward = 25;

app.get('/', handlers.Node.index);
app.get('/info', handlers.Node.getNodeInfo);
app.get('/blocks', handlers.Node.getNodeBlocks);
app.get('/blocks/:index', handlers.Node.getNodeBlockByIndex);
app.get('/balance/:address/confirmations/:confirmCount', handlers.Node.getNodeBalanceByAddress);
app.post('/transactions/new', handlers.Node.postNewTransaction);
app.get('/transactions/:tranHash/info', handlers.Node.getTransactionInfo);
app.get('/transactions/pending', handlers.Node.getPendingTransactiosn);
app.post('/blocks/notify', handlers.Node.newBlockNotify);
app.get('/peers', handlers.Node.getAllPeers);
app.post('/peers', handlers.Node.postNewPeer);
app.get('/mining/get-block/:address', handlers.Node.getMiningBlock);
app.post('/mining/get-block/:address', handlers.Node.postPOW);
app.listen(port, () => console.log(`Server started at port ${port}`))