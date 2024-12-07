using Microsoft.Data.SqlClient;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using TestWebApp.Models;


namespace TestWebApp.Services
{
  public class SalesOrderService
  {
    public readonly string _connectionString;
    private List<SalesOrder> _salesOrders = new List<SalesOrder>();
    private readonly List<SalesOrder> _salesOrdersData;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public string FilePath { get; private set; }

    //private Lazy<List<SalesOrder>> _salesOrders;

    public SalesOrderService(IWebHostEnvironment webHostEnvironment)
    {
      // Build configuration
      var configuration = new ConfigurationBuilder()
          .SetBasePath(AppContext.BaseDirectory) // Set the base path for the file
          .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // Load the appsettings.json file
          .Build();

      // Access the connection string
      _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";

      // Initialize the field only once
      /*      _salesOrders = new Lazy<List<SalesOrder>>(() =>
            {
              _salesOrdersData = InitializeSalesOrders();
              return _salesOrdersData;
            });
      */
      _salesOrders = InitializeSalesOrders();
      _salesOrdersData = _salesOrders;
      Debug.WriteLine($"SalesOrderService: {_salesOrders.Count}");
      _webHostEnvironment = webHostEnvironment;
    }

    public List<SalesOrder> GetAll() => _salesOrders;

    public void Add(SalesOrder salesOrder)
    {
      salesOrder.Id = _salesOrders./*Value.*/Count + 1;
      _salesOrders./*Value.*/Add(salesOrder);
    }
/*
    public void Filter(SearchParameter? searchParam = null)
    {
      if (searchParam == null)
      {
        return;
      }
      Debug.WriteLine($"Filter: {searchParam.Keyword}");

      List<SalesOrder> newList = _salesOrders.*//*Value.*//*Where(o =>
      {
        return o.OrderNumber.Contains(searchParam.Keyword, StringComparison.OrdinalIgnoreCase);
      }).ToList();
      _salesOrders = newList;
    }
*/
    private List<SalesOrder> InitializeSalesOrders()
    {
      // Expensive or one-time initialization logic
      string[] customerNames = { "PROFES", "TITAN", "DIPS", "CUST01" };
      int ci = 0;
      var date = DateTime.Now;

      var salesOrders = new List<SalesOrder>();
      for (int i = 1; i < 37; i++)
      {
        var orderNum = i.ToString().PadLeft(3, '0');
        SalesOrder salesOrder = new SalesOrder
        {
          Id = i,
          No = i,
          OrderNumber = "50_" + orderNum,
          OrderDate = date,
          CustomerName = customerNames[ci],
        };
        salesOrders.Add(salesOrder);
        //Debug.WriteLine($"INSERT INTO dbo.SalesOrder (OrderNumber,OrderDate,CustomerId) "
        //  + $"VALUES ({salesOrder.OrderNumber}, {salesOrder.OrderDate.ToString("yyyy-MM-dd")}, {ci + 1})");

        ci = (ci + 1) % customerNames.Length;
        date = date.AddDays(1);
      }
      return salesOrders;
    }

    public void LoadPage(Pagination pagination, SearchParameter parameter)
    {
      if (_salesOrdersData == null || pagination == null)
      {
        return;
      }

      int skip = (pagination.Page - 1) * pagination.PageLength;
      int i = skip;
      int count = 0;
      var salesOrders = new List<SalesOrder>();
      DataTable dataTable = new DataTable();

      try
      {
        // Fetch data from the database
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
          connection.Open();
          string query = $"SELECT so.Id, OrderNumber, OrderDate, CustomerId, c.Name "
            + $"FROM SalesOrder AS so INNER JOIN Customer c ON so.CustomerId = c.Id ";

          bool included = false;
          if (parameter.OrderDate.HasValue)
          {
            query += $" WHERE OrderDate = '{parameter.OrderDate.Value.Date.ToString("yyyy-MM-dd")}'";
            included = true;
          }
        
          if (!String.IsNullOrEmpty(parameter.Keyword))
          {
            if (included)
            {
              query += $" AND (OrderNumber LIKE '%{parameter.Keyword}%' OR Name LIKE '%{parameter.Keyword}%') ";
            }
            else
            {
              query += $" WHERE (OrderNumber LIKE '%{parameter.Keyword}%' OR Name LIKE '%{parameter.Keyword}%') ";
            }
          }

          query += $" ORDER BY so.Id OFFSET {skip} ROWS FETCH NEXT 10 ROWS ONLY";
          Debug.WriteLine($"quer={query}");

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
          salesOrders.Add(new SalesOrder
          {
            No = ++count,
            Id = (int)row["Id"],
            OrderNumber = (string)row["OrderNumber"],
            OrderDate = (DateTime) row["OrderDate"],
            CustomerId = (int)row["CustomerId"],
            CustomerName = (string)row["Name"]
          });
        }
      }
      catch (Exception)
      {
      }

        /*
              while (count < pagination.PageLength && i < _salesOrdersData.Count)
              {
                var order = _salesOrdersData[i];
                bool included = true;
                if (parameter.OrderDate.HasValue)
                {
                  included = order.OrderDate.Date == parameter.OrderDate.Value.Date;
                }

                if (included && !String.IsNullOrEmpty(parameter.Keyword))
                {
                  included = order.OrderNumber.Contains(parameter.Keyword, StringComparison.OrdinalIgnoreCase);
                }

                if (included)
                {
                  count++;
                  order.No = count;
                  salesOrders.Add(order);
                }
                i++;
              }
        */
        _salesOrders = salesOrders;
      pagination.TotalRowCount = _salesOrdersData.Count;
      pagination.RowCount = count;
    }

    public SalesOrder GetSalesOrderById(int id)
    {
      SalesOrder? order = null;
      
      DataTable dataTable = new DataTable();

      try
      {
        // Fetch data from the database
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
          connection.Open();
          string query = $"SELECT Id, OrderNumber, OrderDate, CustomerId "
            + $"FROM SalesOrder WHERE Id = {id}";
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
          order = new SalesOrder
          {
            No = 0,
            Id = (int)row["Id"],
            OrderNumber = (string)row["OrderNumber"],
            OrderDate = (DateTime)row["OrderDate"],
            CustomerId = (int)row["CustomerId"],
            CustomerName = ""
          };
          break;
        }
      }
      catch (Exception)
      {
      }

      if (order == null)
      {
        return new SalesOrder
        {
          Id = 0,
          OrderDate = DateTime.Now,
          OrderNumber = "",
          CustomerId = 0
        };
      }

      return order;
    }

    public void AddData(SalesOrder salesOrder)
    {
      // Define the query to insert data
      string insertQuery = "INSERT INTO dbo.SalesOrder (OrderNumber,OrderDate,CustomerId) " +
        "VALUES (@OrderNumber, @OrderDate, @CustomerId);";
      Debug.WriteLine($"insertQuery={insertQuery}");

      // Insert data using a connection
      using (SqlConnection connection = new SqlConnection(_connectionString))
      {
        try
        {
          connection.Open();
          using (SqlCommand command = new SqlCommand(insertQuery, connection))
          {
            // Add parameters to prevent SQL injection
            command.Parameters.AddWithValue("@OrderNumber", salesOrder.OrderNumber);
            command.Parameters.AddWithValue("@OrderDate", salesOrder.OrderDate);
            command.Parameters.AddWithValue("@CustomerId", salesOrder.CustomerId);

            // Execute the command
            int rowsAffected = command.ExecuteNonQuery();

            Debug.WriteLine($"{rowsAffected} row(s) inserted.");
          }
        }
        catch (Exception ex)
        {
          Debug.WriteLine($"An error occurred: {ex.Message}");
        }
      }
    }
    
    public void UpdateData(SalesOrder salesOrder)
    {
      // Define the query to update data
      string updateQuery = "UPDATE dbo.SalesOrder SET OrderNumber = @OrderNumber, " +
        "OrderDate = @OrderDate, CustomerId = @CustomerId WHERE Id = @Id;";
      Debug.WriteLine($"updateQuery={salesOrder.Id}");

      // Insert data using a connection
      using (SqlConnection connection = new SqlConnection(_connectionString))
      {
        try
        {
          connection.Open();
          using (SqlCommand command = new SqlCommand(updateQuery, connection))
          {
            // Add parameters to prevent SQL injection
            command.Parameters.AddWithValue("@OrderNumber", salesOrder.OrderNumber);
            command.Parameters.AddWithValue("@OrderDate", salesOrder.OrderDate);
            command.Parameters.AddWithValue("@CustomerId", salesOrder.CustomerId);
            command.Parameters.AddWithValue("@Id", salesOrder.Id);

            // Execute the command
            int rowsAffected = command.ExecuteNonQuery();

            Debug.WriteLine($"{rowsAffected} row(s) updated.");
          }
        }
        catch (Exception ex)
        {
          Debug.WriteLine($"An error occurred: {ex.Message}");
        }
      }
    }
    
    public void RemoveData(int id)
    {
      // Define the query to delete data
      string deleteQuery = "DELETE FROM dbo.SalesOrder WHERE Id = @Id;";
      Debug.WriteLine($"deleteQuery={id}");

      // Insert data using a connection
      using (SqlConnection connection = new SqlConnection(_connectionString))
      {
        try
        {
          connection.Open();
          using (SqlCommand command = new SqlCommand(deleteQuery, connection))
          {
            // Add parameters to prevent SQL injection
            command.Parameters.AddWithValue("@Id", id);

            // Execute the command
            int rowsAffected = command.ExecuteNonQuery();

            Debug.WriteLine($"{rowsAffected} row(s) deleted.");
          }
        }
        catch (Exception ex)
        {
          Debug.WriteLine($"An error occurred: {ex.Message}");
        }
      }
    }

    public string SaveListToExcel/*<T>(List<T> list, string filePath)*/()
    {
      ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

      using (var package = new ExcelPackage())
      {
        // Define the file path in wwwroot
        var wwwrootPath = _webHostEnvironment.WebRootPath;
        var fileName = "export.xlsx";
        var filePath = Path.Combine(wwwrootPath, "downloads", fileName);

        // Ensure the "downloads" folder exists
        string? directory = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directory))
        {
          Directory.CreateDirectory(directory ?? "C:\\Temp");
        }

        // Write content to the file
        //await System.IO.File.WriteAllTextAsync(filePath, "This is a sample file for download.");

        // Add a new worksheet
        var worksheet = package.Workbook.Worksheets.Add("Sheet1");

        // Add headers (property names)
        var properties = typeof(SalesOrder).GetProperties();
        for (int i = 0; i < properties.Length; i++)
        {
          worksheet.Cells[1, i + 1].Value = properties[i].Name;
        }

        // Add data
        for (int row = 0; row < _salesOrders.Count; row++)
        {
          for (int col = 0; col < properties.Length; col++)
          {
            worksheet.Cells[row + 2, col + 1].Value = properties[col].GetValue(_salesOrders[row]);
          }
        }

        // Save the Excel file
        FileInfo fileInfo = new FileInfo(filePath);
        package.SaveAs(fileInfo);
        FilePath = filePath;
        return $"{fileInfo.FullName}";
      }
    }


  }
}
