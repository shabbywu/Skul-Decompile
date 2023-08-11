using Characters;
using Services;
using Singletons;
using UnityEngine;

namespace Runnables;

public sealed class TakeHealth : Runnable
{
	[SerializeField]
	private Target _target;

	[SerializeField]
	private CustomFloat _amount;

	public override void Run()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		Character character = _target.character;
		float value = _amount.value;
		Singleton<Service>.Instance.floatingTextSpawner.SpawnPlayerTakingDamage(value, Vector2.op_Implicit(((Component)character).transform.position));
		character.health.TakeHealth(value);
	}
}
