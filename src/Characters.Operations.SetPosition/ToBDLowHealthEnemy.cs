using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations.SetPosition;

public class ToBDLowHealthEnemy : Policy
{
	[SerializeField]
	private BehaviorTree _tree;

	[SerializeField]
	private string _ownerValueName = "Owner";

	[SerializeField]
	private Collider2D _findRange;

	[SerializeField]
	private bool _includeSelf = true;

	[SerializeField]
	private bool _optimizedCollider = true;

	private Character _ownerValue;

	public override Vector2 GetPosition(Character owner)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return GetPosition();
	}

	public override Vector2 GetPosition()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		Character character = GetLowHealthCharacter();
		if ((Object)(object)character == (Object)null)
		{
			character = _ownerValue;
		}
		return Vector2.op_Implicit(((Component)character).transform.position);
	}

	private Character GetLowHealthCharacter()
	{
		SharedCharacter sharedCharacter = ((Behavior)_tree).GetVariable(_ownerValueName) as SharedCharacter;
		_ownerValue = ((SharedVariable<Character>)sharedCharacter).Value;
		List<Character> list = FindEnemiesInRange(_findRange);
		double num = 1.0;
		Character result = null;
		foreach (Character item in list)
		{
			if (item.liveAndActive && (_includeSelf || !((Object)(object)item == (Object)(object)_ownerValue)) && item.health.percent < num)
			{
				num = item.health.percent;
				result = item;
			}
		}
		return result;
	}

	public List<Character> FindEnemiesInRange(Collider2D collider)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		((Behaviour)collider).enabled = true;
		NonAllocOverlapper val = new NonAllocOverlapper(31);
		((ContactFilter2D)(ref val.contactFilter)).SetLayerMask(LayerMask.op_Implicit(1024));
		List<Character> components = val.OverlapCollider(collider).GetComponents<Character>(true);
		if (_optimizedCollider)
		{
			((Behaviour)collider).enabled = false;
		}
		return components;
	}
}
