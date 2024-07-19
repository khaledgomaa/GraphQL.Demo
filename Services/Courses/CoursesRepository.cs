using GraphQL.Demo.DTOs;

namespace GraphQL.Demo.Services.Courses
{
    public class CoursesRepository(AppDbContext appDbContext)
    {
        public async Task<CourseDTO> Create(CourseDTO course)
        {
            await appDbContext.AddAsync(course);

            await appDbContext.SaveChangesAsync();

            return course;
        }

        public async Task<CourseDTO> Update(CourseDTO course)
        {
            appDbContext.Update(course);

            await appDbContext.SaveChangesAsync();

            return course;
        }

        public async Task<bool> Delete(Guid courseId)
        {
            appDbContext.Courses.Remove(new() { Id = courseId });

            return await appDbContext.SaveChangesAsync() > 0;
        }
    }
}
