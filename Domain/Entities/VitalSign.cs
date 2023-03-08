using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class VitalSign
    {
        [Key]
        public Guid Id { get; set; }
        public VitalSignType Type { get; set; }
        public double Value { get; set; }
        public DateTime MeasurementTimestamp  { get; set; }
        public Guid PatientId { get; set; }

        private VitalSign() { }

        public VitalSign(VitalSignType type, double value, DateTime measurementTimestamp, Guid patientId)
        {
            Id = Guid.NewGuid();
            Type = type;
            Value = value;
            MeasurementTimestamp = measurementTimestamp;
            PatientId = patientId;
        }
    }
}
