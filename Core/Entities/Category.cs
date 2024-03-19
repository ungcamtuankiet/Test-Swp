using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace be_artwork_sharing_platform.Core.Entities
{
    [Table("categories")]
    public class Category : BaseEntity<long>
    {
        public string Name { get; set; }

        //Relationship
        public ICollection<Artwork> Artworks { get; set; }
    }
}
