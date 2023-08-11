using UnityEngine;

namespace Characters.Operations;

public class RandomizedTranslateCollider : CharacterOperation
{
	[SerializeField]
	private Transform _center;

	[SerializeField]
	private Collider2D _targetCollider;

	[Range(0f, 10f)]
	[SerializeField]
	private float _distribution;

	private Vector3 _translate;

	public override void Run(Character owner)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		_translate = _center.position;
		float num = Random.Range(0f - _distribution, _distribution);
		if (Random.value > 0.5f)
		{
			_translate.x += num;
		}
		else
		{
			_translate.x -= num;
		}
		if (Random.value > 0.5f)
		{
			_translate.y += num;
		}
		else
		{
			_translate.y -= num;
		}
		_translate.z = 0f;
		((Component)_targetCollider).transform.position = _translate;
	}
}
