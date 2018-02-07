using NodeDotNet.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NodeDotNet.Core.ViewModels
{
    public class BlockVM
    {
        public int Index { get; set; }
        public IEnumerable<TransactionVM> Transactions { get; set; }
        public long Difficulty { get; set; }
        public string PrevBlockHash { get; set; }
        public string MinedBy { get; set; }
        public string BlockDataHash { get; set; }
        public long Nonce { get; set; }
        public string DateCreated { get; set; }
        public string BlockHash { get; set; }

        public static Func<Block, BlockVM> FromModel
        {
            get
            {
                return b => new BlockVM
                {
                    Index = b.Index,
                    Transactions = b.Transactions.Select(TransactionVM.FromModel),
                    Difficulty = b.Difficulty,
                    PrevBlockHash = b.PrevBlockHash,
                    MinedBy = b.MinedBy.AddressId,
                    BlockDataHash = b.BlockDataHash,
                    Nonce = b.Nonce,
                    DateCreated = b.CreatedOn.ToString("o"),
                    BlockHash = b.BlockHash
                };
            }
        }
    }
}
