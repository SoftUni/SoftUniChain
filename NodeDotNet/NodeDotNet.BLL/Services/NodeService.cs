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
        private ITransactionService _transactionService;

        private ConcurrentDictionary<string, Peer> _peersByAddress;
        private ConcurrentDictionary<string, Transaction> _confirmedTransactionsById;
        private ConcurrentDictionary<string, Transaction> _pendingTransactionsById;
        private ConcurrentDictionary<string, Address> _addresses;
        //TODO: decide on the collection type, if list introduce locking
        private List<Block> _blockchain;

        
        public NodeService(
            NodeSettings nodeSettings,
            ITransactionService transactionService)
        {
            _nodeSettings = nodeSettings;
            _transactionService = transactionService;

            _peersByAddress = new ConcurrentDictionary<string, Peer>();
            _confirmedTransactionsById = new ConcurrentDictionary<string, Transaction>();
            _pendingTransactionsById = new ConcurrentDictionary<string, Transaction>();
            _addresses = new ConcurrentDictionary<string, Address>();

            _blockchain = new List<Block>();

            ProcessNewBlock(Block.Genesis);
        }

        public IEnumerable<BlockVM> GetAllBlocks()
        {
            var blocks = _blockchain
                .Select(BlockVM.FromModel);
            blocks.Reverse();

            return blocks;
        }

        public BlockVM GetBlock(int blockIndex)
        {
            if(blockIndex <0 || blockIndex > _blockchain.Count)
            {
                throw new Exception($"Block not found[Index='{blockIndex}']");
            }

            var block = _blockchain[blockIndex];

            var blockInfo = BlockVM.FromModel(block);

            return blockInfo;

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

        private void ProcessNewBlock(Block block)
        {
            if (IsBlockValid(block))
            {
                UpdateCollections(block);
                _blockchain.Add(block);
            }
        }

        private void UpdateCollections(Block block)
        {
            foreach(var t in block.Transactions)
            {
                t.From = GetOrAddAddress(t.From);
                t.To = GetOrAddAddress(t.To);

                t.From.Amount -= t.Amount;
                t.To.Amount += t.Amount;

                t.MinedInBlockIndex = block.Index;
                // TODO: What exactly Paid means
                t.Paid = true;

                _confirmedTransactionsById.TryAdd(t.TransactionHash, t);
                _pendingTransactionsById.TryRemove(t.TransactionHash, out var Ignore);
            }
        }

        private Address GetOrAddAddress(string address)
        {
            return GetOrAddAddress(new Address(address));
        }

        private Address GetOrAddAddress(Address address)
        {
            _addresses.TryGetValue(address.AddressId, out var addressFromCache);
            if (addressFromCache == null)
            {
                addressFromCache = address;
                _addresses.TryAdd(address.AddressId, address);
            }

            return addressFromCache;
        }

        private bool IsBlockValid(Block block)
        {
            // TODO: Validate block index, previous blockhash, BlockHash(based on Nonce, date, BlockDataHash)
            // TODO: Validate all transactions - if the amounts are available, signature, TransactionHash
            // Is it possible to have single address 2 times in a block? If yes - will need some kind of temp addresses collection to keep track of the transactions in the block
            // TODO: Validate BlockDataHash

            return true;
        }

        public TransactionCreatedVM AddTransaction(TransactionVM transaction)
        {
            var tr = _transactionService.Create(transaction);
            tr.From = GetOrAddAddress(tr.From);
            tr.To = GetOrAddAddress(tr.To);

            var isValid = _transactionService.Validate(tr);

            if(!isValid)
            {
                //throw?
            }

            if(_confirmedTransactionsById.ContainsKey(tr.TransactionHash))
            {
                throw new Exception($"Transaction['{tr.TransactionHash}'] already processed.");
            }

            if (!_pendingTransactionsById.ContainsKey(tr.TransactionHash))
            {
                _pendingTransactionsById.TryAdd(tr.TransactionHash, tr);
            }
            //TODO: Schedule notify peers

            return new TransactionCreatedVM
            {
                DateReceived = DateTime.UtcNow.ToString("o"),
                TransactionHash = tr.TransactionHash
            };
        }

        public TransactionVM GetTransactionInfo(string transactionHash)
        {
            _pendingTransactionsById.TryGetValue(transactionHash, out var transaction);

            if(transaction == null)
            {
                _confirmedTransactionsById.TryGetValue(transactionHash, out transaction);
            }

            var vm = TransactionVM.FromModel(transaction);

            return vm;
        }
    }
}
