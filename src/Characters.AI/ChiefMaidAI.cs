using System.Collections;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI;

public sealed class ChiefMaidAI : AIController
{
	[SerializeField]
	private Collider2D _spawnCollider;

	[SerializeField]
	private Collider2D _keepDistanceCollider;

	[Subcomponent(typeof(CheckWithinSight))]
	[SerializeField]
	private CheckWithinSight _checkWithinSight;

	[SerializeField]
	[Subcomponent(typeof(KeepDistance))]
	private KeepDistance _keepDistance;

	[Subcomponent(typeof(SpawnEnemy))]
	[SerializeField]
	private SpawnEnemy _spawnEnemy;

	[Space]
	[Header("ImpectBell")]
	[Subcomponent(typeof(ActionAttack))]
	[SerializeField]
	private ActionAttack _impactBell;

	[SerializeField]
	private Collider2D _impactBellTrigger;

	[SerializeField]
	private Transform _attackPoint;

	private void Awake()
	{
		_attackPoint.parent = null;
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		((MonoBehaviour)this).StartCoroutine(CProcess());
		((MonoBehaviour)this).StartCoroutine(_checkWithinSight.CRun(this));
	}

	protected override IEnumerator CProcess()
	{
		yield return CPlayStartOption();
		yield return Combat();
	}

	private IEnumerator Combat()
	{
		while (!base.dead)
		{
			yield return null;
			if (!((Object)(object)base.target == (Object)null))
			{
				if (Object.op_Implicit((Object)(object)FindClosestPlayerBody(_spawnCollider)))
				{
					yield return _spawnEnemy.CRun(this);
				}
				if (Object.op_Implicit((Object)(object)FindClosestPlayerBody(_impactBellTrigger)) && _impactBell.CanUse())
				{
					ShiftAttackPoint();
					yield return _impactBell.CRun(this);
				}
				if (Object.op_Implicit((Object)(object)FindClosestPlayerBody(_keepDistanceCollider)) && _keepDistance.CanUseBackMove())
				{
					character.movement.moveBackward = true;
					yield return _keepDistance.CRun(this);
					character.movement.moveBackward = false;
				}
			}
		}
	}

	private void ShiftAttackPoint()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		_attackPoint.position = ((Component)base.target).transform.position;
	}
}
