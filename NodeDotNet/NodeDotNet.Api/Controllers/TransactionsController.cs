using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NodeDotNet.BLL.Interfaces;
using NodeDotNet.Core.ViewModels;

namespace NodeDotNet.Api.Controllers
{
    [Route("Transactions")]
    public class TransactionsController : Controller
    {
        private INodeService _nodeService;

        public TransactionsController(INodeService nodeService)
        {
            _nodeService = nodeService;
        }

        [HttpGet("{transactionHash}/info")]
        public IActionResult Get(string transactionHash)
        {
            var transaction = _nodeService.GetTransactionInfo(transactionHash);

            return Json(transaction);
        }

        [HttpPost]
        public IActionResult Get([FromBody]TransactionVM transaction)
        {
            var response = _nodeService.AddTransaction(transaction);

            return Json(response);
        }
    }
}
