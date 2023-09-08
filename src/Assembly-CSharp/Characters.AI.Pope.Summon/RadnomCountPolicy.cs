using UnityEngine;

namespace Characters.AI.Pope.Summon;

public sealed class RadnomCountPolicy : CountPolicy
{
	[MinMaxSlider(0f, 100f)]
	[SerializeField]
	private Vector2Int _range;

	public override int GetCount()
	{
		return Random.Range(((Vector2Int)(ref _range)).x, ((Vector2Int)(ref _range)).y + 1);
	}
}
