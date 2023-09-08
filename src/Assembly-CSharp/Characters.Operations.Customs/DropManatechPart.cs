using Level;
using UnityEngine;

namespace Characters.Operations.Customs;

public sealed class DropManatechPart : CharacterOperation
{
	[SerializeField]
	[MinMaxSlider(0f, 10f)]
	private Vector2 _countRange;

	[SerializeField]
	private DroppedManatechPart _manatechPart;

	public override void Run(Character owner)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		float num = Random.Range(_countRange.x, _countRange.y);
		for (int i = 0; (float)i < num; i++)
		{
			Vector3 position = ((Component)this).transform.position;
			position.y += 0.5f;
			((Component)_manatechPart.poolObject.Spawn(position, true)).GetComponent<DroppedManatechPart>().cooldownReducingAmount = 1f;
		}
	}
}
