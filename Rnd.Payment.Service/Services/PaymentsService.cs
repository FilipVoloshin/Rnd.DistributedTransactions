using Grpc;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Rnd.Payment.Service.Data;

namespace Rnd.Payment.Service.Services;

public class PaymentsService : Grpc.PaymentService.PaymentServiceBase
{
    private readonly PaymentsContext _paymentsContext;

    public PaymentsService(PaymentsContext paymentsContext)
    {
        _paymentsContext = paymentsContext;
    }

    public override async Task<ProcessPaymentResponse> ProcessPayment(ProcessPaymentRequest request,
        ServerCallContext context)
    {
        Data.Payment payment = new()
        {
            Id = Guid.NewGuid(),
            Amount = request.Amount,
            OrderId = Guid.Parse(request.OrderId),
            UserId = Guid.Parse(request.UserId),
            Status = PaymentStatus.Processed
        };

        await _paymentsContext.AddAsync(payment);
        await _paymentsContext.SaveChangesAsync();

        return new ProcessPaymentResponse { PaymentId = payment.Id.ToString() };
    }

    public override async Task<RefundPaymentResponse> RefundPayment(RefundPaymentRequest request,
        ServerCallContext context)
    {
        try
        {
            var payment = await _paymentsContext.Payments.FirstOrDefaultAsync(x => x.OrderId == Guid.Parse(request.OrderId));
            if (payment == null)
            {
                return new RefundPaymentResponse { IsSuccess = false };
            }

            payment.Status = PaymentStatus.Refunded;
            _paymentsContext.Payments.Update(payment);
            await _paymentsContext.SaveChangesAsync();
        }
        catch (Exception)
        {
            return new RefundPaymentResponse { IsSuccess = false };
        }

        return new RefundPaymentResponse { IsSuccess = true };
    }
}