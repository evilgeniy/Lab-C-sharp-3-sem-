using DB_DataAccess.Extensions;
using DB_DataAccess.Logging;
using DB_Models;
using DB_Models.SearchModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace DB_DataAccess.Repository
{
    public class Person_Repository : IRepository<Person>
    {

        private readonly string _connection;
        private SqlConnection _sqlConnection;
        public Person_Repository(string connection)
        {
            _connection = connection ?? throw new ArgumentOutOfRangeException($"{nameof(connection)} can not be null");
            _sqlConnection = new SqlConnection(_connection);
        }

        public IEnumerable<Person> GetALLInf(SearchCriteria searchCriteria)
        {
            try
            {
                _sqlConnection.Open();

                var command = new SqlCommand(Procedures.GetAllPersons, _sqlConnection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Page", searchCriteria.Page);
                command.Parameters.AddWithValue("@Count", searchCriteria.Count);


                var outputParametr = new SqlParameter("@Total", SqlDbType.Int);
                outputParametr.Direction = ParameterDirection.Output;

                command.Parameters.Add(outputParametr);
                var entities = command.GetData<Person>();
                return entities;
            }
            catch(Exception e)
            {
                ILogger logger = new Log_InDB();
                logger.ErrorToDB(e.Message);
                throw new NotImplementedException();

            }
            finally
            {
                _sqlConnection.Close();
            }

        }

        public Person GetEntity(SearchCriteria searchCriteria)
        {
            try
            {
                _sqlConnection.Open();
                SqlTransaction transaction = _sqlConnection.BeginTransaction();
                var command = new SqlCommand(Procedures.GetPersonByID, _sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Transaction = transaction;

                try
                {
                    command.Parameters.Add(new SqlParameter("@ID", searchCriteria.ID));
                    using (var reader = command.ExecuteReader())
                    {

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var person = new Person();

                                person.ID = searchCriteria.ID;
                                person.FirstName = reader.GetString(1);
                                person.LastName = reader.GetString(2);
                                person.PhoneNumber = reader.GetString(3);
                                person.City = reader.GetString(4);
                                person.Address = reader.GetString(5);

                                return person;
                            }
                        }
                        transaction.Commit();
                    }
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    ILogger logger = new Log_InDB();
                    logger.ErrorToDB(e.Message);
                }
            }
            finally
            {
                _sqlConnection.Close();
            }
            throw new KeyNotFoundException();
            
        }
    }
}