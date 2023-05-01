using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace EMart.Core.ObjectModel
{
    public class Configurations
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [DataMember]
        public string? Key { get; set; }
        [DataMember]
        public string? Value { get; set; }
    }
}
