using System.ComponentModel.DataAnnotations;

namespace MonctonEventsNet.Model;

public class Cost
{
    [Key]
    public int CostId { get; set; }

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Price Min Cost is 0")]
    public decimal MinCost { get; set; }

    public decimal? MaxCost { get; set; }

    public string? Information { get; set; }
    
}