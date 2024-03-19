using be_project_swp.Core.Dtos.RequestOrder;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;

namespace be_artwork_sharing_platform.Core.Entities
{
    [Table("requestorders")]
    public class RequestOrder : BaseEntity<long>
    {
        public string NickName_Sender { get; set; }
        public string NickName_Receivier { get; set; }
        public string UserName_Sender { get; set; }
        public string UserId_Receivier { get; set; }
        public string Text {  get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public StatusRequest StatusRequest { get; set; } = StatusRequest.Waiting;
    }
}
