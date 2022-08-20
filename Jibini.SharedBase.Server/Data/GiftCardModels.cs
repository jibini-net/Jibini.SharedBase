namespace Jibini.SharedBase.Data;

public class GiftCard
{
    public int Id { get; set; }
    public string RedemptionCode { get; set; } = "";
    public decimal InitialAmount { get; set; }
    public DateTime? Expiration { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Redeemed { get; set; }
    public int? RedeemedBy { get; set; }
    public decimal Balance { get; set; }
    public DateTime? LastUsed { get; set; }
    public bool Valid { get; set; }
    public int SortOrder { get; set; }
}

public class GiftCardCharge
{
    public decimal ApplyAmount { get; set; }
    public GiftCard Card { get; set; } = new();
}

public class AssessedGiftCardCharges
{
    public decimal RemainingCharges { get; set; }
    public List<GiftCardCharge> ApplyCards { get; set; } = new();
}