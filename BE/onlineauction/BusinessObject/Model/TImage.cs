using BusinessObject.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Table("TImage")]
public class TImage
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TImageId { get; set; }

    [ForeignKey("AuctionDetails")]
    public int ListAuctionID { get; set; } // Correct foreign key name

    public string Imange { get; set; } = string.Empty;

    // Navigation property
    public virtual AuctionDetail AuctionDetails { get; set; }
}
