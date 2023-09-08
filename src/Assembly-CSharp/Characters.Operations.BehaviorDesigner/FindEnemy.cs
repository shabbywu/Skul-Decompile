using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using Characters.Operations.FindOptions;
using UnityEngine;

namespace Characters.Operations.BehaviorDesigner;

public class FindEnemy : Operation
{
	[SerializeField]
	private BehaviorDesignerCommunicator _communicator;

	[SerializeField]
	private Character _owner;

	[SerializeField]
	private string _storeVariableName = "Target";

	[SerializeField]
	private bool _shuffle = true;

	[SerializeField]
	private bool _exceptSelf;

	[Header("Find Option")]
	[SerializeReference]
	[SubclassSelector]
	private IScope _scope;

	[SubclassSelector]
	[SerializeReference]
	private IFilter[] _filters;

	[SerializeReference]
	[SubclassSelector]
	private ICondition[] _condition;

	public override void Run()
	{
		List<Character> enemyList = _scope.GetEnemyList();
		if (_exceptSelf && enemyList.Contains(_owner))
		{
			enemyList.Remove(_owner);
		}
		SharedCharacter variable = _communicator.GetVariable<SharedCharacter>(_storeVariableName);
		if (variable == null)
		{
			return;
		}
		if (enemyList.Count == 0)
		{
			((SharedVariable)variable).SetValue((object)null);
			return;
		}
		IFilter[] filters = _filters;
		for (int i = 0; i < filters.Length; i++)
		{
			filters[i].Filtered(enemyList);
		}
		if (_shuffle)
		{
			enemyList.PseudoShuffle();
		}
		foreach (Character item in enemyList)
		{
			bool flag = true;
			ICondition[] condition = _condition;
			for (int i = 0; i < condition.Length; i++)
			{
				if (!condition[i].Satisfied(item))
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				((SharedVariable)variable).SetValue((object)item);
				return;
			}
		}
		if (enemyList.Count > 0 && _condition.Length == 0)
		{
			((SharedVariable)variable).SetValue((object)enemyList[0]);
		}
		else
		{
			((SharedVariable)variable).SetValue((object)null);
		}
	}
}
