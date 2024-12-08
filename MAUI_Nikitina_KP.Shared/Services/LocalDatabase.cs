
using MAUI_Nikitina_KP.Shared.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MAUI_Nikitina_KP.Shared.Services
{
    public class LocalDatabase : DbContext
    {
        #region TABLES

        public DbSet<Employer> Employers { get; set; }
        public DbSet<Message> Letters { get; set; }

        #endregion

        #region CONSTRUCTOR

        //parameterless constructor must be above the others,
        //as it seems that EF Tools migrator just takes the .First() of them

        /// <summary>
        /// Constructor for creating migrations
        /// </summary>
        public LocalDatabase()
        {
            File = Path.Combine("../", "LocalDatabase.db3");
            Initialize();
        }

        /// <summary>
        /// Constructor for mobile app
        /// </summary>
        /// <param name="filenameWithPath"></param>
        public LocalDatabase(string filenameWithPath)
        {
            File = filenameWithPath;
            Initialize();
        }

        void Initialize()
        {
            if (!Initialized)
            {
                Initialized = true;

                SQLitePCL.Batteries_V2.Init();

                Database.Migrate();
            }
        }

        public static string File { get; protected set; }
        public static bool Initialized { get; protected set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlite($"Filename={File}");
        }

        #endregion

        public List<Employer> GetEmployers()
        {
            return Employers.ToList();


        }
        public int AuthenticateUser(string login, string password)
        {

            using (var conn = new SqliteConnection($"Filename={Path.Combine("../", "LocalDatabase.db3")}"))
            {
                try
                {
                    conn.Open();

                    // SQL-запрос для проверки существования пользователя с таким логином и паролем
                    string query = "SELECT COUNT(*) FROM Employers WHERE Login = @login AND Password = @password";
                    string query1 = "SELECT COUNT(*) FROM Employers WHERE Login = @login AND Password = @password AND Post = @post";


                    using (var cmd = new SqliteCommand(query, conn))
                    {
                        // Добавляем параметры для защиты от SQL-инъекций
                        cmd.Parameters.AddWithValue("@login", login);
                        cmd.Parameters.AddWithValue("@password", password);

                        // Выполняем запрос и получаем результат
                        int userCount = Convert.ToInt32(cmd.ExecuteScalar());
                        int userCount1 = 0;
                        // Если найден хотя бы один пользователь с такими данными, возвращаем true
                        //return userCount > 0;
                        if (userCount > 0)
                        {
                            using (var cmd1 = new SqliteCommand(query1, conn))
                            {
                                cmd1.Parameters.AddWithValue("@login", login);
                                cmd1.Parameters.AddWithValue("@password", password);
                                cmd1.Parameters.AddWithValue("@post", "Админ");
                                userCount1 = Convert.ToInt32(cmd1.ExecuteScalar());
                                if (userCount1 > 0)
                                {
                                    return 2; // Администратор
                                }
                                else
                                {
                                    return 1; // Обычный пользователь
                                }
                            }


                        }
                        else
                            return 0;



                        //return 1;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка при аутентификации: " + ex.Message);
                    return 0;
                }
                finally
                {
                    conn.Close();
                }
            }
        }
        public List<Employer> FindFilterEmployers(string filterAlf, string filterStat)
        {
            List<Employer> employers = new List<Employer>();
            string orderByClause;

            if (filterAlf == "От А до Я")
            {
                orderByClause = $"{filterStat} ASC"; // По возрастанию
            }
            else
            {
                orderByClause = $"{filterStat} DESC"; // По убыванию
            }

            string query = $"SELECT Name, Surname, SecondName, Post FROM Employers ORDER BY {orderByClause}";

            using (var conn = new SqliteConnection($"Filename={Path.Combine("../", "LocalDatabase.db3")}"))
            {
                try
                {
                    conn.Open();

                    using (var cmd = new SqliteCommand(query, conn))
                    {
                        using (SqliteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var employer = new Employer
                                {
                                    Name = reader.GetString(0),
                                    Surname = reader.GetString(1),
                                    SecondName = reader.GetString(2),
                                    Post = reader.GetString(3)
                                };
                                employers.Add(employer);
                            }
                        }
                    }

                    return employers;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message); // Логируем ошибку
                    return employers;
                }
                finally
                {
                    conn.Close();
                }
            }
        }
        public string FindNameEmployerById(string id)
        {
            using (var conn = new SqliteConnection($"Filename={Path.Combine("../", "LocalDatabase.db3")}"))
            {
                try
                {
                    conn.Open();

                    // SQL-запрос для проверки существования пользователя с таким логином и паролем
                    string query = "SELECT Name FROM Employers WHERE Id = @Id";
                    using (var cmd = new SqliteCommand(query, conn))
                    {
                        // Добавляем параметры для защиты от SQL-инъекций
                        cmd.Parameters.AddWithValue("@Id", id);

                        object result = cmd.ExecuteScalar();

                        if (result == null || result is DBNull)
                        {
                            return String.Empty; // Или другое подходящее значение по умолчанию
                        }

                        return result.ToString();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка при аутентификации: " + ex.Message);
                    return "";
                }
                finally
                {
                    conn.Close();
                }
            }
        }
        public string[] FindNewMessage(string id)
        {

            List<string> messages = new List<string>();
            string query = $"SELECT FromWho, Question, Id FROM Letters WHERE Answer IS NULL AND ToWho = @Id ;";
            using (var conn = new SqliteConnection($"Filename={Path.Combine("../", "LocalDatabase.db3")}"))
            {
                try
                {
                    conn.Open();
                    using (var cmd = new SqliteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        using (SqliteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                StringBuilder userInfo = new StringBuilder();

                                for (int i = 0; i < reader.FieldCount; i++)
                                {

                                    if (i > 0)
                                        userInfo.Append(" : ");

                                    userInfo.Append(reader.GetValue(i));
                                }

                                messages.Add(userInfo.ToString());
                            }
                           
                        }
                    }

                    return messages.ToArray();
                }
                catch (Exception ex)
                {
                    //Console.WriteLine(ex.Message); // Логируем ошибку
                    return messages.ToArray();
                }
                finally
                {
                    conn.Close();
                }
            }
        }
        public int GetUserID (string login, string password)
        {

            using (var conn = new SqliteConnection($"Filename={Path.Combine("../", "LocalDatabase.db3")}"))
            {
                try
                {
                    conn.Open();

                    // SQL-запрос для проверки существования пользователя с таким логином и паролем
                    string query = "SELECT Id FROM Employers WHERE Login = @login AND Password = @password";
                    using (var cmd = new SqliteCommand(query, conn))
                    {
                        // Добавляем параметры для защиты от SQL-инъекций
                        cmd.Parameters.AddWithValue("@login", login);
                        cmd.Parameters.AddWithValue("@password", password);

                        // Выполняем запрос и получаем результат
                        int userCount = Convert.ToInt32(cmd.ExecuteScalar());
                       
                        // Если найден хотя бы один пользователь с такими данными, возвращаем true
                        //return userCount > 0;
                        if (userCount > 0)
                        {
                            return userCount;


                        }
                        else
                            return 0;



                        //return 1;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка при аутентификации: " + ex.Message);
                    return 0;
                }
                finally
                {
                    conn.Close();
                }
            }
        }
        public void RefactorEmployer(int id, string name,  string surname, string secondname, string education, string post, string history, string age)
        {
            using (var conn = new SqliteConnection($"Filename={Path.Combine("../", "LocalDatabase.db3")}"))
            {
                conn.Open();
                string sql = ($"UPDATE Employers SET Name = @Name, Surname = @Surname, SecondName = @SecondName, Education = @Education, " +
                    $"Age = @Age, History = @History, Post = @Post" +
                    $" WHERE Id = @Id");

                using (SqliteCommand cmd = new SqliteCommand(sql, conn))
                {
                    // Установка значений параметров
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Surname", surname);
                    cmd.Parameters.AddWithValue("@SecondName", secondname);
                    cmd.Parameters.AddWithValue("@Education", education);
                    cmd.Parameters.AddWithValue("@Age", age);
                    cmd.Parameters.AddWithValue("@History", history);
                    cmd.Parameters.AddWithValue("@Post", post);

                    // Выполнение команды
                    cmd.ExecuteNonQuery();
                    
                }
            }
        }
        public Employer FindEmployer_Information(int id)
        {
            using (var connection = new SqliteConnection($"Filename={Path.Combine("../", "LocalDatabase.db3")}"))
            {
                connection.Open();

                var selectCommand = new SqliteCommand
                {
                    Connection = connection,
                    CommandText = "SELECT * FROM Employers WHERE Id = @id"
                };
                selectCommand.Parameters.AddWithValue("@id", id);

                using (var reader = selectCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Employer
                        {
                            Id = reader.GetInt32(0),
                            Age = reader.GetString(1),
                            Education = reader.GetString(2),
                            History = reader.GetString(3),
                            Login = reader.GetString(4),
                            Name = reader.GetString(5),
                            Password = reader.GetString(6),
                            Post = reader.GetString(7),
                            SecondName = reader.GetString(8),
                            Surname = reader.GetString(9)
                        };
                    }
                }
            }
            return null; // Если не найден, возвращаем null
        }
        public string[] GetUsersIDFromSQLite()
        {
            List<string> users = new List<string>();

            using (var connection = new SqliteConnection($"Filename={Path.Combine("../", "LocalDatabase.db3")}"))
            {
                connection.Open();
                string c = "SELECT Id FROM Employers;";
                using (var command = new SqliteCommand(c, connection))
                {


                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            StringBuilder userInfo = new StringBuilder();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {

                                if (i > 0)
                                    userInfo.Append(", ");

                                userInfo.Append(reader.GetValue(i));
                            }

                            users.Add(userInfo.ToString());
                        }
                    }
                }
            }

            return users.ToArray();
        }
        public string[] GetUsersFromSQLite()
        {
            List<string> users = new List<string>();

            using (var connection = new SqliteConnection($"Filename={Path.Combine("../", "LocalDatabase.db3")}"))
            {
                connection.Open();
                string c = "SELECT Name, Surname, SecondName, Age, Post, History, Education, Id FROM Employers;";
                using (var command = new SqliteCommand(c,connection))
                {
                    

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            StringBuilder userInfo = new StringBuilder();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {

                                if (i > 0)
                                    userInfo.Append(", ");

                                userInfo.Append(reader.GetValue(i));
                            }

                            users.Add(userInfo.ToString());
                        }
                    }
                }
            }

            return users.ToArray();
        }
        public string[] GetUsersFromSQLiteWithoutOneUser(int id)
        {
            List<string> users = new List<string>();

            using (var connection = new SqliteConnection($"Filename={Path.Combine("../", "LocalDatabase.db3")}"))
            {
                connection.Open();
                string c = "SELECT Name, Surname, SecondName, Age, Post, History, Education, Id FROM Employers WHERE Id != @excludedId;";
                using (var command = new SqliteCommand(c, connection))
                {
                    command.Parameters.AddWithValue("@excludedId", id);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            StringBuilder userInfo = new StringBuilder();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {

                                if (i > 0)
                                    userInfo.Append(", ");

                                userInfo.Append(reader.GetValue(i));
                            }

                            users.Add(userInfo.ToString());
                        }
                    }
                }
            }

            return users.ToArray();
        }
        public void MessageText()
        {
            using (var conn = new SqliteConnection($"Filename={Path.Combine("../", "LocalDatabase.db3")}"))
            {
                string text = "guishigshg";
                try
                {
                    conn.Open();

                    // SQL-запрос для проверки существования пользователя с таким логином и паролем
                    string query = "INSERT INTO Letters (Question) VALUES (@text);";


                    using (var cmd = new SqliteCommand(query, conn))
                    {
                        // Добавляем параметры для защиты от SQL-инъекций
                        cmd.Parameters.AddWithValue("@text", text);
                        cmd.ExecuteScalar();
                        
                    }
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("Ошибка при аутентификации: " + ex.Message);
                   
                }
                finally
                {
                    conn.Close();
                }
            }
        }
        public void SendMessageText(string NameUserFrom, string NameUserTo, string text)
        {
            try
            {
                using (var conn = new SqliteConnection($"Filename={Path.Combine("../", "LocalDatabase.db3")}"))
                {
                    conn.Open();
                    string query = "INSERT INTO Letters (Question, FromWho, ToWho) VALUES (@text, @IdFrom, @NameUserTo);";

                    using (var cmd = new SqliteCommand(query, conn))
                    {
                        // Добавляем параметры для защиты от SQL-инъекций
                        cmd.Parameters.AddWithValue("@text", text);
                        cmd.Parameters.AddWithValue("@IdFrom", NameUserFrom);
                        cmd.Parameters.AddWithValue("@NameUserTo", NameUserTo);

                        // Используем ExecuteNonQuery для выполнения команды INSERT
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Выводим сообщение об ошибке для отладки
                Console.WriteLine("Ошибка при вставке данных: " + ex.Message);
            }
        }
        public void SendAnswerMessageText(string id, string text)
        {
            try
            {
                using (var conn = new SqliteConnection($"Filename={Path.Combine("../", "LocalDatabase.db3")}"))
                {
                    conn.Open();
                    string query = "UPDATE Letters SET Answer = @text WHERE Id = @Id";

                    using (var cmd = new SqliteCommand(query, conn))
                    {
                        // Добавляем параметры для защиты от SQL-инъекций
                        cmd.Parameters.AddWithValue("@text", text);
                        cmd.Parameters.AddWithValue("@Id", id);

                        // Используем ExecuteNonQuery для выполнения команды INSERT
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Выводим сообщение об ошибке для отладки
                Console.WriteLine("Ошибка при вставке данных: " + ex.Message);
            }
        }


        public Employer FindEmployerID (int id)
        {
            using (var connection = new SqliteConnection($"Filename={Path.Combine("../", "LocalDatabase.db3")}"))
            {
                connection.Open();

                var selectCommand = new SqliteCommand
                {
                    Connection = connection,
                    CommandText = "SELECT * FROM Employers WHERE Id = @Id"
                };
                selectCommand.Parameters.AddWithValue("@Id", id);

                using (var reader = selectCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Employer
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(5),
                            Surname = reader.GetString(9),
                            SecondName = reader.GetString(8),
                            Post = reader.GetString(7),
                            Education = reader.GetString(2),
                            History = reader.GetString(3),
                            Login = reader.GetString(4),
                            Password = reader.GetString(6),
                            Age = reader.GetString(1)
                        };
                    }
                }
            }
            return null; // Если не найден, возвращаем null
        }
        public Employer FindEmployer(string name)
        {
            using (var connection = new SqliteConnection($"Filename={Path.Combine("../", "LocalDatabase.db3")}"))
            {
                connection.Open();

                var selectCommand = new SqliteCommand
                {
                    Connection = connection,
                    CommandText = "SELECT * FROM Employers WHERE Name = @name"
                };
                selectCommand.Parameters.AddWithValue("@name", name);

                using (var reader = selectCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Employer
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Surname = reader.GetString(2),
                            SecondName = reader.GetString(3),
                            Post = reader.GetString(4),
                            Education = reader.GetString(5),
                            History = reader.GetString(6),
                            Login = reader.GetString(7),
                            Password = reader.GetString(8),
                            Age = reader.GetString(9)
                        };
                    }
                }
            }
            return null; // Если не найден, возвращаем null
        }
        public bool AddEmployer(string name, string surname, string secondname, string age, string post, string education, string history, string password, string login)//добавление пользователя
        {
            Employer employer = new Employer();
            employer.Name = name;
            employer.Surname = surname;
            employer.SecondName = secondname;
            employer.Post = post;
            employer.Education = education;
            employer.History = history;
            employer.Login = login;
            employer.Password = password;
            employer.Age = age;
            Employers.Add(employer);
            this.SaveChanges();
            return true;
        }
        public bool DeleteEmployer(int id) //Удаление записи
        {
            using (var connection = new SqliteConnection($"Filename={Path.Combine("../", "LocalDatabase.db3")}"))
            {
                connection.Open();

                // Выполняем команду удаления
                var deleteCommand = new SqliteCommand
                {
                    Connection = connection,
                    CommandText = "DELETE FROM Employers WHERE Id = @Id"
                };
                deleteCommand.Parameters.AddWithValue("@Id", id);

                // Проверяем количество удаленных строк
                int affectedRows = deleteCommand.ExecuteNonQuery();

                // Если строка была удалена, возвращаем true
                return affectedRows > 0;
            }
        }

        public void Reload()
        {
            Database.CloseConnection();
            Database.OpenConnection();
        }

    }

}
