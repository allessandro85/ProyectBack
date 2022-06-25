using ProyectIO.PersonIO;
using ProyectRepository.Services;
using ProyectModels;

namespace ProyectRepository.PersonRepository
{
    public class PersonRepository : Services.ServiceWithDbContext
    {
        public PersonRepository(DBContext dBContext) : base(dBContext) { }
        public Persona GetPersonRepository()
        {
            var personaIO = new PersonIO(_dbContext.Connection, _dbContext.Transaction);
            return personaIO.GetPersonIO();
        }

        public Task<int> DeleteLogicalPersonRepository(int id)
        {
            var personaIO = new PersonIO(_dbContext.Connection, _dbContext.Transaction);
            return personaIO.DeleteLogicalPersonIO(id);
        }

        public Task<int> ResetAllActiveRepository()
        {
            var personaIO = new PersonIO(_dbContext.Connection, _dbContext.Transaction);
            return personaIO.ResetAllActivePersonIO();
        }

        public List<Persona> GetAllPersonRepository()
        {
            var personaIO = new PersonIO(_dbContext.Connection, _dbContext.Transaction);
            return personaIO.GetAllPersonIO();
        }
    }
}
