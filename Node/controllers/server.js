let NodeService = require("../services/node.service")
const express = require("express")
const app = express()
const bodyParser = require("body-parser")

app.use(bodyParser.json())

class NodeServer {
    start(port) {
        let service = new NodeService()

        app.listen(port, () => console.log(`Server started at port ${port}`))

        app.get('/', (req, res) => {
            res.send('SoftUni Chain Blockchain Node')
        })

        app.get('/info', (req, res) => {
            service.httpGetNodeInfo(req, res)
        })

        app.get('/blocks', (req, res) => {
            service.httpGetNodeBlocks(req, res)
        })

        app.get('/blocks/:index', (req, res) => {
            service.httpGetNodeBlockByIndex(req, res, parseInt(req.params['index']))
        })

        app.get('/balance/:address/confirmations/:confirmCount', (req, res) => {
            service.httpGetNodeBalanceByAddress(req, res, req.params['address'], parseInt(req.params['confirmCount']))
        })

        app.post('/transactions/new', (req, res) => {
            service.httpPostNewTransaction(req, res, req.body)
        })

        app.get('/transactions/:tranHash/info', (req, res) => {
            service.httpGetTransactionInfo(req, res, req.params['tranHash'])
        })

        app.post('/blocks/notify', (req, res) => {
            service.httpNewBlockNotify(req, res, parseInt(req.body.index))
        })

        app.get('/peers', (req, res) => {
            service.httpGetAllPeers(req, res)
        })

        app.post('/peers', (req, res) => {
            service.httpPostNewPeer(req, res, req.body.url)
        })
    }
}

module.exports = NodeServer