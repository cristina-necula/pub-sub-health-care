using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IVitalSignRepository : IGenericRepository<VitalSign>
    {
        IEnumerable<VitalSign> GetVitalSignsForPatient(Guid patientId);
    }
}
