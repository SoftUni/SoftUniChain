/**
 * Created by milenradkov on 2/2/18.
 */
let CryptoJS = require("crypto-js");
let main = require('../index');

module.exports.getPendingTransactions = () => {
    return main.pendingTransactions;
}

module.exports.insertTransaction = (transaction) => {
    main.pendingTransactions.push(transaction);
    return true;
}