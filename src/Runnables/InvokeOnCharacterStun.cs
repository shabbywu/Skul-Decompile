using Characters;
using Runnables.Triggers;
using UnityEngine;

namespace Runnables;

public class InvokeOnCharacterStun : MonoBehaviour
{
	[Trigger.Subcomponent]
	[SerializeField]
	private Trigger _trigger;

	[SerializeField]
	private Character _owner;

	[SubclassSelector]
	[SerializeReference]
	private IStatusEvent _statusEvent;

	public void Initialize(IStatusEvent statusEvent)
	{
		if ((Object)(object)_owner == (Object)null)
		{
			_owner = ((Component)this).GetComponentInParent<Character>();
		}
		_statusEvent = statusEvent;
	}

	private void Awake()
	{
		_owner.status.stun.onAttachEvents += ApplyStatusEvent;
		_owner.status.stun.onDetachEvents += ReleaseStatusEvent;
	}

	private void OnDestroy()
	{
		_owner.status.stun.onAttachEvents -= ApplyStatusEvent;
		_owner.status.stun.onDetachEvents -= ReleaseStatusEvent;
	}

	private void ApplyStatusEvent(Character owner, Character target)
	{
		if (_trigger.IsSatisfied())
		{
			_statusEvent.Apply(owner, target);
		}
	}

	private void ReleaseStatusEvent(Character owner, Character target)
	{
		if (_trigger.IsSatisfied())
		{
			_statusEvent.Release(owner, target);
		}
	}
}
