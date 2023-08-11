using System;
using Characters;
using Characters.Actions;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

[TaskIcon("{SkinColor}StackedActionIcon.png")]
[TaskDescription("Allows multiple action tasks to be added to a single node.")]
public sealed class TryToHit : Action
{
	[SerializeField]
	private SharedCharacter _character;

	[SerializeField]
	private SharedCharacterAction _attack;

	[SerializeField]
	private string _key;

	private Character _characterValue;

	private Characters.Actions.Action _attackValue;

	private bool _hit;

	private TaskStatus _beforeTaskStatus;

	public override void OnAwake()
	{
		_characterValue = ((SharedVariable<Character>)_character).Value;
		_attackValue = ((SharedVariable<Characters.Actions.Action>)_attack).Value;
	}

	public override void OnStart()
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		_hit = false;
		Character characterValue = _characterValue;
		characterValue.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(characterValue.onGaveDamage, new GaveDamageDelegate(HandleOnGaveDamage));
		_beforeTaskStatus = (TaskStatus)0;
	}

	public override TaskStatus OnUpdate()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Invalid comparison between Unknown and I4
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		if (_hit)
		{
			_beforeTaskStatus = (TaskStatus)2;
			return _beforeTaskStatus;
		}
		if (_attackValue.running)
		{
			_beforeTaskStatus = (TaskStatus)3;
			return (TaskStatus)3;
		}
		if ((int)_beforeTaskStatus != 3)
		{
			if ((int)_beforeTaskStatus == 0)
			{
				if (_attackValue.TryStart())
				{
					_beforeTaskStatus = (TaskStatus)3;
					return (TaskStatus)3;
				}
				_beforeTaskStatus = (TaskStatus)0;
				return (TaskStatus)3;
			}
			return (TaskStatus)3;
		}
		return (TaskStatus)1;
	}

	public override void OnEnd()
	{
		Character characterValue = _characterValue;
		characterValue.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(characterValue.onGaveDamage, new GaveDamageDelegate(HandleOnGaveDamage));
	}

	private void HandleOnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
	{
		if (string.IsNullOrEmpty(_key))
		{
			_hit = true;
		}
		else if (string.Equals(originalDamage.key, _key, StringComparison.OrdinalIgnoreCase))
		{
			_hit = true;
		}
	}
}
