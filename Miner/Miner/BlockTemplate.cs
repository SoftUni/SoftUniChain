using System;

namespace Miner
{
    class BlockTemplate
    {
        public Int32 Index { get; set; } // number of block to mine
        //public String PrevBlockHash { get; set; }
        public Decimal ExpectedReward { get; set; }
        public String TransactionsHash { get; set; }
        public String PrevBlockHash { get; set; }
        public Int32 Difficulty { get; set; }
    }
}
