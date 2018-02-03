using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NodeDotNet.BLL.Interfaces;

namespace NodeDotNet.Api.Controllers
{
    [Route("Blocks")]
    public class BlocksController : Controller
    {
        private INodeService _nodeService;

        public BlocksController(INodeService nodeService)
        {
            _nodeService = nodeService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var blocks = _nodeService.GetAllBlocks();

            return Json(blocks);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var block = _nodeService.GetBlock(id);

            return Json(block);
        }
    }
}
