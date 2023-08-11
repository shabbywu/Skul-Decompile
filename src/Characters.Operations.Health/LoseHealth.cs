using Services;
using Singletons;
using UnityEngine;

namespace Characters.Operations.Health;

public class LoseHealth : CharacterOperation
{
	private enum Type
	{
		Constnat,
		Percent,
		CurrentPercent
	}

	[SerializeField]
	private Type _type;

	[SerializeField]
	private CustomFloat _amount;

	[Tooltip("피해입었을 때 나타나는 숫자를 띄울지")]
	[SerializeField]
	private bool _spawnFloatingText;

	[SerializeField]
	private bool _allowSmallAmount;

	public override void Run(Character owner)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		double amount = GetAmount(owner);
		if (_allowSmallAmount || !(amount < 1.0))
		{
			owner.health.TakeHealth(amount);
			if (_spawnFloatingText)
			{
				Singleton<Service>.Instance.floatingTextSpawner.SpawnPlayerTakingDamage(amount, Vector2.op_Implicit(((Component)owner).transform.position));
			}
		}
	}

	private double GetAmount(Character owner)
	{
		return _type switch
		{
			Type.Constnat => _amount.value, 
			Type.Percent => (double)_amount.value * owner.health.maximumHealth * 0.01, 
			Type.CurrentPercent => (double)_amount.value * owner.health.currentHealth * 0.01, 
			_ => 0.0, 
		};
	}
}
