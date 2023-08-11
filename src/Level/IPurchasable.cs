using Data;

namespace Level;

public interface IPurchasable
{
	int price { get; set; }

	GameData.Currency.Type priceCurrency { get; set; }
}
