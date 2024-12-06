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
      switch (action)
      {
        case "search":
          Debug.WriteLine($"search was clicked. {SearchParameter.Keyword == null}");
          _salesOrderService.LoadPage(Pagination, SearchParameter);

          //if (String.IsNullOrEmpty(SearchParameter.Keyword))
          //{
          //  _salesOrderService.Refresh();

          //}
          //else
          //{
          //  _salesOrderService.Filter(SearchParameter);
          //}
          break;

        default:
          break;
      }
    }
  }
}