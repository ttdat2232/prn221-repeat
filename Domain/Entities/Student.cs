﻿using Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Student : BaseEntity
    {
        [Column (TypeName = "nvarchar(100)")]
        public string Name { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public long MajorId { get; set; }
        public virtual Major? Major { get; set; }
        public virtual ICollection<Participant>? ParticipatedActivity { get; set; }
        public virtual ICollection<Membership> Memberships { get; set; }
    }

    public enum StudentStatus
    {

    }
}
