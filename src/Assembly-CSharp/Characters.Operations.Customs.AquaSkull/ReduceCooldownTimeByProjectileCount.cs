using Characters.Actions;
using Characters.Projectiles;
using UnityEngine;

namespace Characters.Operations.Customs.AquaSkull;

public class ReduceCooldownTimeByProjectileCount : Operation
{
	[SerializeField]
	private Action[] _actions;

	[SerializeField]
	private Projectile[] _projectilesToCount;

	[SerializeField]
	[Tooltip("0~100사이 값")]
	private float[] _reducePercentByCount;

	public override void Run()
	{
		int num = 0;
		Projectile[] projectilesToCount = _projectilesToCount;
		foreach (Projectile projectile in projectilesToCount)
		{
			num += projectile.reusable.spawnedCount;
		}
		int num2 = Mathf.Clamp(num, 0, _reducePercentByCount.Length - 1);
		Action[] actions = _actions;
		for (int i = 0; i < actions.Length; i++)
		{
			actions[i].cooldown.time.ReduceCooldownPercent(_reducePercentByCount[num2] / 100f);
		}
	}
}
