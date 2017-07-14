using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using WebApiDEMO.Models;

namespace WebApiDEMO.Repository
{
    public static class UserDBContext
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["MySQLConnection"].ConnectionString;
        private static bool isTesting = Convert.ToBoolean(ConfigurationManager.AppSettings["IsTesting"]);
        
        public static IEnumerable<User> GetAllUsers()
        {
            List<User> users = null;

            if (!string.IsNullOrEmpty(connectionString))
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        MySqlCommand cmd = new MySqlCommand();
                        cmd.Connection = connection;
                        cmd.CommandText = "select Id, firstname, lastname, username, password from User;";

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (users == null)
                            {
                                users = new List<User>();
                            }

                            User user = new User{Id = reader.GetInt32("Id"),
                                                 FirstName = reader.GetString("firstname"),
                                                 LastName = reader.GetString("lastname"),
                                                 UserName = reader.GetString("username"),
                                                 UserPassword = reader.GetString("password")};
                            users.Add(user);
                        }

                        if (!reader.IsClosed)
                        {
                            reader.Close();
                        }
                    }
                    catch (MySqlException ex)
                    {
                        string error = ex.Message;
                    }
                    catch (Exception ex)
                    {
                        string error = ex.Message;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            return users;
        }

        public static User GetUser(int Id)
        {
            User user = null;

            if (!string.IsNullOrEmpty(connectionString))
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        MySqlCommand cmd = new MySqlCommand();
                        cmd.Connection = connection;
                        cmd.CommandText = string.Format("select Id, firstname, lastname, username from User where Id = {0};", Id);

                        MySqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            user = new User
                            {
                                Id = reader.GetInt32("Id"),
                                FirstName = reader.GetString("firstname"),
                                LastName = reader.GetString("lastname"),
                                UserName = reader.GetString("username")
                            };
                        }

                        if (!reader.IsClosed)
                        {
                            reader.Close();
                        }
                    }
                    catch (MySqlException ex)
                    {
                        string error = ex.Message;
                    }
                    catch (Exception ex)
                    {
                        string error = ex.Message;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            return user;
        }

        public static int Add(User user)
        {
            int result = 0;

            if (!string.IsNullOrEmpty(connectionString))
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        MySqlCommand cmd = new MySqlCommand();
                        cmd.Connection = connection;
                        cmd.CommandText = string.Format("select Id from User where UserName = '{0}';", user.UserName);

                        MySqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            //Get the Id of the user that already exists
                            result = reader.GetInt32("Id");
                        }
                        else
                        {
                            if (!reader.IsClosed)
                            {
                                reader.Close();
                            }

                            cmd.CommandText = string.Format("insert into User(firstname, lastname, username, password, insertdate) " +
                                                            "values('{0}', '{1}', '{2}', '{3}', '{4}') ;",
                                                            user.FirstName, user.LastName, 
                                                            user.UserName, user.UserPassword, 
                                                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            cmd.ExecuteNonQuery();

                            cmd.CommandText = string.Format("select Id from User where UserName = '{0}';", user.UserName);

                            reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                //Get the Id of the user that was inserted
                                result = reader.GetInt32("Id");
                            }
                        }

                        if (!reader.IsClosed)
                        {
                            reader.Close();
                        }
                    }
                    catch (MySqlException ex)
                    {
                        string error = ex.Message;

                        result = -1;
                    }
                    catch (Exception ex)
                    {
                        string error = ex.Message;

                        result = -1;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            return result;
        }

        public static int Update(User user)
        {
            int rowsAffected = 0;

            if (!string.IsNullOrEmpty(connectionString))
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        MySqlCommand cmd = new MySqlCommand();
                        cmd.Connection = connection;
                        cmd.CommandText = string.Format("update User set firstname = '{0}', " +
                                                        "                lastname = '{1}', " +
                                                        "                username = '{2}'," +
                                                        "                password = '{3}' " +
                                                        "where Id = {4};",
                                                        user.FirstName,
                                                        user.LastName,
                                                        user.UserName,
                                                        user.UserPassword,
                                                        user.Id);

                        rowsAffected = cmd.ExecuteNonQuery();
                    }
                    catch (MySqlException ex)
                    {
                        string error = ex.Message;
                    }
                    catch (Exception ex)
                    {
                        string error = ex.Message;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            return rowsAffected;
        }

        public static int Remove(int Id)
        {
            int rowsAffected = 0;

            if (!string.IsNullOrEmpty(connectionString))
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        MySqlCommand cmd = new MySqlCommand();
                        cmd.Connection = connection;
                        cmd.CommandText = string.Format("delete from User where Id = {0};", Id);

                        rowsAffected = cmd.ExecuteNonQuery();
                    }
                    catch (MySqlException ex)
                    {
                        string error = ex.Message;
                    }
                    catch (Exception ex)
                    {
                        string error = ex.Message;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            return rowsAffected;
        }

        public static bool ValidateLogin(string userName, string password)
        {
            var users = GetAllUsers();
            if (users == null)
            {
                return (isTesting || false);
            }

            return isTesting || users.Any(user => user.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase) && user.UserPassword == password);
        }
    }
}