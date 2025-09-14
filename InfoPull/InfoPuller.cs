using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.Sqlite;
using RestServiceFinal.Models;
using System;
using System.Threading;


namespace RestServiceFinal.InfoPull
{
    public class InfoPuller
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private Thread _thread;
        private bool _running = false;

        public InfoPuller(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public void Start()
        {
            if (_running) return;
            _running = true;
            _thread = new Thread(Run);
            _thread.IsBackground = true;
            _thread.Start();
        }

        public void Stop()
        {
            _running = false;
            _thread?.Join();
        }

        private void Run()
        {
            while (_running)
            {
                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ServiceContext>();

                using (var connection = new SqliteConnection("Data Source=info.db"))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = @"CREATE TABLE IF NOT EXISTS InfoTable (Id INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT NOT NULL, Lname TEXT NOT NULL," +
                        "Email TEXT NOT NULL, Gender INTEGER, Birth TEXT);";// format of Birth YYYY-MM-DD
                    command.ExecuteNonQuery();
                    command.CommandText = "SELECT Id, Name, Lname, Email, Gender, Birth FROM InfoTable";
                    using (var reader = command.ExecuteReader())
                    {
                        db.Users.RemoveRange(db.Users);
                        long _nextId = 1;
                        while (reader.Read())
                        {
                            if (DateTime.TryParse(reader.GetString(5), out var birthDate) && birthDate.Day == DateTime.Today.Day && birthDate.Month == DateTime.Today.Month)
                            {
                                var id = reader.GetInt64(0);
                                var name = reader.GetString(1);
                                var lname = reader.GetString(2);
                                var email = reader.GetString(3);
                                var gender = reader.GetInt32(4);
                                
                                var user = new User
                                {
                                    Id = _nextId++,
                                    Name = name,
                                    Lname = lname,
                                    Email = email,
                                    Gender = gender,
                                };
                                db.Users.Add(user);
                            }
                        }
                    }
                    db.SaveChanges();
                }


                Thread.Sleep(60000); 
            }
        }
    }
}
