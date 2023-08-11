using BT.SharedValues;
using Characters;
using PhysicsUtils;
using UnityEngine;

namespace BT;

public sealed class CheckRaySight : Node
{
	[SerializeField]
	private CharacterTypeBoolArray _characterTypeFilter;

	[SerializeField]
	private TargetLayer _targetLayer;

	[SerializeField]
	private Collider2D _rayBounds;

	[SerializeField]
	private float _rightRayDistance;

	[SerializeField]
	private float _leftRayDistance;

	[SerializeField]
	[Range(1f, 10f)]
	private int _rayCount;

	private Character _owner;

	private LineSequenceNonAllocCaster _rightLineRaycaster;

	private LineSequenceNonAllocCaster _leftLineRaycaster;

	protected override void OnInitialize()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		//IL_002d: Expected O, but got Unknown
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_005a: Expected O, but got Unknown
		_rightLineRaycaster = new LineSequenceNonAllocCaster(_rayCount, _rayCount)
		{
			caster = (Caster)new RayCaster
			{
				direction = Vector2.right
			}
		};
		_leftLineRaycaster = new LineSequenceNonAllocCaster(_rayCount, _rayCount)
		{
			caster = (Caster)new RayCaster
			{
				direction = Vector2.left
			}
		};
	}

	protected override NodeState UpdateDeltatime(Context context)
	{
		if ((Object)(object)_owner == (Object)null)
		{
			_owner = context.Get<Character>(Key.OwnerCharacter);
		}
		Character character = FindTarget();
		if ((Object)(object)character == (Object)null)
		{
			return NodeState.Fail;
		}
		context.Set(Key.Target, new SharedValue<Character>(character));
		return NodeState.Success;
	}

	private Character FindTarget()
	{
		SetBounds();
		Character character = CheckRight();
		if ((Object)(object)character != (Object)null)
		{
			return character;
		}
		return CheckLeft();
	}

	private void SetBounds()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		Bounds bounds = _rayBounds.bounds;
		_leftLineRaycaster.start = new Vector2(((Bounds)(ref bounds)).min.x, ((Bounds)(ref bounds)).min.y);
		_leftLineRaycaster.end = new Vector2(((Bounds)(ref bounds)).min.x, ((Bounds)(ref bounds)).max.y);
		_rightLineRaycaster.start = new Vector2(((Bounds)(ref bounds)).max.x, ((Bounds)(ref bounds)).min.y);
		_rightLineRaycaster.end = new Vector2(((Bounds)(ref bounds)).max.x, ((Bounds)(ref bounds)).max.y);
	}

	private Character CheckRight()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		LineSequenceNonAllocCaster rightLineRaycaster = _rightLineRaycaster;
		((ContactFilter2D)(ref rightLineRaycaster.caster.contactFilter)).SetLayerMask(_targetLayer.Evaluate(((Component)_owner).gameObject));
		rightLineRaycaster.caster.origin = Vector2.zero;
		rightLineRaycaster.caster.distance = ((_owner.lookingDirection == Character.LookingDirection.Right) ? _rightRayDistance : _leftRayDistance);
		rightLineRaycaster.Cast();
		for (int i = 0; i < rightLineRaycaster.nonAllocCasters.Count; i++)
		{
			ReadonlyBoundedList<RaycastHit2D> results = rightLineRaycaster.nonAllocCasters[i].results;
			if (results.Count != 0)
			{
				RaycastHit2D val = results[0];
				Target component = ((Component)((RaycastHit2D)(ref val)).collider).GetComponent<Target>();
				if (!((Object)(object)component == (Object)null) && !((Object)(object)component.character == (Object)null) && ((EnumArray<Character.Type, bool>)_characterTypeFilter)[component.character.type])
				{
					return component.character;
				}
			}
		}
		return null;
	}

	private Character CheckLeft()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		LineSequenceNonAllocCaster leftLineRaycaster = _leftLineRaycaster;
		((ContactFilter2D)(ref leftLineRaycaster.caster.contactFilter)).SetLayerMask(_targetLayer.Evaluate(((Component)_owner).gameObject));
		leftLineRaycaster.caster.origin = Vector2.zero;
		leftLineRaycaster.caster.distance = ((_owner.lookingDirection == Character.LookingDirection.Right) ? _leftRayDistance : _rightRayDistance);
		leftLineRaycaster.Cast();
		for (int i = 0; i < leftLineRaycaster.nonAllocCasters.Count; i++)
		{
			ReadonlyBoundedList<RaycastHit2D> results = leftLineRaycaster.nonAllocCasters[i].results;
			if (results.Count != 0)
			{
				RaycastHit2D val = results[0];
				Target component = ((Component)((RaycastHit2D)(ref val)).collider).GetComponent<Target>();
				if (!((Object)(object)component == (Object)null) && !((Object)(object)component.character == (Object)null) && ((EnumArray<Character.Type, bool>)_characterTypeFilter)[component.character.type])
				{
					return component.character;
				}
			}
		}
		return null;
	}
}
