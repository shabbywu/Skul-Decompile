using System.Collections;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours;

public sealed class GoldmanePriestAI : AIController
{
	[Subcomponent(typeof(CheckWithinSight))]
	[SerializeField]
	private CheckWithinSight _checkWithinSight;

	[SerializeField]
	[Subcomponent(typeof(KeepDistance))]
	private KeepDistance _keepDistance;

	[Subcomponent(typeof(ActionAttack))]
	[SerializeField]
	private ActionAttack _heal;

	[SerializeField]
	private Collider2D _minimumCollider;

	[SerializeField]
	private Collider2D _healRange;

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
			if ((Object)(object)base.target == (Object)null)
			{
				continue;
			}
			if ((Object)(object)FindClosestPlayerBody(_minimumCollider) != (Object)null)
			{
				yield return _keepDistance.CRun(this);
				if (CanStartToHeal())
				{
					yield return _heal.CRun(this);
				}
			}
			else if (CanStartToHeal())
			{
				yield return _heal.CRun(this);
			}
		}
	}

	private bool CanStartToHeal()
	{
		foreach (Character item in FindEnemiesInRange(_healRange))
		{
			if (((Component)item).gameObject.activeSelf && !item.health.dead && item.health.percent < 1.0)
			{
				return true;
			}
		}
		return false;
	}
}
