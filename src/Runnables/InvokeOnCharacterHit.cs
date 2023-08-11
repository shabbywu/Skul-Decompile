using Characters;
using Runnables.Triggers;
using UnityEngine;

namespace Runnables;

public class InvokeOnCharacterHit : MonoBehaviour
{
	[SerializeField]
	[Trigger.Subcomponent]
	private Trigger _trigger;

	[SerializeField]
	private Character _owner;

	[SerializeReference]
	[SubclassSelector]
	private IHitEvent[] _hitEvents;

	private void Awake()
	{
		if ((Object)(object)_owner == (Object)null)
		{
			_owner = ((Component)this).GetComponentInParent<Character>();
		}
		IHitEvent[] hitEvents = _hitEvents;
		for (int i = 0; i < hitEvents.Length; i++)
		{
			_ = hitEvents[i];
			_owner.health.onTookDamage += ExecuteHitEvents;
		}
	}

	private void OnDestroy()
	{
		IHitEvent[] hitEvents = _hitEvents;
		for (int i = 0; i < hitEvents.Length; i++)
		{
			_ = hitEvents[i];
			_owner.health.onTookDamage -= ExecuteHitEvents;
		}
	}

	private void ExecuteHitEvents(in Damage originalDamage, in Damage tookDamage, double damageDealt)
	{
		if (_trigger.IsSatisfied())
		{
			IHitEvent[] hitEvents = _hitEvents;
			for (int i = 0; i < hitEvents.Length; i++)
			{
				hitEvents[i].OnHit(in originalDamage, in tookDamage, damageDealt);
			}
		}
	}
}
