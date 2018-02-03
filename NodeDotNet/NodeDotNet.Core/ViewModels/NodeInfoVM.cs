using System;
using System.Collections.Generic;
using System.Text;

namespace NodeDotNet.Core.ViewModels
{
    public class NodeInfoVM
    {
        public string About { get; set; }
        public string NodeName { get; set; }
        public int Peers { get; set; }
        public int Blocks { get; set; }
        public long ConfirmedTransactions { get; set; }
        public long PendingTransactions { get; set; }
        public long Addresses { get; set; }
        public long Coins { get; set; }
    }
}
