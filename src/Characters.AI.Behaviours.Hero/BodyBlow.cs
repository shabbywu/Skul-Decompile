using System;
using System.Collections;
using Characters.AI.Hero;
using Characters.Actions;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.Hero;

public class BodyBlow : Behaviour, IFinish, IComboable
{
	[SerializeField]
	[Subcomponent(typeof(ChainAction))]
	private Characters.Actions.Action _startAction;

	[Subcomponent(typeof(ChainAction))]
	[SerializeField]
	private Characters.Actions.Action _readyAction;

	[Subcomponent(typeof(ChainAction))]
	[SerializeField]
	private Characters.Actions.Action _attackAction;

	[SerializeField]
	[Subcomponent(typeof(ChainAction))]
	private Characters.Actions.Action _failAction;

	[SerializeField]
	[Subcomponent(typeof(SlashCombo))]
	private SlashCombo _slashCombo;

	[Subcomponent(typeof(SkipableIdle))]
	[SerializeField]
	private SkipableIdle _skipableIdle;

	private bool _canUseSlashCombo;

	public IEnumerator CTryContinuedCombo(AIController controller, ComboSystem comboSystem)
	{
		comboSystem.Clear();
		yield return CCombat(controller);
	}

	public override IEnumerator CRun(AIController controller)
	{
		_startAction.TryStart();
		while (_startAction.running)
		{
			yield return null;
		}
		yield return CCombat(controller);
	}

	private IEnumerator CCombat(AIController controller)
	{
		Character character = controller.character;
		character.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character.onGaveDamage, new GaveDamageDelegate(CheckHit));
		_readyAction.TryStart();
		while (_readyAction.running)
		{
			yield return null;
		}
		_attackAction.TryStart();
		while (_attackAction.running)
		{
			yield return null;
		}
		Character character2 = controller.character;
		character2.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character2.onGaveDamage, new GaveDamageDelegate(CheckHit));
		if (_canUseSlashCombo)
		{
			yield return _slashCombo.CRun(controller);
		}
		else
		{
			_failAction.TryStart();
			while (_failAction.running)
			{
				yield return null;
			}
		}
		_canUseSlashCombo = false;
		yield return _skipableIdle.CRun(controller);
	}

	private void CheckHit(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
	{
		_canUseSlashCombo = true;
	}
}
