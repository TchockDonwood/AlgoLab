using AlgoLab.Application.DTOs;
using AlgoLab.Application.Features.Benchmarks.StartBenchmark;
using Microsoft.AspNetCore.Mvc;

namespace AlgoLab.API.Controllers
{
    [ApiController]
    [Route("api/benchmarks")]
    public class BenchmarksController : ControllerBase
    {
        private readonly StartBenchmarkHandler _startBenchmark;

        public BenchmarksController(
            StartBenchmarkHandler startBenchmark)
        {
            _startBenchmark = startBenchmark;
        }

        [HttpPost]
        public async Task<IActionResult> Start(
            [FromBody] StartBenchmarkRequest request,
            CancellationToken cancellationToken)
        {
            var id = await _startBenchmark.HandleAsync(
                request,
                cancellationToken);

            return Accepted(new
            {
                id
            });
        }
    }
}
