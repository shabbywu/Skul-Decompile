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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected I4, but got Unknown
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		return (int)rarity switch
		{
			0 => Evaluate(_commonRange), 
			1 => Evaluate(_rareRange), 
			2 => Evaluate(_uniqueRange), 
			3 => Evaluate(_legendaryRange), 
			_ => Evaluate(_commonRange), 
		};
	}

	private int Evaluate(Vector2Int range)
	{
		return Random.Range(((Vector2Int)(ref range)).x, ((Vector2Int)(ref range)).y);
	}
}
