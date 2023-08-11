using System.Collections;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI;

public sealed class DarkQuartzEnt : AIController
{
	[SerializeField]
	[Subcomponent(typeof(CheckWithinSight))]
	private CheckWithinSight _checkWithinSight;

	[SerializeField]
	[Wander.Subcomponent(true)]
	private Wander _wander;

	[Subcomponent(typeof(Chase))]
	[SerializeField]
	private Chase _chase;

	[Subcomponent(typeof(ActionAttack))]
	[SerializeField]
	private ActionAttack _meleeAttack;

	[Subcomponent(typeof(ActionAttack))]
	[SerializeField]
	private ActionAttack _areaAttack;

	[SerializeField]
	private Collider2D _meleeAttackCollider;

	[SerializeField]
	private Collider2D _areaAttackCollider;

	[SerializeField]
	private Transform _effect;

	[SerializeField]
	private Collider2D _effectCollider;

	private float _effectBoundsX;

	protected override void OnEnable()
	{
		base.OnEnable();
		((MonoBehaviour)this).StartCoroutine(CProcess());
		((MonoBehaviour)this).StartCoroutine(_checkWithinSight.CRun(this));
	}

	protected override IEnumerator CProcess()
	{
		yield return CPlayStartOption();
		yield return _wander.CRun(this);
		yield return Combat();
	}

	private void OnDestroy()
	{
		Object.Destroy((Object)(object)((Component)_effect).gameObject);
	}

	private IEnumerator Combat()
	{
		DarkQuartzEnt darkQuartzEnt = this;
		Bounds bounds = _effectCollider.bounds;
		darkQuartzEnt._effectBoundsX = ((Bounds)(ref bounds)).size.x;
		while (!base.dead)
		{
			yield return null;
			if ((Object)(object)base.target == (Object)null || !character.movement.controller.isGrounded)
			{
				continue;
			}
			if ((Object)(object)FindClosestPlayerBody(_meleeAttackCollider) != (Object)null)
			{
				if (_areaAttack.CanUse())
				{
					SetPosition();
					yield return _areaAttack.CRun(this);
				}
				else
				{
					yield return _meleeAttack.CRun(this);
				}
			}
			else if ((Object)(object)FindClosestPlayerBody(_areaAttackCollider) != (Object)null)
			{
				if (base.target.movement.isGrounded)
				{
					if (_areaAttack.CanUse())
					{
						SetPosition();
						yield return _areaAttack.CRun(this);
					}
					else
					{
						yield return _chase.CRun(this);
					}
				}
			}
			else
			{
				yield return _chase.CRun(this);
			}
		}
	}

	private void SetPosition()
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		Bounds bounds = base.target.movement.controller.collisionState.lastStandingCollider.bounds;
		if (((Bounds)(ref bounds)).max.x - 1f < ((Component)base.target).transform.position.x + _effectBoundsX / 2f)
		{
			_effect.position = Vector2.op_Implicit(new Vector2(((Bounds)(ref bounds)).max.x - _effectBoundsX / 2f, ((Bounds)(ref bounds)).max.y));
		}
		else if (((Bounds)(ref bounds)).min.x + 1f > ((Component)base.target).transform.position.x - _effectBoundsX / 2f)
		{
			_effect.position = Vector2.op_Implicit(new Vector2(((Bounds)(ref bounds)).min.x + _effectBoundsX / 2f, ((Bounds)(ref bounds)).max.y));
		}
		else
		{
			_effect.position = Vector2.op_Implicit(new Vector2(((Component)base.target).transform.position.x, ((Bounds)(ref bounds)).max.y));
		}
	}
}
