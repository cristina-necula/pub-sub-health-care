using Domain.Entities;
using Domain.Interfaces;

namespace DataAccess.EFCore.Repositories
{
    public class VitalSignRepository : GenericRepository<VitalSign>, IVitalSignRepository
    {
        public VitalSignRepository(ApplicationContext context) : base(context)
        {
        }

        public IEnumerable<VitalSign> GetVitalSignsForPatient(Guid patientId)
        {
            return _context.VitalSigns.Where(v => v.PatientId == patientId);
        }
    }
}
