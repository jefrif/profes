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
    public string SessionPage { get; private set; }

    [BindProperty]
    public SearchParameter SearchParameter { get; set; } = new SearchParameter();

    [BindProperty]
    public Pagination Pagination { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public bool ExcelReady { get; set; }

    public IndexModel(ILogger<IndexModel> logger, SalesOrderService service)
    {
      _logger = logger;
      _salesOrderService = service;
      Pagination = new Pagination();
      SessionPage = "1";
    }

    public void OnGet(string action)
    {
      SessionPage = HttpContext.Session.GetString("page") ?? "1";
      ExcelReady = false;
      Debug.WriteLine($"OnGet: SessionPage={SessionPage}");

      switch (action)
      {
        case "first":
          Debug.WriteLine("first was clicked.");
          int page = 1;
          HttpContext.Session.SetString("page", page.ToString());
          Pagination.Page = page;
          break;

        case "previous":
          Debug.WriteLine("prev was clicked.");
          page = Int32.Parse(SessionPage) - 1;
          if (page <= 0)
          {
            break;
          }
          HttpContext.Session.SetString("page", page.ToString());
          Pagination.Page = page;
          break;

        case "next":
          Debug.WriteLine("next was clicked.");
          page = Int32.Parse(SessionPage) + 1;
          HttpContext.Session.SetString("page", page.ToString());
          Pagination.Page = page;
          //Page();
          break;

        case "last":
          Debug.WriteLine("last was clicked.");
          break;

        default:
          page = 1;
          HttpContext.Session.SetString("page", page.ToString());
          Pagination.Page = page;
          break;
      }

      _salesOrderService.LoadPage(Pagination, SearchParameter);
    }

    public void OnPost(string action)
    {
      ExcelReady = false;

      switch (action)
      {
        case "search":
          Debug.WriteLine($"search was clicked. {SearchParameter.Keyword == null}");
          _salesOrderService.LoadPage(Pagination, SearchParameter);
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