
using System.ComponentModel.DataAnnotations;

namespace ThreadLine.Models;

public class Sale
{
    public int Id { get; set; }

    public DateTime SaleDate { get; set; } = DateTime.Now;

    [Range(typeof(decimal), "0", "999999999")]
    public decimal TotalAmount { get; set; }

    public ICollection<SaleItem> Items { get; set; }
        = new List<SaleItem>();
}