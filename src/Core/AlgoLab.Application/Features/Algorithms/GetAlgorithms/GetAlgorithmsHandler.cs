using AlgoLab.Application.Common.Interfaces;
using AlgoLab.Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace AlgoLab.Application.Features.Algorithms.GetAlgorithms
{
    public class GetAlgorithmsHandler
    {
        private readonly IApplicationDbContext _db;

        public GetAlgorithmsHandler(IApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IReadOnlyList<AlgorithmDto>> HandleAsync(
            CancellationToken cancellationToken)
        {
            return await _db.Algorithms
                .AsNoTracking()
                .OrderBy(a => a.Name)
                .Select(a => new AlgorithmDto(
                    a.Id,
                    a.Code,
                    a.Name
                ))
                .ToListAsync(cancellationToken);
        }
    }
}