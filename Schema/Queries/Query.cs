using GraphQL.Demo.DTOs;
using GraphQL.Demo.Services;
using HotChocolate.Authorization;

namespace GraphQL.Demo.Schema.Queries
{
    public class Query(IGenericRepository<CourseDTO> coursesRepo)
    {
        //private readonly Faker<CourseType> _courseTypeFaker;
        //private readonly Faker<InstructorType> _instructorTypeFaker;
        //private readonly Faker<StudentType> _studentTypeFaker;

        //public Query()
        //{
        //    _courseTypeFaker = new Faker<CourseType>()
        //        .RuleFor(c => c.Id, f => Guid.NewGuid())
        //        .RuleFor(c => c.Name, f => f.Name.JobTitle())
        //        .RuleFor(c => c.Subject, f => f.PickRandom<Subject>())
        //        .RuleFor(c => c.Instructor, f => _instructorTypeFaker.Generate())
        //        .RuleFor(c => c.Students, f => _studentTypeFaker.GenerateBetween(1, 5));
        //    _instructorTypeFaker = new Faker<InstructorType>()
        //        .RuleFor(c => c.Id, f => Guid.NewGuid())
        //        .RuleFor(c => c.Name, f => f.Name.FirstName())
        //        .RuleFor(c => c.Salary, f => f.Random.Double());
        //    _studentTypeFaker = new Faker<StudentType>()
        //        .RuleFor(c => c.Name, f => f.Name.LastName())
        //        .RuleFor(c => c.GPA, f => f.Random.Double());
        //}
        [Authorize]
        [UsePaging(IncludeTotalCount = true, RequirePagingBoundaries = true)]
        public async Task<IEnumerable<CourseType>> GetCourses()
        {
            var courses = await coursesRepo.GetAll();

            return courses.Select(crs => new CourseType
            {
                Id = crs.Id,
                Name = crs.Name,
                Subject = crs.Subject,
                InstructorId = crs.InstructorId
            });
        }

        //[UseDbContext(typeof(AppDbContext))] // This attribute should not be used
        [UsePaging(IncludeTotalCount = true, RequirePagingBoundaries = true)] // Order of these attributes does matter
        //[UseProjection]
        //[UseFiltering(typeof(CourseFilterType))]
        //[UseSorting(typeof(CourseSortType))]
        public IQueryable<CourseType> GetPaginatedCourses(AppDbContext appDbContext)
        {
            var data = appDbContext
                .Courses
                .Skip(1)
                .Take(1)
                .ToList();

            return appDbContext.Courses.Select(crs => new CourseType
            {
                Id = crs.Id,
                Name = crs.Name,
                Subject = crs.Subject,
                InstructorId = crs.InstructorId
            });
        }

        public string Instructions => "GraphQl first demo";
    }
}
