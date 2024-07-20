using GraphQL.Demo.DTOs;
using GraphQL.Demo.Services;

namespace GraphQL.Demo.DataLoaders
{
    public class InstructorDataLoader : BatchDataLoader<Guid, InstructorDTO>
    {
        private readonly IGenericRepository<InstructorDTO> _instructorRepository;

        public InstructorDataLoader(
            IGenericRepository<InstructorDTO> instructorRepository,
            IBatchScheduler batchScheduler,
            DataLoaderOptions options = null)
            : base(batchScheduler, options)
        {
            _instructorRepository = instructorRepository;
        }

        protected async override Task<IReadOnlyDictionary<Guid, InstructorDTO>> LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
        {
            var instructors = await _instructorRepository.GetByIds(keys);

            return instructors.ToDictionary(c => c.Id);
        }
    }
}
