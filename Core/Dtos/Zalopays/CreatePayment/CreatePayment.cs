using Amazon.Runtime.Internal;
using be_project_swp.Core.Base;
using be_project_swp.Core.Dtos.Zalopays.Config;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

namespace be_project_swp.Core.Dtos.Zalopays.CreatePayment
{
    public interface IRequest<T> { };
    public class CreatePayment : IRequest<BaseResultWithData<PaymentLinkDtos>>
    {
        public string PaymentContent { get; set; } = string.Empty;
        public string PaymentCurrency { get; set; } = string.Empty;
        public string PaymentRefId { get; set; } = string.Empty;
        public double RequiredAmount { get; set; }
        public DateTime? PaymentDate { get; set; } = DateTime.Now;
        public DateTime? ExpireDate { get; set; } = DateTime.Now.AddMinutes(15);
        public string? PaymentLanguage { get; set; } = string.Empty;
        public string? MerchantId { get; set; } = string.Empty;
        public string? PaymentDestinationId { get; set; } = string.Empty;
        public string? Signature { get; set; } = string.Empty;
    }

    /*public class CreatePaymentHandler : IRequestHandler<CreatePayment, BaseResultWithData<PaymentLinkDtos>>
    {
        private readonly ZaloPayConfig zaloPayConfig;

        public CreatePaymentHandler(IOptions<ZaloPayConfig> zaloPayConfig)
        {
            this.zaloPayConfig = zaloPayConfig.Value;
        }

        public Task<BaseResultWithData<PaymentLinkDtos>> Handle(CreatePayment request, CancellationToken cancellationToken)
        {
            var result = new BaseResultWithData<PaymentLinkDtos>();
            try
            {

            }
            catch
            {

            }
        }
    }*/
}





