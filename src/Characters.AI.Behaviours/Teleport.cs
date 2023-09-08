using System;
using System.Collections;
using Characters.Actions;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours;

public class Teleport : Behaviour
{
	[AttributeUsage(AttributeTargets.Field)]
	public new class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
	{
		public new static readonly Type[] types = new Type[2]
		{
			typeof(Teleport),
			typeof(TeleportBehind)
		};

		public SubcomponentAttribute(bool allowCustom = true)
			: base(allowCustom, types)
		{
		}
	}

	[SerializeField]
	private Characters.Actions.Action _teleportStart;

	[SerializeField]
	private Characters.Actions.Action _teleportEnd;

	[UnityEditor.Subcomponent(typeof(Hide))]
	[SerializeField]
	private Hide _hide;

	[SerializeField]
	[UnityEditor.Subcomponent(typeof(Idle))]
	private Idle _idle;

	public override IEnumerator CRun(AIController controller)
	{
		base.result = Result.Doing;
		float num = ((Component)controller.target).transform.position.x - ((Component)controller.character).transform.position.x;
		controller.character.lookingDirection = ((!(num > 0f)) ? Character.LookingDirection.Left : Character.LookingDirection.Right);
		if ((Object)(object)_teleportStart == (Object)null || _teleportStart.TryStart())
		{
			if ((Object)(object)_teleportStart != (Object)null)
			{
				while (_teleportStart.running)
				{
					yield return null;
				}
			}
			yield return _hide.CRun(controller);
			_teleportEnd.TryStart();
			while (_teleportEnd.running)
			{
				yield return null;
			}
			yield return _idle.CRun(controller);
			base.result = Result.Success;
		}
		else
		{
			base.result = Result.Fail;
		}
	}

	public bool CanUse()
	{
		if (_teleportEnd.canUse)
		{
			return _teleportStart.canUse;
		}
		return false;
	}
}
