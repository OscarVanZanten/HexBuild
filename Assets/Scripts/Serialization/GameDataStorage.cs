using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using Mono.Data.SqliteClient;

namespace Assets.Scripts.Serialization
{
    public class GameDataStorage
    {
        public static GameDataStorage Instance { get { if (instance == null) { instance = new GameDataStorage(); } return instance; } }
        private static GameDataStorage instance;

        public const string SAVE_LOCATION = "/Saves/";
        public const string SQL_FORMAT = 
            "CREATE TABLE IF NOT EXISTS Plots (" +
            "plot_id INTEGER PRIMARY KEY," +
            "hex_x INTEGER NOT NULL," +
            "hex_y INTEGER NOT NULL," +
            "hex_z INTEGER NOT NULL," +
            "type TINYINT NOT NULL," +
            "height DOUBLE NOT NULL," +
            "temperature DOUBLE NOT NULL" +
            ");";

        public const string SQL_INSERT_PLOT =
            "INSERT INTO Plots (" +
            "hex_x," +
            "hex_y," +
            "hex_z," +
            "type," +
            "height," +
            "temperature" +
            ")" +
            "VALUES (" +
            "HEXX, " +
            "HEXY, " +
            "HEXZ, " +
            "TYPE, " +
            "HEIGHT, " +
            "TEMPERATURE " +
            ");";

        public string Path { get { return Application.persistentDataPath + SAVE_LOCATION; } }
        private GameDataStorage()
        {
            Debug.Log(Path);
            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }
        }

        public void Save(string name, GridData level)
        {
            if (!SaveExists(name))
            {
                CreateSave(name);
            }

            string path = Path + name + ".db";
            string sqlconnsstring = "URI=file:" + path;
            var sql_con = new SqliteConnection(sqlconnsstring);
            sql_con.Open();

            var transaction =  sql_con.BeginTransaction();

            foreach (PlotData plot in level.Plots)
            {
                string insert = SQL_INSERT_PLOT.Replace("HEXX", plot.Location.X + "");
                insert = insert.Replace("HEXY", plot.Location.Y + "");
                insert = insert.Replace("HEXZ", plot.Location.Z + "");
                insert = insert.Replace("TYPE", (int)plot.Type + "");
                insert = insert.Replace("HEIGHT", plot.Height + "");
                insert = insert.Replace("TEMPERATURE", plot.Temperature + "");

                var command = sql_con.CreateCommand();
                command.CommandText = insert;
                command.ExecuteNonQuery();
            }
            transaction.Commit();

            sql_con.Close();
        }

        public void Save(GridData level)
        {
            string name = CreateSave();

            Save(name, level);
        }

        public bool SaveExists(string name)
        {
            string path = Path + name + ".db";

            return File.Exists(path);
        }

        public void CreateSave(string name)
        {
            string path = Path + name + ".db";
            string sqlconnsstring = "URI=file:" + path;
            var sql_con = new SqliteConnection(sqlconnsstring);
            sql_con.Open();

            var command = sql_con.CreateCommand();
            command.CommandText = SQL_FORMAT;
            var result = command.ExecuteReader();

            sql_con.Close();
        }

        public string CreateSave() 
        {
            string name = Guid.NewGuid().ToString();
            CreateSave(name);
            return name;
        }

        public bool DeleteSave(string name)
        {
            if (SaveExists(name))
            {
                return false;
            }
            File.Delete(Path + name + ".db");
            return true;
        }

    }
}
