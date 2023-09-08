using System.Linq;
using Data;
using UnityEngine;

namespace Level;

[CreateAssetMenu]
public sealed class CurrencyBagResource : ScriptableObject
{
	private static CurrencyBagResource _instance;

	[SerializeField]
	[Header("Gold")]
	private CurrencyBag[] _goldBags;

	[SerializeField]
	[Header("Bone")]
	private CurrencyBag[] _boneBags;

	[SerializeField]
	[Header("DarkQuartz")]
	private CurrencyBag[] _quartzBags;

	public static CurrencyBagResource instance
	{
		get
		{
			if ((Object)(object)_instance == (Object)null)
			{
				_instance = Resources.Load<CurrencyBagResource>("CurrencyBagResource");
			}
			return _instance;
		}
	}

	public CurrencyBag GetCurrencyBag(GameData.Currency.Type type, Rarity rarity)
	{
		return type switch
		{
			GameData.Currency.Type.Gold => _goldBags.Where((CurrencyBag bag) => bag.rarity == rarity).Random(), 
			GameData.Currency.Type.Bone => _boneBags.Where((CurrencyBag bag) => bag.rarity == rarity).Random(), 
			GameData.Currency.Type.DarkQuartz => _quartzBags.Where((CurrencyBag bag) => bag.rarity == rarity).Random(), 
			_ => null, 
		};
	}
}
