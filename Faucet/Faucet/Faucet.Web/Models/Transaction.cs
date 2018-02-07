using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Faucet.Web.Models
{
    public class Transaction
    {
        public int From { get; set; }
        public int To { get; set; }
        public int Value { get; set; }
        public int Nonce { get; set; }
        public int SenderPubKey { get; set; }
        public int SenderSignature { get; set; }
    }
}