using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Patient
    {
        [Key]
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public List<VitalSign>? VitalSigns { get; set; }

        private Patient() { }

        public Patient(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }

    }
}
