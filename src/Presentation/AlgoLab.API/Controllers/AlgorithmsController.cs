using AlgoLab.Application.DTOs;
using AlgoLab.Application.Features.Algorithms.GetAlgorithms;
using Microsoft.AspNetCore.Mvc;

namespace AlgoLab.API.Controllers
{
    [ApiController]
    [Route("api/algorithms")]
    public class AlgorithmsController : ControllerBase
    {
        private readonly GetAlgorithmsHandler _handler;

        public AlgorithmsController(GetAlgorithmsHandler handler)
        {
            _handler = handler;
        }

        // GET /api/algorithms
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<AlgorithmDto>>> GetAll(
            CancellationToken ct)
        {
            var list = await _handler.HandleAsync(ct);
            return Ok(list);
        }
    }
}