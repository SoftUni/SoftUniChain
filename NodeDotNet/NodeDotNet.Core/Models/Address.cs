using System;
using System.Collections.Generic;
using System.Text;

namespace NodeDotNet.Core.Models
{
    public class Address
    {
        public string AddressId { get; set; }
        public long Amount { get; set; }
        //TODO: possible concurency issues?
        public List<Transaction> Transactions { get; set; }
    }
}
