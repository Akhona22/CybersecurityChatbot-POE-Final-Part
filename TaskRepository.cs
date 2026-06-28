using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace CybersecurityChatbot
{
    public class TaskRepository
    {
        private DatabaseHelper db;

        public TaskRepository()
        {
            db = new DatabaseHelper();
        }

        // Add Task
        public void AddTask(TaskItem task)
        {
            using (MySqlConnection connection = db.GetConnection())
            {
                connection.Open();

                string query = @"INSERT INTO Tasks
                                (Title, Description, ReminderDate, IsCompleted)
                                VALUES
                                (@title,@description,@reminderDate,@completed)";

                MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@title", task.Title);
                command.Parameters.AddWithValue("@description", task.Description);
                command.Parameters.AddWithValue("@reminderDate",
                    task.ReminderDate.HasValue ? task.ReminderDate.Value : DBNull.Value);

                command.Parameters.AddWithValue("@completed", task.IsCompleted);

                command.ExecuteNonQuery();
            }
        }

        // Read All Tasks
        public List<TaskItem> GetAllTasks()
        {
            List<TaskItem> tasks = new List<TaskItem>();

            using (MySqlConnection connection = db.GetConnection())
            {
                connection.Open();

                string query = "SELECT * FROM Tasks ORDER BY TaskID";

                MySqlCommand command = new MySqlCommand(query, connection);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TaskItem task = new TaskItem();

                        task.TaskID = Convert.ToInt32(reader["TaskID"]);
                        task.Title = reader["Title"].ToString() ?? "";
                        task.Description = reader["Description"].ToString() ?? "";

                        if (reader["ReminderDate"] != DBNull.Value)
                            task.ReminderDate = Convert.ToDateTime(reader["ReminderDate"]);
                        else
                            task.ReminderDate = null;

                        task.IsCompleted = Convert.ToBoolean(reader["IsCompleted"]);

                        tasks.Add(task);
                    }
                }
            }

            return tasks;
        }

        // Complete Task
        public void MarkTaskCompleted(int id)
        {
            using (MySqlConnection connection = db.GetConnection())
            {
                connection.Open();

                string query =
                    "UPDATE Tasks SET IsCompleted = TRUE WHERE TaskID=@id";

                MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@id", id);

                command.ExecuteNonQuery();
            }
        }

        // Delete Task
        public void DeleteTask(int id)
        {
            using (MySqlConnection connection = db.GetConnection())
            {
                connection.Open();

                string query =
                    "DELETE FROM Tasks WHERE TaskID=@id";

                MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@id", id);

                command.ExecuteNonQuery();
            }
        }
    }
}