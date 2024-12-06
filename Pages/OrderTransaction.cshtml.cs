using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using TestWebApp.Models;
using TestWebApp.Services;

namespace TestWebApp.Pages
{
  public class OrderTransaction : PageModel
  {
    private readonly SalesOrderService _salesOrderService;
    private readonly CustomerService _customerService;

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    [BindProperty(SupportsGet = true)]
    public int Remove { get; set; }

    [BindProperty]
    public SalesOrder SalesOrder { get; set; }

    public string Title { get; set; } = "Add Sales Order";

    public List<Customer> Customers { get; set; }

    [BindProperty]
    public int SelectedCustomerId { get; set; }

    public OrderTransaction(SalesOrderService _salesOrderservice, CustomerService customerService)
    {
      _salesOrderService = _salesOrderservice;
      _customerService = customerService;
      SalesOrder = new SalesOrder();
      Customers = _customerService.GetAll();
    }

    public IActionResult OnPost()
    {
      if (SalesOrder == null)
      {
        return RedirectToPage("./AddOrder");
      }

      if (!ModelState.IsValid)
      {
        return Page();
      }

      SalesOrder.CustomerId = SelectedCustomerId;
      if (Id == 0)
      {
        _salesOrderService.AddData(SalesOrder);
      }
      else if (Id > 0 && Remove == 0)
      {
        SalesOrder.Id = Id;
        _salesOrderService.UpdateData(SalesOrder);
      }
      else
      {
        _salesOrderService.RemoveData(Id);
      }

      return RedirectToPage("./Index");
    }

    public void OnGet()
    {
      Debug.WriteLine($"OrderId={Id}");
      if (Id == 0)
      {
        Title = "Add Sales Order";
      }
      else if (Id > 0 && Remove == 0)
      {
        Title = "Edit Sales Order";
      }
      else
      {
        Title = "Remove Sales Order";
      }
      SalesOrder = _salesOrderService.GetSalesOrderById(Id);
      SelectedCustomerId = SalesOrder.CustomerId;
    }

  }
}
