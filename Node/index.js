let NodeServer = require("./controllers/server")

port = 5555
let server = new NodeServer()
server.start(port)
