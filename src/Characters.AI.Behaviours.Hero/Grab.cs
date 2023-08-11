using System;
using System.Collections;
using Characters.AI.Hero;
using Characters.Actions;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.Hero;

public class Grab : Behaviour, IFinish, IComboable
{
	[SerializeField]
	[Subcomponent(typeof(ChainAction))]
	private Characters.Actions.Action _startAction;

	[SerializeField]
	[Subcomponent(typeof(ChainAction))]
	private Characters.Actions.Action _attackAction;

	[Subcomponent(typeof(ChainAction))]
	[SerializeField]
	private Characters.Actions.Action _grapFailAction;

	[SerializeField]
	[Subcomponent(typeof(SkipableIdle))]
	private SkipableIdle _skipableIdle;

	[Subcomponent(typeof(Throwing))]
	[SerializeField]
	private Throwing _throwing;

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
		_attackAction.TryStart();
		while (_attackAction.running)
		{
			yield return null;
		}
		Character character2 = controller.character;
		character2.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character2.onGaveDamage, new GaveDamageDelegate(CheckHit));
		if (_canUseSlashCombo)
		{
			yield return _throwing.CRun(controller);
		}
		else
		{
			_grapFailAction.TryStart();
			while (_grapFailAction.running)
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
