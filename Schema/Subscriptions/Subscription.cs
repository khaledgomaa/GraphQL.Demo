using GraphQL.Demo.Schema.Mutations;

namespace GraphQL.Demo.Schema.Subscriptions
{
    public class Subscription
    {
        [Subscribe]
        public CourseResult CourseCreated([EventMessage] CourseResult course) => course;
    }
}
