using be_artwork_sharing_platform.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace be_project_swp.Core.Entities
{
    [Table("orders")]
    public class Order : BaseEntity<long>
    {
        public string User_Id { get; set; }
        public string Payment_Id { get; set; }
        public long Artwork_Id { get; set; }

        //Relationship
        [ForeignKey("User_Id")]
        public ApplicationUser User { get; set; }
        [ForeignKey("Payment_Id")]
        public Payment Payment { get; set; }
        [ForeignKey("Artwork_Id")]
        public Artwork Artwork { get; set; }
    }
}
