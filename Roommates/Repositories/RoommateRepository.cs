using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Roommates.Models;

namespace Roommates.Repositories
{
    class RoommateRepository : BaseRepository
    {
        public RoommateRepository(string connectionString) : base(connectionString) { }
        public List<Roommate> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, FirstName, LastName FROM Roommate";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Roommate> roommates = new List<Roommate>();

                    while (reader.Read())
                    {
                        int idValue = reader.GetInt32(reader.GetOrdinal("Id"));
                        string roommateFirst = reader.GetString(reader.GetOrdinal("FirstName"));
                        string roommateLast = reader.GetString(reader.GetOrdinal("LastName"));


                        Roommate roommate = new Roommate
                        {
                            Id = idValue,
                            FirstName = roommateFirst,
                            LastName = roommateLast,
                        };

                        roommates.Add(roommate);
                    }

                    reader.Close();

                    return roommates;
                }
            }
        }

        public Roommate GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT rm.Id, rm.FirstName, rm.RentPortion, r.Name as 'Room' 
                                         FROM Roommate rm 
                                         LEFT JOIN Room r on rm.RoomId = r.Id 
                                         WHERE rm.Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    Roommate roommate = null;
                    Room roommateRoom = null;

                    // If we only expect a single row back from the database, we don't need a while loop.
                    if (reader.Read())
                    {
                        roommate = new Roommate
                        {
                            Id = id,
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                            Room = roommateRoom
                        };
                        roommateRoom = new Room
                        {
                            Name = reader.GetString(reader.GetOrdinal("Room"))
                        };
                    }

                    reader.Close();

                    return roommate;
                }
            }
        }









    }
}
