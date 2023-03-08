using Domain.Entities;

namespace Publisher.Services
{
    public interface IVitalSignProcessingService
    {
        void ProcessNewVitalSign(VitalSign vitalSign);
    }
}
