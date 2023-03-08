using Common;
using Domain.Entities;
using Domain.Interfaces;

namespace Publisher.Services
{
    public class VitalSignProcessingService : IVitalSignProcessingService
    {
        private const double CRITICAL_TRESHOLD = 25;
        private const double WARNING_TRESHOLD = 10;

        private readonly IUnitOfWork _unitOfWork;
        private readonly IPublisherBase _publisherBase;
        public VitalSignProcessingService(IUnitOfWork unitOfWork, IPublisherBase publisherBase)
        {
            _unitOfWork = unitOfWork;
            _publisherBase = publisherBase;
        }

        public void ProcessNewVitalSign(VitalSign vitalSign)
        {
            var oldVitalSigns = _unitOfWork.VitalSigns
                .GetVitalSignsForPatient(vitalSign.PatientId)
                .OrderByDescending(x => x.MeasurementTimestamp)
                .Where(x => x.Type == vitalSign.Type)
                .Take(20);

            if (!oldVitalSigns.Any())
            {
                Console.WriteLine("No previous vital signs. Skipping processing...");
                return;
            }

            var oldVitalSignAvg = oldVitalSigns.Average(x => x.Value);

            var variation = Math.Abs(oldVitalSignAvg - vitalSign.Value);

            string routingKey = "";
            string message = $"New {vitalSign.Type} reading of {vitalSign.Value} for patient {vitalSign.Id}";

            if (variation > CRITICAL_TRESHOLD)
            {
                routingKey = $"{vitalSign.Type}.critical";
            }
            else if (variation > WARNING_TRESHOLD)
            {
                routingKey = $"{vitalSign.Type}.warning";
            }
            else
            {
                routingKey = $"{vitalSign.Type}.normal";
            }

            _publisherBase.Publish(message, routingKey);
        }
    }
}
