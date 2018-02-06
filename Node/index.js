const express = require("express")
const bodyParser = require("body-parser")

let CryptoJS = require("crypto-js");
let handlers = require('./handlers')

let Block = require("./model/block")

const port = 5555
const app = express()

app.use(bodyParser.json())

let getGenesisBlock = () => {
    return new Block(0, [], 5, "d279fa6a31ae4fb07cfd9cf7f35cc013cf20a", "f582d57711a618e69d588ce93895d749858fa95b", "5d845cddcd4404ecfd5476fd6b1cf0ea80cd3", 2455432, "2018-02-01T23:23:56.337Z", '816534932c2b7154836da6afc367695e6337db8a921823784c14378abed4f7d7');
};

module.exports.blockchain = [getGenesisBlock()];
module.exports.pendingTransactions = [];
module.exports.miningJobs = [];

app.get('/', handlers.Node.index);
app.get('/info', handlers.Node.getNodeInfo);
app.get('/blocks', handlers.Node.getNodeBlocks);
app.get('/blocks/:index', handlers.Node.getNodeBlockByIndex);
app.get('/balance/:address/confirmations/:confirmCount', handlers.Node.getNodeBalanceByAddress);
app.post('/transactions/new', handlers.Node.postNewTransaction);
app.get('/transactions/:tranHash/info', handlers.Node.getTransactionInfo);
app.post('/blocks/notify', handlers.Node.newBlockNotify);
app.get('/peers', handlers.Node.getAllPeers);
app.post('/peers', handlers.Node.postNewPeer);
app.get('/mining/get-block/:address', handlers.Node.getMiningBlock);
app.post('/mining/pow', handlers.Node.postPOW);
app.listen(port, () => console.log(`Server started at port ${port}`))