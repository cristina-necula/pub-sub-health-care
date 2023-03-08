using DataAccess.EFCore.Repositories;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EFCore.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public IPatientRepository Patients { get; private set; }

        public IVitalSignRepository VitalSigns { get; private set; }

        private readonly ApplicationContext _context;

        public UnitOfWork(ApplicationContext context) 
        { 
            _context = context;
            VitalSigns = new VitalSignRepository(context);
            Patients = new PatientRepository(context);
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
