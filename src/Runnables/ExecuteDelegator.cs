using System;
using Characters;
using UnityEngine;

namespace Runnables;

[Serializable]
public class ExecuteDelegator : IHitEvent, IStatusEvent
{
	[SerializeField]
	private Runnable[] _runnables;

	public void OnHit(in Damage originalDamage, in Damage tookDamage, double damageDealt)
	{
		Runnable[] runnables = _runnables;
		for (int i = 0; i < runnables.Length; i++)
		{
			runnables[i].Run();
		}
	}

	public void Apply(Character owner, Character target)
	{
		Runnable[] runnables = _runnables;
		for (int i = 0; i < runnables.Length; i++)
		{
			runnables[i].Run();
		}
	}

	public void Release(Character owner, Character target)
	{
	}

	void IHitEvent.OnHit(in Damage originalDamage, in Damage tookDamage, double damageDealt)
	{
		OnHit(in originalDamage, in tookDamage, damageDealt);
	}
}
