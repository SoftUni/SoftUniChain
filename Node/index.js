class Node {
	// Peers: URL[]
	// Blocks: Block[]
	// PendingTransactions: Transaction[]
	// Balances: map(address => number)
	// Difficulty: number
	// MiningJobs: map(address => Block)
}

class Block {
	// Index: number
	// Transactions: Transaction[]
	// Difficulty: number
	// PrevBlockHash: hex_number
	// MinedBy: address
	// BlockDataHash: address
	
	// Nonce: number
	// DateCreated: timestamp
	// BlockHash: hex_number

}

class Transaction {
	// From: address
	// To: address
	// Value: number
	// SenderPubKey: hex_number
	// SenderSignature: hex_number[2]
	
	// TransactionHash: hex_number
	// DateReceived: timestamp
	// MinedInBlockIndex: number
	// Paid: bool
}

class BlockchainNodeServer {
	start(port = 5555) {
		const express = require("express")
		const app = express()
		app.get('/', (req, res) => {
			res.send('SoftUni Chain Blockchain Node')
		})

		app.listen(port, () => console.log(
			`Server started at port ${port}`))
	}
}

let server = new BlockchainNodeServer()
server.start()
