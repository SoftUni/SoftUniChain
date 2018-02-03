using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NodeDotNet.BLL.Interfaces;

namespace NodeDotNet.Api.Controllers
{
    [Route("Info")]
    public class InfoController : Controller
    {
        private INodeService _nodeService;

        public InfoController(INodeService nodeService)
        {
            _nodeService = nodeService;
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            var info = _nodeService.GetInfo();

            return Json(info);
        }
    }
}
