using System;

namespace Faucet.Web.ViewModels
{
    public class TransactionViewModel
    {
        public string To { get; set; }
        public decimal Value { get; set; }
        public string MessageResponse { get; set; }
       // public string TransactionHash { get; set; }

        public string dateReceived { get; set; }
        public string transactionHash { get; set; }
    }
}