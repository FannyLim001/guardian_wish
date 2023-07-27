using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data.SQLite;
using System;

public class DbConnection
{
    private string connectionString;
    private SQLiteConnection connection;

    public DbConnection()
    {
        connectionString = "Data Source=Assets/Database/guardian_wish.db;Version=3;";
    }

    public SQLiteConnection GetConnection()
    {
       SQLiteConnection con = new SQLiteConnection(connectionString);
       con.Open();
       return con;
    }

    public void CloseConnection()
    {
        // Create a connection to the database.
        connection = GetConnection();

        // Connect to the database.
        connection.Close();
    }
}
