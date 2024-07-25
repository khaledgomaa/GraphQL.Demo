using GraphQL.Demo.Schema.Queries;
using HotChocolate.Data.Sorting;

namespace GraphQL.Demo.Schema.Sorters
{
    public class CourseSortType : SortInputType<CourseType>
    {
        protected override void Configure(ISortInputTypeDescriptor<CourseType> descriptor)
        {
            descriptor.Ignore(x => x.Id);
            descriptor.Ignore(x => x.Students);
            descriptor.Ignore(x => x.InstructorId);
        }
    }
}
