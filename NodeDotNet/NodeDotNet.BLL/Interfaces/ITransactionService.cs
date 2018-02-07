using NodeDotNet.Core.Models;
using NodeDotNet.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace NodeDotNet.BLL.Interfaces
{
    public interface ITransactionService
    {
        Transaction Create(TransactionVM transaction);
        Transaction Sign(Transaction transaction, string privateKey);
        bool Validate(Transaction transaction);
    }
}
