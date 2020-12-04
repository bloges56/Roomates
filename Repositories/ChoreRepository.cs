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

        /// return a single chore by the given id
        public Chore GetById(int id)
        {
            /// create the connection
            using (SqlConnection conn = Connection)
            {
                //open the connection
                conn.Open();

                //create a cmd from the connection
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    ///give command sql request along with given id
                    cmd.CommandText = "SELECT Name FROM Chore WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    /// execute the sql
                    SqlDataReader reader = cmd.ExecuteReader();

                    /// declare and initialize a chore to null
                    Chore chore = null;

                    /// if there is data, read it and and assign the data to the chore object
                    if(reader.Read())
                    {
                        chore = new Chore
                        {
                            Id = id,
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };
                    }

                    //close the reader
                    reader.Close();

                    //return the chore
                    return chore;
                }
            }
        }

        //add a new chore to the database
        public void Insert(Chore chore)
        {
            //establish connection
            using (SqlConnection conn = Connection)
            {
                //open conneciton
                conn.Open();

                //use command
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    /// insertion sql command
                    cmd.CommandText = @"INSERT INTO Chore (Name)
                                        OUTPUT INSERTED.Id
                                        VALUES (@name)";
                    cmd.Parameters.AddWithValue("@name", chore.Name);

                    //execute command and get its id
                    int id = (int)cmd.ExecuteScalar();

                    chore.Id = id;

                }
            }
        }

        public List<Chore> GetUnassignedChores()
        {
            ///establish a connection
            using (SqlConnection conn = Connection)
            {
                /// open the connect
                conn.Open();

                /// use command
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    /// create sql command
                    cmd.CommandText = @"SELECT c.Id, c.Name
                                        FROM Chore c
                                        LEFT JOIN RoommateChore rc on rc.ChoreId = c.Id
                                        WHERE rc.ChoreId IS NULL";

                    ///execute cmd and return reader
                    SqlDataReader reader = cmd.ExecuteReader();


                    ///declare an empty list to store the Chore objects
                    List<Chore> chores = new List<Chore>() { };

                    /// read all the data in reader
                    while(reader.Read())
                    {
                        int id = reader.GetInt32(reader.GetOrdinal("Id"));
                        string name = reader.GetString(reader.GetOrdinal("Name"));

                        ///create a chore object with the data
                        Chore chore = new Chore()
                        {
                            Id = id,
                            Name = name
                        };

                        ///add the chore to the list
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
