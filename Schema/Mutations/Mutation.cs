using GraphQL.Demo.DTOs;
using GraphQL.Demo.Models;
using GraphQL.Demo.Schema.Subscriptions;
using GraphQL.Demo.Services;
using HotChocolate.Subscriptions;

namespace GraphQL.Demo.Schema.Mutations
{
    public class Mutation(IGenericRepository<CourseDTO> coursesRepo)
    {
        //[Authorize]
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

        public async Task<CourseResult> UpdateCourse(Guid courseId, string name, Subject subject, Guid instructorId, [Service] ITopicEventSender topicEventSender)
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

            await topicEventSender.SendAsync($"{courseId}_{nameof(Subscription.CourseUpdated)}", courseType);

            return courseType;
        }

        public async Task<bool> DeleteCourse(Guid courseId)
        {
            return await coursesRepo.Delete(courseId);
        }
    }
}
