using System;
using UnityEngine;

namespace Level;

[Serializable]
public class CurrencyRangeByRarity
{
	[SerializeField]
	private Vector2Int _commonRange;

	[SerializeField]
	private Vector2Int _rareRange;

	[SerializeField]
	private Vector2Int _uniqueRange;

	[SerializeField]
	private Vector2Int _legendaryRange;

	public int Evaluate(Rarity rarity)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		return rarity switch
		{
			Rarity.Common => Evaluate(_commonRange), 
			Rarity.Rare => Evaluate(_rareRange), 
			Rarity.Unique => Evaluate(_uniqueRange), 
			Rarity.Legendary => Evaluate(_legendaryRange), 
			_ => Evaluate(_commonRange), 
		};
	}

	private int Evaluate(Vector2Int range)
	{
		return Random.Range(((Vector2Int)(ref range)).x, ((Vector2Int)(ref range)).y);
	}
}
