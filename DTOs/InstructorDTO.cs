namespace GraphQL.Demo.DTOs
{
    public class InstructorDTO : IBaseEntity<Guid>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public double Salary { get; set; }
    }
}