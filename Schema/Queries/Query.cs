using Bogus;
using GraphQL.Demo.Models;

namespace GraphQL.Demo.Schema.Queries
{
    public class Query
    {
        private readonly Faker<CourseType> _courseTypeFaker;
        private readonly Faker<InstructorType> _instructorTypeFaker;
        private readonly Faker<StudentType> _studentTypeFaker;

        public Query()
        {
            _courseTypeFaker = new Faker<CourseType>()
                .RuleFor(c => c.Id, f => Guid.NewGuid())
                .RuleFor(c => c.Name, f => f.Name.JobTitle())
                .RuleFor(c => c.Subject, f => f.PickRandom<Subject>())
                .RuleFor(c => c.Instructor, f => _instructorTypeFaker.Generate())
                .RuleFor(c => c.Students, f => _studentTypeFaker.GenerateBetween(1, 5));
            _instructorTypeFaker = new Faker<InstructorType>()
                .RuleFor(c => c.Id, f => Guid.NewGuid())
                .RuleFor(c => c.Name, f => f.Name.FirstName())
                .RuleFor(c => c.Salary, f => f.Random.Double());
            _studentTypeFaker = new Faker<StudentType>()
                .RuleFor(c => c.Name, f => f.Name.LastName())
                .RuleFor(c => c.GPA, f => f.Random.Double());
        }

        public IEnumerable<CourseType> GetCourses()
        {
            return _courseTypeFaker.Generate(5);
        }

        public string Instructions => "GraphQl first demo";
    }
}
