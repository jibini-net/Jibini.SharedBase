using Jibini.SharedBase.Util.Services;

namespace Jibini.SharedBase.Data;

public class GiftCardRepository
{
    private readonly DatabaseService database;

    public GiftCardRepository(DatabaseService database)
    {
        this.database = database;
    }

    private class GiftCard_AssessCharge_Args
    {
        public int AccountId { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public AssessedGiftCardCharges AssessCharge(int accountId, decimal totalAmount) =>
        database.CallProcForJson<GiftCard_AssessCharge_Args, AssessedGiftCardCharges>("dbo.[GiftCard_AssessCharge]",
                new()
                {
                    AccountId = accountId,
                    TotalAmount = totalAmount
                })
            .Single()!;
}