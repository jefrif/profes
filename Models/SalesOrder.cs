namespace TestWebApp.Models
{
  public class SalesOrder
  {
    public int Id { get; set; }
    public int No { get; set; }
    public string? OrderNumber{ get; set; }
    public DateTime OrderDate { get; set; }
    public int CustomerId { get; set; }
    public string? CustomerName { get; set; }     // ? mean optional field
  }
}
