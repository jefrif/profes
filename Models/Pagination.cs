namespace TestWebApp.Models
{
  public class Pagination
  {
    public int Page { get; set; } = 1;
    public int PageLength { get; set; } = 10;
    public int RowCount { get; set; }
    public int PageCount { get; set; } = 1;
    public int TotalRowCount { get; set; }

  }
}
