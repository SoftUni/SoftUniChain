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

        public void GenerateHashes()
        {
            this.BlockDataHash = "hardcoded data hash, TODO merkel tree and hashing";
            this.CreatedOn = DateTime.UtcNow;
            this.Nonce = 1234567890;
            this.BlockHash = "hardcoded block hash - TODO use mining process";
        }

        private static Block _genesisBlock;
        public static Block Genesis
        {
            get
            {
                if (_genesisBlock == null)
                {

                    _genesisBlock = new Block
                    {
                        Index = 0,
                        Transactions = new List<Transaction>()
                        {
                            new Transaction{
                                From = Address.GeneratorAddress,
                                Amount = 100,
                                To = new Address
                                {
                                    AddressId = "1"
                                },
                                ReceivedOn = DateTime.UtcNow,
                                SenderPublickKey = "hardocoded SenderPublicKey",
                                SenderSignature = new List<string> {"hardcoded SenderSignatoure", "somee" }
                            },
                        },
                        Difficulty = 1,
                        MinedBy = new Address
                        {
                            AddressId = "00",
                            Amount = 10000
                        },
                        PrevBlockHash = String.Empty
                    };
                    _genesisBlock.GenerateHashes();
                }

                return _genesisBlock;
            }
        }
    }
}
