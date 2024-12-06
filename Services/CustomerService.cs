using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Diagnostics;
using TestWebApp.Models;

namespace TestWebApp.Services
{
  public class CustomerService
  {
    public readonly string? _connectionString;
    private readonly List<Customer> _customersData;

    public List<Customer> GetAll() => _customersData;

    public CustomerService()
    {
      // Build configuration
      var configuration = new ConfigurationBuilder()
          .SetBasePath(AppContext.BaseDirectory) // Set the base path for the file
          .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // Load the appsettings.json file
          .Build();

      // Access the connection string
      _connectionString = configuration.GetConnectionString("DefaultConnection");

      _customersData = InitializeCustomers();
    }

    private List<Customer> InitializeCustomers()
    {
      var customers = new List<Customer>();
      DataTable dataTable = new DataTable();

      try
      {
        // Fetch data from the database
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
          connection.Open();
          string query = "SELECT Id, Name FROM Customer";
          using (SqlCommand command = new SqlCommand(query, connection))
          using (SqlDataAdapter adapter = new SqlDataAdapter(command))
          {
            adapter.Fill(dataTable);
          }
        }

        // Convert DataTable to List of Dictionary
        //List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        foreach (DataRow row in dataTable.Rows)
        {
/*
          //Dictionary<string, object> rowData = new Dictionary<string, object>();
          foreach (DataColumn column in dataTable.Columns)
          {
            Debug.WriteLine($"{row[column]}");

            //rowData[column.ColumnName] = row[column];
          }
          //rows.Add(rowData);
*/
          customers.Add(new Customer
          {
            Id = (int)row["Id"],
            Name = row["Name"].ToString()
          });
        }
      }
      catch (Exception)
      {
        // Expensive or one-time initialization logic
        string[] customerNames = { "PROFES", "TITAN", "DIPS", "CUST01" };

        for (int i = 0; i < customerNames.Length; i++)
        {
          customers.Add(new Customer
          {
            Id = i,
            Name = customerNames[i],
          });
          //Debug.WriteLine($"INSERT INTO dbo.Customer (Name) VALUES ('{customerNames[i]}')");
        }
      }

      return customers;
    }
  }
}
