using System.Collections.Generic;
using System.Threading.Tasks;
using Crud.Data;
using Crud.Models;
using Microsoft.EntityFrameworkCore;

namespace Crud.Services;

public class PersonService(IGenericRepository<Person> repository):IPersonService
{
    public async Task<List<Person>> GetAll()
    {
        var people = await repository.Get().ToListAsync();
        return people;
    }

    public async Task<Person> Add(Person person)
    {
        var newPerson =  await repository.Add(person);
        return newPerson;
    }

    public async Task<bool> Update(Person person)
    {
        var currentPerson = await repository.Get()
            .FirstOrDefaultAsync(p => p.Id == person.Id);
        if (currentPerson == null) return false;
        currentPerson.Name = person.Name;
        currentPerson.BirthDate = person.BirthDate;
        currentPerson.Gender = person.Gender;
        currentPerson.AcceptTerms=person.AcceptTerms;
        var result=await repository.Update(currentPerson);
        return result;
    }

    public async Task<bool> Delete(int id)
    {
        return await repository.Delete(id);
    }
}