using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Diagnostics;
using TestWebApp.Models;
using TestWebApp.Services;

namespace TestWebApp.Pages
{
  public class IndexModel : PageModel
  {
    private readonly ILogger<IndexModel> _logger;
    private readonly SalesOrderService _salesOrderService;
    private readonly Dictionary<string, int> _session = new Dictionary<string, int>();

    [BindProperty]
    public SearchParameter SearchParameter { get; set; } = new SearchParameter();

    [BindProperty(SupportsGet = true)]
    public Pagination Pagination { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public bool ExcelReady { get; set; }

    public IndexModel(ILogger<IndexModel> logger, SalesOrderService service)
    {
      _logger = logger;
      _salesOrderService = service;
      Pagination = new Pagination();
      _session["Page"] = 1;
    }

    public void OnGet(string action)
    {
      int page = Int32.Parse(HttpContext.Session.GetString("page") ?? "1");
      int totalRowCount = Int32.Parse(HttpContext.Session.GetString("totalRowCount") ?? "0");
      int pageCount = totalRowCount / 10 + (totalRowCount % 10 > 0 ? 1 : 0);
      ExcelReady = false;
      Debug.WriteLine($"OnGet: _session={totalRowCount}");

      switch (action)
      {
        case "first":
          Debug.WriteLine("first was clicked.");
          page = 1;
          HttpContext.Session.SetString("page", page.ToString());
          Pagination.Page = page;
          break;

        case "previous":
          Debug.WriteLine($"prev was clicked. {pageCount}");
          page--;
          if (page <= 0)
          {
            break;
          }
          HttpContext.Session.SetString("page", page.ToString());
          Pagination.Page = page;
          break;

        case "next":
          Debug.WriteLine($"next was clicked. {pageCount}");
          page++;
          if (page > pageCount)
          {
            Pagination.Page = page - 1;
            break;
          }
          HttpContext.Session.SetString("page", page.ToString());
          Pagination.Page = page;
          //Page();
          break;

        case "last":
          Debug.WriteLine("last was clicked.");
          HttpContext.Session.SetString("page", pageCount.ToString());
          Pagination.Page = pageCount;
          break;

        default:
          page = 1;
          HttpContext.Session.SetString("page", page.ToString());
          Pagination.Page = _session["Page"];
          break;
      }

      _salesOrderService.LoadPage(Pagination, SearchParameter);
      Debug.WriteLine($"exit. {Pagination.PageCount}, {Pagination.TotalRowCount}");
      HttpContext.Session.SetString("totalRowCount", Pagination.TotalRowCount.ToString());
    }

    public void OnPost(string action)
    {
      ExcelReady = false;

      switch (action)
      {
        case "search":
          Debug.WriteLine($"search was clicked. {SearchParameter.Keyword == null}");
          HttpContext.Session.SetString("page", "1");
          Pagination.Page = 1;
          _salesOrderService.LoadPage(Pagination, SearchParameter);
          HttpContext.Session.SetString("totalRowCount", Pagination.TotalRowCount.ToString());
          break;

        case "excel":
          _filePath = _salesOrderService.SaveListToExcel();
          ExcelReady = true;
          Debug.WriteLine($"excel was clicked. {_filePath}");
          break;

        default:
          break;
      }
    }

    private string _filePath;

    public IActionResult OnGetDownloadFile()
    {
      // Replace with your file path or data source
      //string filePath = "/downloads/export.xlsx";
      ExcelReady = false;

      if (System.IO.File.Exists(_salesOrderService.FilePath))
      {
        // Read the file into a byte array
        var fileBytes = System.IO.File.ReadAllBytes(_salesOrderService.FilePath);

        // Define the file name to download as
        string fileName = "export.xlsx";

        // Return the file as a download
        return File(fileBytes, "application/octet-stream", fileName);
      }
      else
      {
        return NotFound(); // Return 404 if the file is not found
      }
    }

  }
}