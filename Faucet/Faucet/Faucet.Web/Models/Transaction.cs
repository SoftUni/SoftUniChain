using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Faucet.Web.Models
{
    public class Transaction
    {
        public string From { get; set; }
        public string To { get; set; }
        public decimal Value { get; set; }
        public int Nonce { get; set; }
        public string SenderPubKey { get; set; }
        public string SenderSignature { get; set; }
    }
}