using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.SqlClient;
using Roommates.Models;

namespace Roommates.Repositories
{
    public class ChoreRepository : BaseRepository
    {

        /// Constructor for ChoreRepository
        public ChoreRepository(string connectionString) : base(connectionString) { }

        /// Get a list of all the chores
        public List<Chore> GetAll()
        {
            /// Establish the connection (tunnel)
            using (SqlConnection conn = Connection)
            {
                /// must open it ourselves
                conn.Open();

                /// create command for our use (contains the request for Bertha)
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    /// setup the SQL command
                    cmd.CommandText = "SELECT Id, Name FROM Chore";

                    /// execute sql and get reader (send Bertha with instructions and get back the excel papers)
                    SqlDataReader reader = cmd.ExecuteReader();

                    /// declare list of chores to hold the chores from the databse
                    List<Chore> chores = new List<Chore>();

                    /// Keep reading until there is no more data
                    while(reader.Read())
                    {
                        ///Get the numeric position of the given column name and then get the value at that position
                        int idColumnPosition = reader.GetOrdinal("Id");
                        int idValue = reader.GetInt32(idColumnPosition);

                        int nameColumnPosition = reader.GetOrdinal("Name");
                        string nameValue = reader.GetString(nameColumnPosition);

                        /// create the chore object with the data grabbed from the database
                        Chore chore = new Chore
                        {
                            Id = idValue,
                            Name = nameValue
                        };

                        /// add the chore to the list
                        chores.Add(chore);
                    }

                    /// close the reader
                    reader.Close();

                    /// return the list
                    return chores;
                }

                
            }
        }

    }
}
