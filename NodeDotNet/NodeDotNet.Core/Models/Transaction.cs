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
                    _transactionHash = "hardcodedTrHash" + From.AddressId;
                }

                return _transactionHash;
            }
        }
        public Address From { get; set; }
        public Address To { get; set; }
        public long Amount { get; set; }
        public string SenderPublickKey { get; set; }
        public List<string> SenderSignature { get; set; }
        public DateTime ReceivedOn { get; set; }
        public int MinedInBlockIndex { get; set; }
        public bool Paid { get; set; }
    }
}
