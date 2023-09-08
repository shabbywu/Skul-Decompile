using System.Collections.Generic;
using Characters.Operations.FindOptions;
using UnityEngine;
using Utils;

namespace Characters.Operations.GrabBorad;

public class AddToGrabBoard : Operation
{
	[SerializeField]
	private GrabBoard _grabBoard;

	[SerializeField]
	private Character _owner;

	[SerializeField]
	private bool _exceptSelf;

	[SerializeReference]
	[SubclassSelector]
	[Header("Find Option")]
	private IScope _scope;

	[SerializeReference]
	[SubclassSelector]
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
		if (enemyList.Count == 0)
		{
			return;
		}
		IFilter[] filters = _filters;
		for (int i = 0; i < filters.Length; i++)
		{
			filters[i].Filtered(enemyList);
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
			if (!flag)
			{
				continue;
			}
			Target target = ((Component)item).GetComponent<Target>();
			if ((Object)(object)target != (Object)null)
			{
				_grabBoard.Add(target);
				item.onDie += delegate
				{
					_grabBoard.Remove(target);
				};
			}
		}
	}
}
