using System.Collections;
using UnityEngine;

namespace Characters.AI.Behaviours;

public class Repeat : Decorator
{
	[MinMaxSlider(0f, 100f)]
	[SerializeField]
	private Vector2Int _countRange;

	[SerializeField]
	[Subcomponent(true)]
	private Behaviour _behaviour;

	public override IEnumerator CRun(AIController controller)
	{
		base.result = Result.Doing;
		int count = Random.Range(((Vector2Int)(ref _countRange)).x, ((Vector2Int)(ref _countRange)).y + 1);
		for (int i = 0; i < count; i++)
		{
			yield return _behaviour.CRun(controller);
		}
		base.result = Result.Success;
	}
}
