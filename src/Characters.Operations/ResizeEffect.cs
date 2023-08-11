using FX;
using UnityEngine;

namespace Characters.Operations;

public class ResizeEffect : CharacterOperation
{
	[SerializeField]
	private Transform _start;

	[SerializeField]
	private Transform _end;

	[SerializeField]
	private EffectInfo _info;

	[SerializeField]
	private float _originSize;

	public override void Run(Character owner)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		_info.scaleX = new CustomFloat(GetScaleX());
		EffectInfo info = _info;
		Vector3 position = _end.position;
		Quaternion rotation = _start.rotation;
		info.Spawn(position, owner, ((Quaternion)(ref rotation)).eulerAngles.z);
	}

	private float GetScaleX()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		return Mathf.Abs((((Component)_end).transform.position.x - ((Component)_start).transform.position.x) / _originSize);
	}
}
