using System;
using Services;
using Singletons;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

[Serializable]
public class SetPlayerAsTarget : Action
{
	[SerializeField]
	private SharedCharacter _target;

	public override TaskStatus OnUpdate()
	{
		if (!((Object)(object)Singleton<Service>.Instance.levelManager.player == (Object)null))
		{
			((SharedVariable)_target).SetValue((object)Singleton<Service>.Instance.levelManager.player);
			return (TaskStatus)2;
		}
		return (TaskStatus)1;
	}
}
