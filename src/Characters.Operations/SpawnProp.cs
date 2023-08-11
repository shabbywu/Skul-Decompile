using Level;
using UnityEngine;

namespace Characters.Operations;

public sealed class SpawnProp : CharacterOperation
{
	[SerializeField]
	private Prop _prop;

	[SerializeField]
	private Transform _spawnPoint;

	[SerializeField]
	private bool relatedLookingDirection;

	public override void Run(Character owner)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		Prop prop = Object.Instantiate<Prop>(_prop, ((Component)_spawnPoint).transform.position, Quaternion.identity, ((Component)Map.Instance).transform);
		if (relatedLookingDirection && owner.lookingDirection == Character.LookingDirection.Left)
		{
			((Component)prop).transform.localScale = new Vector3(((Component)prop).transform.localScale.x * -1f, ((Component)prop).transform.localScale.y, ((Component)prop).transform.localScale.z);
		}
	}
}
