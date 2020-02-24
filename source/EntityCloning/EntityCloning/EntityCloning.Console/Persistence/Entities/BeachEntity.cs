using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityCloning.Console.Persistence.Entities
{
    [Table("Beach")]
    public class BeachEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InternalId { get; set; }

        public Guid? Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual List<SandGrainEntity> Grains { get; set; } = new List<SandGrainEntity>();
    }
}
