using System.Collections;
using Characters.AI.Behaviours.Attacks;
using Characters.AI.Pope;
using Characters.Actions;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.Pope;

public sealed class Sacrament : Behaviour
{
	[SerializeField]
	private Character _character;

	[Subcomponent(typeof(ActionAttack))]
	[SerializeField]
	private ActionAttack _attack;

	[SerializeField]
	private Action _endMotion;

	[SerializeField]
	private SacramentOrbPool _sacramentOrbPool;

	[Subcomponent(typeof(MoveHandler))]
	[SerializeField]
	private MoveHandler _moveHandler;

	private void Start()
	{
		_sacramentOrbPool.Initialize(_character);
		_character.health.onDied += _sacramentOrbPool.Hide;
	}

	public override IEnumerator CRun(AIController controller)
	{
		base.result = Result.Doing;
		yield return _moveHandler.CMove(controller);
		_sacramentOrbPool.Run();
		yield return _attack.CRun(controller);
		_sacramentOrbPool.Hide();
		_endMotion.TryStart();
		while (_endMotion.running)
		{
			yield return null;
		}
		base.result = Result.Success;
	}
}
