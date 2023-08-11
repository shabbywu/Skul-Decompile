using System;
using UnityEngine;

namespace Characters.Operations.SetPosition;

[Serializable]
public class TargetInfo
{
	[SerializeField]
	private CustomFloat _customOffsetX;

	[SerializeField]
	private CustomFloat _customOffsetY;

	[SerializeField]
	[Policy.Subcomponent(true)]
	private Policy _policy;

	public Vector2 GetPosition()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		Vector2 position = _policy.GetPosition();
		return new Vector2(position.x + _customOffsetX.value, position.y + _customOffsetY.value);
	}

	public Vector2 GetPosition(Character owner)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		Vector2 position = _policy.GetPosition(owner);
		return new Vector2(position.x + _customOffsetX.value, position.y + _customOffsetY.value);
	}
}
