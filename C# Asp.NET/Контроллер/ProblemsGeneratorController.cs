using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using GeneratorLibrary;
using System.Security.Cryptography;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProblemsGeneratorController : ControllerBase
    {
        private readonly ILogger<ProblemsGeneratorController> _logger;

        public ProblemsGeneratorController(ILogger<ProblemsGeneratorController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Problem> Get(int num, int tasks = 1, int paragraph = 1, int seed = 0)
        {
            tasks = (tasks - 1) % 20 + 1;
            num = (num - 1) % 100 + 1;
            paragraph = (paragraph - 1) % 4 + 1;
            if (seed == 0)
            {
                seed = (int)(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() % 10000);
            }

            return Enumerable.Range(1, num).Select(index => new Problem(paragraph, tasks, seed, index))
            .ToArray();
        }
    }
}
