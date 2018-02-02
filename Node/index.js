const express = require("express")
const app = express()
const bodyParser = require("body-parser")
const handlers = require('./handlers')
const port = 5555
app.use(bodyParser.json())

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
app.listen(port, () => console.log(`Server started at port ${port}`))
	

// class Node {
//     // Peers: URL[]
//     // Blocks: Block[]
//     // PendingTransactions: Transaction[]
//     // Balances: map(address => number)
//     // Difficulty: number
//     // MiningJobs: map(address => Block)
// }
//
// class Block {
//     // Index: number
//     // Transactions: Transaction[]
//     // Difficulty: number
//     // PrevBlockHash: hex_number
//     // MinedBy: address
//     // BlockDataHash: address
//
//     // Nonce: number
//     // DateCreated: timestamp
//     // BlockHash: hex_number
//
// }
//
// class Transaction {
//     // From: address
//     // To: address
//     // Value: number
//     // SenderPubKey: hex_number
//     // SenderSignature: hex_number[2]
//
//     // TransactionHash: hex_number
//     // DateReceived: timestamp
//     // MinedInBlockIndex: number
//     // Paid: bool
// }