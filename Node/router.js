var express = require('express');
var router = express.Router();

/* GET home page. */
router.get('/info', function(req, res, next) {
   this.httpGetNodeInfo(req, res)
})

module.exports = router;
