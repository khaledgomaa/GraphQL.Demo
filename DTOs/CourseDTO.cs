﻿using GraphQL.Demo.Models;

namespace GraphQL.Demo.DTOs
{
    public class CourseDTO : IBaseEntity<Guid>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Subject Subject { get; set; }

        public Guid InstructorId { get; set; }
        public InstructorDTO Instructor { get; set; }

        public IEnumerable<StudentDTO> Students { get; set; }
    }
}
