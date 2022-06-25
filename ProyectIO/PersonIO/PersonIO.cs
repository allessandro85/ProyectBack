using Dapper;
using ProyectModels;
using System.Data;

namespace ProyectIO.PersonIO
{
    public class PersonIO : IO
    {
		public PersonIO(IDbConnection connection, IDbTransaction transaction) : base(connection, transaction) { }
		public Persona GetPersonIO()
        {
			string sql = @"
SELECT ID,Nombre,Apellido,Provincia,Dni,Telefono,Activo,Email,Profiles,Skills
FROM [dbo].[Persona]
Where Activo = 1
";

            return _connection.Query<Persona>(sql, transaction: _transaction).First();
		}

        public Task<int> DeleteLogicalPersonIO(int id)
        {
            string sql = @"
UPDATE Persona
set Activo = 0
Where Persona.ID = @ID
";

            return _connection.ExecuteAsync(sql, param: new { ID = id}, transaction: _transaction);            
        }

        public Task<int> ResetAllActivePersonIO()
        {
            string sql = @"
UPDATE Persona
set Activo = 1
";

            return _connection.ExecuteAsync(sql, transaction: _transaction);
        }


        public List<Persona> GetAllPersonIO()
        {
            string sql = @"
SELECT ID,Nombre,Apellido,Provincia,Dni,Telefono,Activo,Email,Profiles,Skills
FROM [dbo].[Persona]";

            return _connection.Query<Persona>(sql, transaction: _transaction).ToList();
        }
    }
}
