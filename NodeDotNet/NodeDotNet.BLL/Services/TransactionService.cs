using NodeDotNet.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using NodeDotNet.Core.Models;
using System.Numerics;
using NodeDotNet.Core.ViewModels;

namespace NodeDotNet.BLL.Services
{
    public class TransactionService : ITransactionService
    {
        public Transaction Create(TransactionVM transaction)
        {
            Transaction t = new Transaction
            {
                From = new Address(transaction.From),
                To = new Address(transaction.To),
                Amount = transaction.Value,
                Nonce = transaction.Nonce,
                SenderPublickKey = transaction.SenderPublickKey,
                SenderSignature = transaction.SenderSignature
            };

            return t;
        }

        public Transaction Sign(Transaction transaction, string privateKey)
        {
            // get transactionData
            // hash transactionData
            // sign transaction data
            // populate signature in the transaction

            return transaction;
        }

        public bool Validate(Transaction transaction)
        {
            // get transactionData
            // hash transactionData
            // validate signature???

            return true;
        }
    }
}
