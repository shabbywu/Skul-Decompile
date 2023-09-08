using UnityEngine;

namespace Characters.Operations.ObjectTransform;

public class SetScaleToDistance : CharacterOperation
{
	[SerializeField]
	private Transform _object;

	[SerializeField]
	private Transform _target;

	[SerializeField]
	private Transform _transformToApply;

	[SerializeField]
	private float _multiplier = 1f;

	[SerializeField]
	private bool _changeXScale = true;

	public override void Run(Character owner)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		float num = Vector2.Distance(Vector2.op_Implicit(_object.position), Vector2.op_Implicit(_target.position));
		if ((Object)(object)_transformToApply == (Object)null)
		{
			_transformToApply = _object;
		}
		if (_changeXScale)
		{
			_transformToApply.localScale = new Vector3(num * _multiplier, _object.localScale.y);
		}
		else
		{
			_transformToApply.localScale = new Vector3(_object.localScale.x, num * _multiplier);
		}
	}
}
