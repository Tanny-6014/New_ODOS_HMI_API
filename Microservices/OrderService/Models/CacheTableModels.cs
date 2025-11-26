using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderService.Models
{
    [Table("CacheTable")]
    public class CacheTableModels
    {
         [Key]
         public int Id { get; set; }

         public string TokenKey { get; set; }

         public string Value { get; set; }

         public DateTimeOffset ExpiresAtTime { get; set; }
    }
}
