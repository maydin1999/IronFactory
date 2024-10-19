using System;
using System.Data.SqlClient;
using IronFactoryAspNet.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace IronFactoryAspNet.Data
{
    public class UserRepository
    {
        private readonly string connectionString;

        public UserRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public User GetUserByUsername(string username)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Employees WHERE EmployeeUsername = @username", connection);
                command.Parameters.AddWithValue("@username", username);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new User
                        {
                            Id = (int)reader["EmployeeID"],
                            Username = (string)reader["EmployeeUsername"],
                            Password = (string)reader["EmployeePassword"],
                            Salt = (string)reader["salts"]
                        };
                    }
                }
            }
            return null;
        }
    }
}
