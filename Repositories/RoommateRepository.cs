using System;
using System.Collections.Generic;
using System.Text;
using Roommates.Models;
using Microsoft.Data.SqlClient;

namespace Roommates.Repositories
{
    public class RoommateRepository : BaseRepository
    {
        /// constructor
        public RoommateRepository(string connectionString) :  base(connectionString) { }

        /// get a roommate by a given id
        public Roommate GetById(int id)
        {
            /// create a connection
            using(SqlConnection conn = Connection)
            {
                /// open the connection
                conn.Open();

                /// use the command
                using(SqlCommand cmd = conn.CreateCommand())
                {
                    /// create the sql command
                    cmd.CommandText = @"SELECT m.Id AS RoommateId, m.FirstName, m.LastName, m.RentPortion, m.MoveInDate, r.Id AS RoomId, r.Name, r.MaxOccupancy
                                        FROM Roommate m
                                        LEFT JOIN Room r on m.RoomId = r.Id
                                        WHERE m.Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    Roommate roommate = null;

                    /// read the roommate
                    SqlDataReader reader = cmd.ExecuteReader();
                    if(reader.Read())
                    {
                        /// store the data in local variables
                        int roommateId = reader.GetInt32(reader.GetOrdinal("RoommateId"));
                        string firstName = reader.GetString(reader.GetOrdinal("FirstName"));
                        string lastName = reader.GetString(reader.GetOrdinal("LastName"));
                        int rentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion"));
                        DateTime moveInDate = reader.GetDateTime(reader.GetOrdinal("MoveInDate"));
                        int roomId = reader.GetInt32(reader.GetOrdinal("RoomId"));
                        string roomName = reader.GetString(reader.GetOrdinal("Name"));
                        int maxOccupancy = reader.GetInt32(reader.GetOrdinal("MaxOccupancy"));

                        //create a room object from the data
                        Room room = new Room()
                        {
                            Id = roomId,
                            Name = roomName,
                            MaxOccupancy = maxOccupancy
                        };

                        //create a roommate object with the data and newly created room
                        roommate = new Roommate()
                        {
                            Id = roommateId,
                            Firstname = firstName,
                            Lastname = lastName,
                            RentPortion = rentPortion,
                            MovedInDate = moveInDate,
                            Room = room
                        };

                    }

                    //close the reader
                    reader.Close();

                    //return the roommate
                    return roommate;
                }
            }
        }
    }
}
