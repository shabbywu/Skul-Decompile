using Level;
using UnityEngine;

namespace Characters.Operations.Customs;

public sealed class SpawnPropAtTarget : TargetedCharacterOperation
{
	[SerializeField]
	private Prop _prop;

	public override void Run(Character owner, Character target)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		Object.Instantiate<Prop>(_prop, ((Component)target).transform.position, Quaternion.identity, ((Component)Map.Instance).transform);
	}
}
