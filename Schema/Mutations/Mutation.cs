using GraphQL.Demo.DTOs;
using GraphQL.Demo.Models;
using GraphQL.Demo.Schema.Subscriptions;
using GraphQL.Demo.Services.Courses;
using HotChocolate.Subscriptions;

namespace GraphQL.Demo.Schema.Mutations
{
    public class Mutation(CoursesRepository coursesRepo)
    {
        public async Task<CourseResult> CreateCourse(string name, Subject subject, Guid instructorId, [Service] ITopicEventSender topicEventSender)
        {
            CourseDTO courseDTO = await coursesRepo.Create(new()
            {
                Name = name,
                InstructorId = instructorId,
                Subject = subject
            });

            CourseResult courseType = new()
            {
                Id = courseDTO.Id,
                Name = courseDTO.Name,
                Subject = courseDTO.Subject,
                InstructorId = courseDTO.InstructorId
            };

            await topicEventSender.SendAsync(nameof(Subscription.CourseCreated), courseType);

            return courseType;
        }

        public async Task<CourseResult> UpdateCourse(Guid courseId, string name, Subject subject, Guid instructorId)
        {
            CourseDTO courseDTO = await coursesRepo.Update(new()
            {
                Id = courseId,
                Name = name,
                InstructorId = instructorId,
                Subject = subject
            });

            CourseResult courseType = new()
            {
                Id = courseDTO.Id,
                Name = courseDTO.Name,
                Subject = courseDTO.Subject,
                InstructorId = courseDTO.InstructorId
            };

            return courseType;
        }

        public async Task<bool> DeleteCourse(Guid courseId)
        {
            return await coursesRepo.Delete(courseId);
        }
    }
}
