using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Patients.CatalogAPI.DataContext;
using Patients.CatalogAPI.Models;

namespace Patients.CatalogAPI.Apis;
public static class PatientsApi
{
    public static void UsePatientsApi(this IEndpointRouteBuilder endpoints)
    {
        var patientsGroup = endpoints.MapGroup("Patients");
        patientsGroup.MapGet("", GetAll);
        patientsGroup.MapPost("", AddNewPatient);
        patientsGroup.MapGet("{id}", GetById);
    }

    async static Task<Ok<List<PatientModel>>> GetAll(PatientsDataContext context)
    {
        var result = await context.Patients.ToListAsync();
        return TypedResults.Ok(result);
    }

    async static Task<Results<Ok<PatientModel>, NotFound>> GetById(int id, PatientsDataContext context)
    {
        var result = await context.Patients.FirstOrDefaultAsync(x => x.id == id);
        if (result is not null)
        {
            return TypedResults.Ok(result);
        }

        return TypedResults.NotFound();
    }


    async static Task<Created<PatientModel>> AddNewPatient(PatientModel patient, PatientsDataContext context)
    {
        await context.Patients.AddAsync(patient);
        await context.SaveChangesAsync();
        return TypedResults.Created($"/Patients/{patient.id}", patient);
    }

}
