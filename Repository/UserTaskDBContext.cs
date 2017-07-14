using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using WebApiDEMO.Models;

namespace WebApiDEMO.Repository
{
    public static class UserTaskDBContext
    {
        //code to test opening a MySql connection
        private static string connectionString = ConfigurationManager.ConnectionStrings["MySQLConnection"].ConnectionString;

        public static IEnumerable<UserTask> GetAllUserTasks(int userId)
        {
            List<UserTask> userTasks = null;

            if (!string.IsNullOrEmpty(connectionString))
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        MySqlCommand cmd = new MySqlCommand();
                        cmd.Connection = connection;
                        cmd.CommandText = "select Id, title, completedate, userid, taskcomplete from usertasks;";

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (userTasks == null)
                            {
                                userTasks = new List<UserTask>();
                            }

                            UserTask userTask = new UserTask
                            {
                                Id = reader.GetInt32("Id"),
                                Title = reader.GetString("title"),
                                CompleteDate = reader.GetDateTime("completedate"),
                                UserID = reader.GetInt32("userid"),
                                TaskComplete = reader.GetBoolean("taskcomplete")
                            };
                            userTasks.Add(userTask);
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

            return userTasks;
        }

        public static int Add(UserTask userTask)
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
                        cmd.CommandText = string.Format("insert into usertasks(title, completedate, userid, taskcomplete, insertdate) " +
                                                        "values('{0}', '{1}', '{2}', '{3}', '{4}') ;",
                                                        userTask.Title, userTask.CompleteDate,
                                                        userTask.UserID, userTask.TaskComplete,
                                                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmd.ExecuteNonQuery();
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

        public static int Update(int id, bool isComplete)
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
                        int boolVal = (isComplete) ? 1 : 0;
                        cmd.CommandText = string.Format("update usertasks set taskcomplete = '{0}' " +
                                                        "where Id = {1};",
                                                        boolVal.ToString(),
                                                        id);

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
                        cmd.CommandText = string.Format("delete from usertasks where Id = {0};", Id);

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
    }
}