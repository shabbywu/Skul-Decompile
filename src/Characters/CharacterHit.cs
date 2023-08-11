using Characters.AI;
using Characters.Actions;
using FX;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Characters;

public class CharacterHit : MonoBehaviour
{
	[SerializeField]
	[GetComponent]
	private Character _character;

	[SerializeField]
	[GetComponent]
	private CharacterHealth _health;

	[SerializeField]
	protected SoundInfo _hitSound;

	[SerializeField]
	[Subcomponent(true, typeof(SequentialAction))]
	private SequentialAction _action;

	[SerializeField]
	private EnemyDiedAction _deadAction;

	private int _motionIndex;

	public Action action => _action;

	private void Awake()
	{
		_health.onTookDamage += onTookDamage;
	}

	private void onTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		if (damageDealt > 0.0)
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(_hitSound, ((Component)this).transform.position);
		}
	}

	public void Stop(float stoppingPower)
	{
		stoppingPower *= (float)_character.stat.GetFinal(Stat.Kind.StoppingResistance);
		if (!(stoppingPower <= 0f) && !((Object)(object)_action == (Object)null) && !((Object)(object)_action.currentMotion == (Object)null) && !_character.stunedOrFreezed && !_character.health.dead && (!((Object)(object)_deadAction != (Object)null) || !_deadAction.diedAction.running))
		{
			_action.currentMotion.length = stoppingPower;
			_action.TryStart();
		}
	}
}
