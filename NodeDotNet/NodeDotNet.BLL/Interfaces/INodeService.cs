using NodeDotNet.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeDotNet.BLL.Interfaces
{
    public interface INodeService
    {
        NodeInfoVM GetInfo();
        IEnumerable<BlockVM> GetAllBlocks();
        BlockVM GetBlock(int blockIndex);

        TransactionVM GetTransactionInfo(string transactionHash);
        TransactionCreatedVM AddTransaction(TransactionVM transaction);
    }
}
