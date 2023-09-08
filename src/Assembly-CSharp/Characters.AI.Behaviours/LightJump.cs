using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours.Attacks;
using Characters.Actions;
using Characters.Operations;
using PhysicsUtils;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours;

public sealed class LightJump : Behaviour
{
	[UnityEditor.Subcomponent(typeof(Teleport))]
	[SerializeField]
	private Teleport _teleport;

	[UnityEditor.Subcomponent(typeof(ActionAttack))]
	[SerializeField]
	private ActionAttack _fall;

	[UnityEditor.Subcomponent(typeof(ActionAttack))]
	[SerializeField]
	private ActionAttack _attack;

	[SerializeField]
	[UnityEditor.Subcomponent(typeof(EscapeTeleport))]
	private EscapeTeleport _escapeTeleport;

	[UnityEditor.Subcomponent(typeof(ShiftObject))]
	[SerializeField]
	private ShiftObject _shiftObject;

	[SerializeField]
	private Transform _destination;

	[UnityEditor.Subcomponent(typeof(Hide))]
	[SerializeField]
	private Hide _hide;

	[SerializeField]
	private Action _teleportStart;

	[SerializeField]
	private Action _teleportEnd;

	private void Awake()
	{
		_childs = new List<Behaviour> { _teleport, _fall, _attack, _escapeTeleport };
	}

	public override IEnumerator CRun(AIController controller)
	{
		base.result = Result.Doing;
		yield return _hide.CRun(controller);
		if (!controller.character.movement.controller.Teleport(Vector2.op_Implicit(_destination.position)) || !_teleportEnd.TryStart())
		{
			yield break;
		}
		while (_teleportEnd.running && base.result == Result.Doing)
		{
			yield return null;
		}
		if (base.result != Result.Doing)
		{
			yield break;
		}
		((MonoBehaviour)this).StartCoroutine(_fall.CRun(controller));
		while (_fall.result == Result.Doing)
		{
			if (controller.character.movement.isGrounded)
			{
				controller.character.CancelAction();
				_fall.StopPropagation();
				yield break;
			}
			yield return null;
		}
		if (base.result == Result.Doing)
		{
			yield return _attack.CRun(controller);
			if (base.result == Result.Doing)
			{
				yield return _escapeTeleport.CRun(controller);
				base.result = Result.Done;
			}
		}
	}

	public bool CanUse(Character character)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		if (!_fall.CanUse() || !_attack.CanUse())
		{
			return false;
		}
		if (!character.movement.isGrounded)
		{
			return false;
		}
		Bounds bounds = ((Collider2D)character.collider).bounds;
		((Bounds)(ref bounds)).center = Vector2.op_Implicit(new Vector2(_destination.position.x, _destination.position.y + (((Bounds)(ref bounds)).center.y - ((Bounds)(ref bounds)).min.y)));
		((ContactFilter2D)(ref NonAllocOverlapper.shared.contactFilter)).SetLayerMask(LayerMask.op_Implicit(LayerMask.op_Implicit(Layers.terrainMask) | 0x11));
		if (NonAllocOverlapper.shared.OverlapBox(Vector2.op_Implicit(((Bounds)(ref bounds)).center), Vector2.op_Implicit(((Bounds)(ref bounds)).size), 0f).results.Count == 0)
		{
			return true;
		}
		return false;
	}
}
