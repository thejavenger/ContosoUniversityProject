using System;
using System.Collections.Generic;

using System.ComponentModel;

namespace ContosoUniversity.Models
{
    public class Student
    {
        public int ID { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [DisplayName("First Name")]
        public string FirstMidName { get; set; }
        [DisplayName("Enrollment Date")]
        public DateTime EnrollmentDate { get; set; }
        public string Secret { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}