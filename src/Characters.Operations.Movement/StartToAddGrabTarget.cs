using System;
using System.Collections;
using System.Linq;
using Characters.Movements;
using UnityEngine;
using Utils;

namespace Characters.Operations.Movement;

public sealed class StartToAddGrabTarget : CharacterOperation
{
	[SerializeField]
	private GrabBoard _grabBoard;

	[SerializeField]
	private float _duration;

	[SerializeField]
	private string[] _attackKeys = new string[1] { "grab" };

	[SerializeField]
	private ChronoInfo _chronoToGlobe;

	[SerializeField]
	private ChronoInfo _chronoToOwner;

	[SerializeField]
	private ChronoInfo _chronoToTarget;

	private Character _owner;

	public override void Run(Character owner)
	{
		_owner = owner;
		owner.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(owner.onGaveDamage, new GaveDamageDelegate(OnOwnerGaveDamage));
		if (_duration > 0f)
		{
			((MonoBehaviour)this).StartCoroutine(WaitForDuration());
		}
	}

	private IEnumerator WaitForDuration()
	{
		yield return Chronometer.global.WaitForSeconds(_duration);
		Character owner = _owner;
		owner.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(owner.onGaveDamage, new GaveDamageDelegate(OnOwnerGaveDamage));
	}

	private void OnOwnerGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
	{
		if (target == null || (Object)(object)target.character == (Object)null || target.character.health.dead)
		{
			return;
		}
		Damage gaveDamageTemp = gaveDamage;
		if (!_attackKeys.Any((string key) => gaveDamageTemp.key.Equals(key, StringComparison.OrdinalIgnoreCase)))
		{
			return;
		}
		Target target2 = target as Target;
		if (!((Object)(object)target2 == (Object)null))
		{
			if (target.character.movement.config.type == Characters.Movements.Movement.Config.Type.Static)
			{
				_grabBoard.AddFailed(target2);
				return;
			}
			if (target.character.stat.GetFinal(Stat.Kind.KnockbackResistance) == 0.0)
			{
				_grabBoard.AddFailed(target2);
				return;
			}
			_chronoToGlobe.ApplyGlobe();
			_chronoToOwner.ApplyTo(_owner);
			_chronoToTarget.ApplyTo(target.character);
			_grabBoard.Add(target2);
		}
	}

	public override void Stop()
	{
		base.Stop();
		if (!((Object)(object)_owner == (Object)null))
		{
			Character owner = _owner;
			owner.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(owner.onGaveDamage, new GaveDamageDelegate(OnOwnerGaveDamage));
		}
	}
}
