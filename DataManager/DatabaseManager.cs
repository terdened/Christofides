using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data.SQLite;
using System.Data;

namespace Kristofides.DataManager
{
    class DatabaseManager
    {
        #region const

        private string _connectionString;

        private const string CreateDatabaseCommand = @"
            DROP TABLE IF EXISTS [Link];
            CREATE TABLE [Research] (
                [ID] integer PRIMARY KEY AUTOINCREMENT,
                [GraphID] integer NOT NULL,
                [KristofidesResult] varchar,
                [BrutforceResult] varchar,
                [KristofidesTime] number,
                [BrutforceTime] number
            );

            CREATE TABLE [Graph] (
                [ID] integer PRIMARY KEY AUTOINCREMENT,
                [VertexCount] int NOT NULL,
                [Matrix] varchar NOT NULL
            );";

        private const string InsertCommand = @"
            INSERT INTO Link (GraphID, KristofidesResult, BrutforceResult, KristofidesTime, BrutforceTime)
            VALUES (
            	@GraphID, 
            	@KristofidesResult, 
            	@BrutforceResult, 
            	@KristofidesTime, 
            	@BrutforceTime)";

        #endregion

        public DatabaseManager(string filename)
        {
            _connectionString = string.Format(@"URI=file:{0}", filename);
        }

        public void Create()
        {
            var connection = new SQLiteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = CreateDatabaseCommand;
            command.ExecuteNonQuery();
            command.Dispose();
            connection.Dispose();
        }

        private void InsertResearchRow(ResearchRow row, SQLiteCommand command)
        {
            InsertRow(row.GraphID, row.KristofidesResult, row.BrutforceResult, row.KristofidesTime, row.BrutforceTime, command);
        }

        private void InsertRow(int GraphID, string KristofidesResult, string BrutforceResult, float KristofidesTime,
                                float BrutforceTime, SQLiteCommand command)
        {
            command.Parameters["@GraphID"].Value = GraphID;
            command.Parameters["@KristofidesResult"].Value = KristofidesResult;
            command.Parameters["@BrutforceResult"].Value = BrutforceResult;
            command.Parameters["@KristofidesTime"].Value = KristofidesTime;
            command.Parameters["@BrutforceTime"].Value = BrutforceTime;
            command.ExecuteNonQuery();
        }

        public void StartInserting(out SQLiteConnection connection, out SQLiteCommand command,
                                   out SQLiteTransaction transaction)
        {
            connection = new SQLiteConnection(_connectionString);
            connection.Open();
            command = connection.CreateCommand();
            transaction = connection.BeginTransaction();

            command.CommandText = InsertCommand;

            command.Parameters.AddWithValue("@GraphID", 0);
            command.Parameters.AddWithValue("@KristofidesResult", string.Empty);
            command.Parameters.AddWithValue("@BrutforceResult", string.Empty);
            command.Parameters.AddWithValue("@KristofidesTime", 0);
            command.Parameters.AddWithValue("@BrutforceTime", 0);
        }

        public void FinishInserting(SQLiteConnection connection, SQLiteCommand command, SQLiteTransaction transaction)
        {
            transaction.Commit();
            command.Dispose();
            connection.Dispose();
        }

        private static ResearchRow ReadRow(IDataRecord reader)
        {
            var GraphID = Int32.Parse(reader.GetValue(1) is DBNull ? string.Empty : reader.GetString(1));
            var KristofidesResult = reader.GetValue(2) is DBNull ? string.Empty : reader.GetString(2);
            var BrutforceResult = reader.GetValue(3) is DBNull ? string.Empty : reader.GetString(3);
            var KristofidesTime = float.Parse(reader.GetValue(4) is DBNull ? string.Empty : reader.GetString(4));
            var BrutforceTime = float.Parse(reader.GetValue(5) is DBNull ? string.Empty : reader.GetString(5));

            return new ResearchRow
                {
                    GraphID = GraphID,
                    KristofidesResult = KristofidesResult,
                    BrutforceResult = BrutforceResult,
                    KristofidesTime = KristofidesTime,
                    BrutforceTime = BrutforceTime
                };
        }

        private static void UpdateRow(int GraphID, string KristofidesResult, string BrutforceResult, float KristofidesTime,
                                float BrutforceTime, SQLiteCommand command)
        {
            command.Parameters.AddWithValue("@KristofidesResult", KristofidesResult);
            command.Parameters.AddWithValue("@BrutforceResult", BrutforceResult);
            command.Parameters.AddWithValue("@KristofidesTime", KristofidesTime);
            command.Parameters.AddWithValue("@BrutforceTime", BrutforceTime);

            command.ExecuteNonQuery();
        }

        private void StartUpdating(out SQLiteConnection connection, out SQLiteCommand command,
                                  out SQLiteTransaction transaction)
        {
            connection = new SQLiteConnection(_connectionString);
            connection.Open();
            command = connection.CreateCommand();
            transaction = connection.BeginTransaction();

            command.CommandText = "UPDATE Link SET Role = @Role " +
                                  "WHERE Verb = @Verb and Prep = @Prep and Word = @Word";

            command.Parameters.AddWithValue("@KristofidesResult", string.Empty);
            command.Parameters.AddWithValue("@BrutforceResult", string.Empty);
            command.Parameters.AddWithValue("@KristofidesTime", 0);
            command.Parameters.AddWithValue("@BrutforceTime", 0);
        }

        private static void FinishUpdating(IDisposable connection, IDisposable command, IDbTransaction transaction)
        {
            transaction.Commit();
            command.Dispose();
            connection.Dispose();
        }
    }

    public struct ResearchRow
    {
        public  int GraphID;
        public string KristofidesResult;
        public string BrutforceResult;
        public float KristofidesTime;
        public float BrutforceTime;
    }
}
