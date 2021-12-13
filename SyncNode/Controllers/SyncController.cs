using Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SyncNode.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyncNode.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SyncController : ControllerBase
    {
        private readonly SyncWorkJobService _workJobService; 

        public SyncController(SyncWorkJobService workJobService)
        {
            _workJobService = workJobService;
        }

        [HttpPost]
        IActionResult Sync(SyncEntity entity)
        {
            _workJobService.AddItem(entity);

            return Ok();
        }
    }
}
