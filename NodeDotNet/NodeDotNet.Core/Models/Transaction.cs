using NodeDotNet.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeDotNet.Core.Models
{
    public class Transaction
    {
        private string _transactionHash { get; set; }
        public string TransactionHash
        {
            get
            {
                if(_transactionHash == null)
                {
                    _transactionHash = Crypto.Sha256(TransactionData);
                }

                return _transactionHash;
            }
        }

        public Address From { get; set; }
        public Address To { get; set; }
        public long Amount { get; set; }
        public string SenderPublickKey { get; set; }
        public List<string> SenderSignature { get; set; }
        public long Nonce { get; set; }
        public int MinedInBlockIndex { get; set; }
        public bool Paid { get; set; }

        private string _transactionData;
        public string TransactionData
        {
            get {
                if (_transactionData == null)
                {
                    _transactionData = $"{{'from':'{From.AddressId}','nonce':{Nonce},'value':'{Amount}','to':'{To.AddressId}'}}";
                }

                return _transactionData;
            }
        }

    }
}
