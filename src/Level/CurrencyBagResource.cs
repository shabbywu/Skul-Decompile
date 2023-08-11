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
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		return type switch
		{
			GameData.Currency.Type.Gold => ExtensionMethods.Random<CurrencyBag>(_goldBags.Where((CurrencyBag bag) => bag.rarity == rarity)), 
			GameData.Currency.Type.Bone => ExtensionMethods.Random<CurrencyBag>(_boneBags.Where((CurrencyBag bag) => bag.rarity == rarity)), 
			GameData.Currency.Type.DarkQuartz => ExtensionMethods.Random<CurrencyBag>(_quartzBags.Where((CurrencyBag bag) => bag.rarity == rarity)), 
			_ => null, 
		};
	}
}
