﻿namespace GraphQL.Demo.DTOs
{
    public class StudentDTO : IBaseEntity<Guid>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public double GPA { get; set; }

        public IEnumerable<CourseDTO> Courses { get; set; }
    }
}