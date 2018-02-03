using System;
using System.Collections.Generic;
using System.Text;

namespace NodeDotNet.Core.Models
{
    public class Transaction
    {
        public string TransactionHash { get; set; }
        public Address From { get; set; }
        public Address To { get; set; }
        public long Amount { get; set; }
        public string SenderPublickKey { get; set; }
        public string SenderSignature { get; set; }
        public DateTime ReceivedOn { get; set; }
        public int MinedInBlockIndex { get; set; }
        public bool Paid { get; set; }
    }
}
