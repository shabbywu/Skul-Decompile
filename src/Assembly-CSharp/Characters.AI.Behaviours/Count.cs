using System.Collections;
using UnityEngine;

namespace Characters.AI.Behaviours;

public class Count : Decorator
{
	[MinMaxSlider(0f, 100f)]
	[SerializeField]
	private Vector2Int _range;

	[SerializeField]
	[Subcomponent(true)]
	private Behaviour _behaviour;

	private int _max;

	private int _current;

	private void OnEnable()
	{
		_current = 0;
		_max = Random.Range(((Vector2Int)(ref _range)).x, ((Vector2Int)(ref _range)).y + 1);
	}

	public override IEnumerator CRun(AIController controller)
	{
		base.result = Result.Doing;
		if (_current >= _max)
		{
			base.result = Result.Fail;
			yield break;
		}
		_current++;
		yield return _behaviour.CRun(controller);
		base.result = Result.Success;
	}
}
