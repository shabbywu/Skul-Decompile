using System;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Operations.Decorator;

public sealed class ByPositionX : CharacterOperation
{
	[Serializable]
	private class TargetInfo
	{
		[SerializeField]
		private Type _type;

		[SerializeField]
		private Transform _object;

		public float GetPositionX(Character owner)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			return _type switch
			{
				Type.Object => ((Component)_object).transform.position.x, 
				Type.Owner => ((Component)owner).transform.position.x, 
				Type.Player => ((Component)Singleton<Service>.Instance.levelManager.player).transform.position.x, 
				_ => 0f, 
			};
		}
	}

	private enum Type
	{
		Object,
		Owner,
		Player
	}

	[SerializeField]
	private TargetInfo _target;

	[SerializeField]
	private TargetInfo _based;

	[Subcomponent]
	[SerializeField]
	private CharacterOperation _onRight;

	[SerializeField]
	[Subcomponent]
	private CharacterOperation _onLeft;

	public override void Initialize()
	{
		if ((Object)(object)_onRight != (Object)null)
		{
			_onRight.Initialize();
		}
		if ((Object)(object)_onLeft != (Object)null)
		{
			_onLeft.Initialize();
		}
	}

	public override void Run(Character owner)
	{
		if (_target.GetPositionX(owner) > _based.GetPositionX(owner))
		{
			if (!((Object)(object)_onRight == (Object)null))
			{
				_onRight.Stop();
				_onRight.Run(owner);
			}
		}
		else if (!((Object)(object)_onLeft == (Object)null))
		{
			_onLeft.Stop();
			_onLeft.Run(owner);
		}
	}

	public override void Stop()
	{
		if ((Object)(object)_onRight != (Object)null)
		{
			_onRight.Stop();
		}
		if ((Object)(object)_onLeft != (Object)null)
		{
			_onLeft.Stop();
		}
	}
}
