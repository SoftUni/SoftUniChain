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
        //public List<Transaction> Transactions { get; set; }

        public Address(string address)
        {
            //TODO: validate address?
            this.AddressId = address;
        }

        private static Address _generatorAddress;
        public static Address GeneratorAddress
        {
            get
            {
                if (_generatorAddress == null)
                {
                    _generatorAddress = new Address("0"); ;
                }

                return _generatorAddress;
            }
        }
    }
}
