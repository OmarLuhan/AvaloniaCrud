using System.Collections.Generic;
using System.Threading.Tasks;
using Crud.Models;

namespace Crud.Services;

public interface IPersonService
{
    Task<List<Person>> GetAll();
    Task<Person>Add(Person person);
    Task<bool> Update(Person person);
    Task<bool> Delete(int id);
}