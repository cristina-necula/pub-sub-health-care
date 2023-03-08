using Common;
using DataAccess.EFCore;
using DataAccess.EFCore.Repositories;
using DataAccess.EFCore.UnitOfWork;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Publisher.Services;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

# region services config

builder.Services.AddDbContext<ApplicationContext>(
    opt => opt.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName)));

builder.Services.AddSingleton(serviceProvider =>
{
    return new ConnectionFactory { HostName = "localhost" };
});

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IVitalSignRepository, VitalSignRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IVitalSignProcessingService, VitalSignProcessingService>();
builder.Services.AddScoped<IPublisherBase, PublisherBase>();

#endregion

var app = builder.Build();


app.MapGet("/", () => "Hello World!");


app.MapPost("/patient", (string name, IUnitOfWork unitOfWork) =>
{
    var patient = new Patient(name);
    unitOfWork.Patients.Add(patient);
    unitOfWork.Commit();

    return patient.Id;
});

app.MapGet("/patient", (IUnitOfWork unitOfWork) =>
{
    var patients = unitOfWork.Patients.GetAll();
    return patients;

});

app.MapPost("/vitalSigns", (VitalSign vitalSign, IUnitOfWork unitOfWork, IVitalSignProcessingService processingService) =>
{
    processingService.ProcessNewVitalSign(vitalSign);

    unitOfWork.VitalSigns.Add(vitalSign);
    unitOfWork.Commit();


    return vitalSign.Id;
});

app.MapGet("/vitalSigns", (Guid patientId, IUnitOfWork unitOfWork) =>
{
    var vitalSigns = unitOfWork.VitalSigns.GetVitalSignsForPatient(patientId);
    return vitalSigns;
});

app.Run();
