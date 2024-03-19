using be_artwork_sharing_platform.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace be_project_swp.Core.Entities
{
    [Table("payments")]
    public class Payment 
    {
        [Key]
        [Required]
        public string Id { get; set; }
        public string MerchantName { get; set; }
        public string MerchantWebLink { get; set; }
        public string MerchantIpnUrl { get; set; }
        public string MerchantReturnIpnUrl { get; set; }
        public string SecretKey { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdateAt { get; set; }
        public long Artwork_Id { get; set; }
        // RelationShip
        [ForeignKey("InsertUser")]
        public ApplicationUser User { get; set; }

        [ForeignKey("Artwork_Id")]
        public Artwork Artworks { get; set; }
    }
}
