using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class BaseEntity
    {
        public Guid Id { get; set; } 

        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid CreatedById { get; set; } = Guid.NewGuid();
        public DateTime? UpdatedOn { get; set; }
        public string? UpdatedById { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
