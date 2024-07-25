using GraphQL.Demo.Schema.Queries;
using HotChocolate.Data.Filters;

namespace GraphQL.Demo.Schema.Filters
{
    public class CourseFilterType : FilterInputType<CourseType>
    {
        protected override void Configure(IFilterInputTypeDescriptor<CourseType> descriptor)
        {
            descriptor.Ignore(crs => crs.Students);

            base.Configure(descriptor);
        }
    }
}
