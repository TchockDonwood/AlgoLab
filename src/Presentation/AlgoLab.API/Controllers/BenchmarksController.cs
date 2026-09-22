using AlgoLab.Application.DTOs;
using AlgoLab.Application.Features.Benchmarks.CancelBenchmark;
using AlgoLab.Application.Features.Benchmarks.GetComparison;
using AlgoLab.Application.Features.Benchmarks.GetHistory;
using AlgoLab.Application.Features.Benchmarks.GetSessionDetails;
using AlgoLab.Application.Features.Benchmarks.StartBenchmark;
using Microsoft.AspNetCore.Mvc;

namespace AlgoLab.API.Controllers
{
    [ApiController]
    [Route("api/benchmarks")]
    public class BenchmarksController : ControllerBase
    {
        private readonly StartBenchmarkHandler _start;
        private readonly CancelBenchmarkHandler _cancel;
        private readonly GetHistoryHandler _history;
        private readonly GetSessionDetailsHandler _details;
        private readonly GetComparisonHandler _comparison;

        public BenchmarksController(
            StartBenchmarkHandler start,
            CancelBenchmarkHandler cancel,
            GetHistoryHandler history,
            GetSessionDetailsHandler details,
            GetComparisonHandler comparison)
        {
            _start = start;
            _cancel = cancel;
            _history = history;
            _details = details;
            _comparison = comparison;
        }

        // POST /api/benchmarks
        [HttpPost]
        public async Task<ActionResult<Guid>> Start(
            [FromBody] StartBenchmarkRequest request,
            CancellationToken ct)
        {
            var id = await _start.HandleAsync(request, ct);
            return CreatedAtAction(nameof(GetDetails), new { id }, id);
        }

        // GET /api/benchmarks
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<BenchmarkSessionDto>>> GetHistory(
            CancellationToken ct)
        {
            var list = await _history.HandleAsync(ct);
            return Ok(list);
        }

        // GET /api/benchmarks/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<BenchmarkDetailsDto>> GetDetails(
            Guid id,
            CancellationToken ct)
        {
            var details = await _details.HandleAsync(id, ct);
            return details is null ? NotFound() : Ok(details);
        }

        // POST /api/benchmarks/{id}/cancel
        [HttpPost("{id:guid}/cancel")]
        public async Task<IActionResult> Cancel(Guid id, CancellationToken ct)
        {
            await _cancel.HandleAsync(id, ct);
            return NoContent();
        }

        // GET /api/benchmarks/comparison?algorithmIds=...&n=...
        [HttpGet("comparison")]
        public async Task<ActionResult<IReadOnlyList<BenchmarkSeriesDto>>> GetComparison(
            [FromQuery] Guid[] sessionIds,
            CancellationToken ct)
        {
            var result = await _comparison.HandleAsync(sessionIds, ct);
            return Ok(result);
        }
    }
}
