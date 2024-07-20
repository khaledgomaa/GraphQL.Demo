using GraphQL.Demo.DTOs;
using GraphQL.Demo.Models;
using GraphQL.Demo.Services;

namespace GraphQL.Demo.Schema.Queries
{
    public class CourseType
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Subject Subject { get; set; }

        public Guid InstructorId { get; set; }

        [GraphQLNonNullType]
        public async Task<InstructorType> Instructor([Service] IGenericRepository<InstructorDTO> instructorRepository)
        {
            var instructor = await instructorRepository.GetById(InstructorId);

            return new InstructorType { Id = instructor.Id, Name = instructor.Name, Salary = instructor.Salary };
        }

        public IEnumerable<StudentType> Students { get; set; }
    }
}
