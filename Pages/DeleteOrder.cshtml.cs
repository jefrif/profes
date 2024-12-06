using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestWebApp.Services;

namespace TestWebApp.Pages
{
  public class DeleteOrderModel : PageModel
  {
    private readonly SalesOrderService _salesOrderService;

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    public DeleteOrderModel(SalesOrderService salesOrderService)
    {
      _salesOrderService = salesOrderService;
    }

    public IActionResult OnPost()
    {
      _salesOrderService.RemoveData(Id);

      return RedirectToPage("./Index");
  }
}
}
