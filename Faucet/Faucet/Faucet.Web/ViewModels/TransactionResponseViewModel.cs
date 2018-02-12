using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Faucet.Web.ViewModels
{
    public class TransactionResponseViewModel
    {
        public DateTime dateReceived { get; set; }
        public string transactionHash { get; set; }
    }
}