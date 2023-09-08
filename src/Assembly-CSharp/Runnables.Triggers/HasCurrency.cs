using Data;
using UnityEngine;

namespace Runnables.Triggers;

public class HasCurrency : Trigger
{
	[SerializeField]
	private GameData.Currency.Type _type;

	[CurrencyAmount.Subcomponent]
	[SerializeField]
	private CurrencyAmount _currencyAmount;

	protected override bool Check()
	{
		int amount = _currencyAmount.GetAmount();
		return _type switch
		{
			GameData.Currency.Type.Gold => GameData.Currency.gold.Has(amount), 
			GameData.Currency.Type.Bone => GameData.Currency.bone.Has(amount), 
			GameData.Currency.Type.DarkQuartz => GameData.Currency.darkQuartz.Has(amount), 
			_ => GameData.Currency.gold.Has(amount), 
		};
	}
}
