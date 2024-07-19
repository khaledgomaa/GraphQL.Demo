namespace GraphQL.Demo.Schema.Queries
{
    public class StudentType
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        [GraphQLName("gpa")]
        public double GPA { get; set; }
    }
}
