using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityCloning.Console.Persistence.Entities
{
    [Table("SandGrain")]
    public class SandGrainEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InternalId { get; set; }

        public Guid? Id { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
