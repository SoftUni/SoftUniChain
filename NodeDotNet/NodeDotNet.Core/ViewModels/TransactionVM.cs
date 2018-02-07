using NodeDotNet.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace NodeDotNet.Core.ViewModels
{
    public class TransactionVM
    {
        public string TransactionHash { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public long Value { get; set; }
        public string SenderPublickKey { get; set; }
        public List<string> SenderSignature { get; set; }
        public long Nonce { get; set; }
        public int MinedInBlockIndex { get; set; }
        public bool Paid { get; set; }

        public static Func<Transaction, TransactionVM> FromModel
        {
            get
            {
                return t => t == null ? null :new TransactionVM
                {
                    TransactionHash = t.TransactionHash,
                    From = t.From.AddressId,
                    To = t.To.AddressId,
                    Value = t.Amount,
                    Nonce = t.Nonce,
                    SenderPublickKey = t.SenderPublickKey,
                    SenderSignature = t.SenderSignature,
                    MinedInBlockIndex = t.MinedInBlockIndex,
                    Paid = t.Paid
                };
            }
        }
    }
}
