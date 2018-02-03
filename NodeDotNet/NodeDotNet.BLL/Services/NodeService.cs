using NodeDotNet.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using NodeDotNet.Core.ViewModels;
using NodeDotNet.Core.Models;
using System.Collections.Concurrent;
using System.Linq;

namespace NodeDotNet.BLL.Services
{
    public class NodeService : INodeService
    {
        private NodeSettings _nodeSettings;
        private ConcurrentDictionary<string, Peer> _peersByAddress;
        private ConcurrentDictionary<string, Transaction> _confirmedTransactionsById;
        private ConcurrentDictionary<string, Transaction> _pendingTransactionsById;
        private ConcurrentDictionary<string, Address> _addresses;
        //TODO: decide on the collection type, if list introduce locking
        private List<Block> _blockchain;

        
        public NodeService(NodeSettings nodeSettings)
        {
            _nodeSettings = nodeSettings;

            _peersByAddress = new ConcurrentDictionary<string, Peer>();
            _confirmedTransactionsById = new ConcurrentDictionary<string, Transaction>();
            _pendingTransactionsById = new ConcurrentDictionary<string, Transaction>();
            _addresses = new ConcurrentDictionary<string, Address>();

            _blockchain = new List<Block>();
        }

        public NodeInfoVM GetInfo()
        {
            var info = new NodeInfoVM
            {
                About = _nodeSettings.About,
                NodeName = _nodeSettings.Name,
                Peers = _peersByAddress.Count,
                Blocks = _blockchain.Count,
                ConfirmedTransactions = _confirmedTransactionsById.Count,
                PendingTransactions = _pendingTransactionsById.Count,
                Addresses = _addresses.Count,
                //TODO: this number can be cached, changes only when the blockchain is modified
                Coins = _addresses.Sum(a => a.Value.Amount)
            };

            return info;
        }
    }
}
