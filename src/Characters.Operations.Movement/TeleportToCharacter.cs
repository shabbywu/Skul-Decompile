using System;
using System.Collections.Generic;
using Characters.Movements;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations.Movement;

public class TeleportToCharacter : CharacterOperation
{
	public enum FindingMethod
	{
		Random,
		Closest
	}

	private const int _maxTargets = 32;

	private static readonly NonAllocOverlapper _overlapper;

	private static readonly List<Character> _characters;

	[Header("Find")]
	[SerializeField]
	private TargetLayer _layer = new TargetLayer(LayerMask.op_Implicit(0), allyBody: false, foeBody: true, allyProjectile: false, foeProjectile: false);

	[SerializeField]
	private Collider2D _findingRange;

	[Tooltip("콜라이더 최적화 여부, Composite Collider등 특별한 경우가 아니면 true로 유지")]
	[SerializeField]
	private bool _optimizeFindingRange = true;

	[SerializeField]
	private FindingMethod _findingMethod;

	[SerializeField]
	private bool _onlyGroundedTarget;

	[SerializeField]
	[Header("Position")]
	private float _xOffset = 1f;

	[SerializeField]
	private bool _flipXOffsetByTargetDirection;

	static TeleportToCharacter()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		_overlapper = new NonAllocOverlapper(32);
		_characters = new List<Character>(32);
		((ContactFilter2D)(ref _overlapper.contactFilter)).SetLayerMask(LayerMask.op_Implicit(1024));
	}

	private void Awake()
	{
		if (_optimizeFindingRange)
		{
			((Behaviour)_findingRange).enabled = false;
		}
	}

	public override void Run(Character owner)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		UsingCollider val = default(UsingCollider);
		((UsingCollider)(ref val))._002Ector(_findingRange, _optimizeFindingRange);
		Character character;
		try
		{
			character = FindTargetCharacter((Collider2D)(object)owner.collider, _findingRange, _layer.Evaluate(((Component)owner).gameObject), _findingMethod, _onlyGroundedTarget);
		}
		finally
		{
			((IDisposable)(UsingCollider)(ref val)).Dispose();
		}
		if (!((Object)(object)character == (Object)null))
		{
			bool flag = false;
			if (_flipXOffsetByTargetDirection)
			{
				flag = ((character.movement.config.type != Characters.Movements.Movement.Config.Type.Walking) ? MMMaths.RandomBool() : (character.lookingDirection == Character.LookingDirection.Left));
			}
			Vector2 destination = Vector2.op_Implicit(((Component)character).transform.position);
			Character.LookingDirection lookingDirection;
			if (flag)
			{
				destination.x -= _xOffset;
				lookingDirection = Character.LookingDirection.Right;
			}
			else
			{
				destination.x += _xOffset;
				lookingDirection = Character.LookingDirection.Left;
			}
			if (owner.movement.controller.TeleportUponGround(destination) || owner.movement.controller.Teleport(destination))
			{
				owner.ForceToLookAt(lookingDirection);
				owner.movement.verticalVelocity = 0f;
			}
		}
	}

	private static Character FindTargetCharacter(Collider2D origin, Collider2D range, LayerMask layerMask, FindingMethod findingMethod, bool onlyGroundedTarget)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		((ContactFilter2D)(ref _overlapper.contactFilter)).SetLayerMask(layerMask);
		List<Target> components = _overlapper.OverlapCollider(range).GetComponents<Target>(true);
		if (components.Count == 0)
		{
			return null;
		}
		if (components.Count == 1)
		{
			return components[0].character;
		}
		_characters.Clear();
		foreach (Target item in components)
		{
			if (!((Object)(object)item.character == (Object)null) && (!onlyGroundedTarget || item.character.movement.isGrounded))
			{
				_characters.Add(item.character);
			}
		}
		if (_characters.Count == 0)
		{
			return null;
		}
		if (_characters.Count == 1)
		{
			return _characters[0];
		}
		switch (findingMethod)
		{
		case FindingMethod.Random:
			return ExtensionMethods.Random<Character>((IEnumerable<Character>)_characters);
		case FindingMethod.Closest:
		{
			float num = float.MaxValue;
			Character result = null;
			{
				foreach (Character character in _characters)
				{
					ColliderDistance2D val = Physics2D.Distance(origin, (Collider2D)(object)character.collider);
					float distance = ((ColliderDistance2D)(ref val)).distance;
					if (num > distance)
					{
						result = character;
						num = distance;
					}
				}
				return result;
			}
		}
		default:
			return null;
		}
	}
}
