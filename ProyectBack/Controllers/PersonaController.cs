using AttestationAPI.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectModels;
using ProyectRepository.PersonRepository;

namespace ProyectBack.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class PersonaController : PYController
    {
      

        [HttpGet]
        [AllowAnonymous]
        public Persona GetPerson()
        {
            var persona = new Persona();

            PersonRepository personaRepository = new(dbContext);      
            return personaRepository.GetPersonRepository();
        }

        [HttpGet]
        [AllowAnonymous]
        public List<Persona> GetAll()
        {
            PersonRepository personaRepository = new(dbContext);
            return personaRepository.GetAllPersonRepository();
        }

        [HttpPut]
        [AllowAnonymous]
        public Task<int> DeletePerson(int id)
        {
            PersonRepository personaRepository = new(dbContext);
            return personaRepository.DeleteLogicalPersonRepository(id);
        }

        [HttpPut]
        [AllowAnonymous]
        public Task<int> ResetAllActive()
        {
            PersonRepository personaRepository = new(dbContext);
            return personaRepository.ResetAllActiveRepository();
        }

    }
}