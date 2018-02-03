using System;
using System.Collections.Generic;
using System.Text;

namespace NodeDotNet.Core.Models
{
    public class Block
    {
        public int Index { get; set; }
        public List<Transaction> Transactions { get; set; }
        public long Difficulty { get; set; }
        public string PrevBlockHash { get; set; }
        public Address MinedBy { get; set; }
        public string BlockDataHash { get; set; }
        public long Nonce { get; set; }
        public DateTime CreatedOn { get; set; }
        public string BlockHash { get; set; }
    }
}
