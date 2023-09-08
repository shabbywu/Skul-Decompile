using BehaviorDesigner.Runtime;
using Characters;
using Characters.AI;
using UnityEngine;

public class GuardColliderController : MonoBehaviour
{
	[SerializeField]
	private Collider2D _collider;

	[SerializeField]
	private AIController _ai;

	[SerializeField]
	private BehaviorDesignerCommunicator _communicator;

	[SerializeField]
	private string _ownerKey = "Owner";

	[SerializeField]
	private string _targetKey = "Target";

	private Character _owner;

	private Character _target;

	private Character GetTarget()
	{
		if ((Object)(object)_target == (Object)null)
		{
			if ((Object)(object)_ai != (Object)null)
			{
				_target = _ai.target;
			}
			else
			{
				_target = ((SharedVariable<Character>)_communicator.GetVariable<SharedCharacter>(_targetKey)).Value;
			}
		}
		return _target;
	}

	private void Awake()
	{
		if ((Object)(object)_ai != (Object)null)
		{
			_owner = _ai.character;
		}
		else
		{
			_owner = ((SharedVariable<Character>)_communicator.GetVariable<SharedCharacter>(_ownerKey)).Value;
		}
	}

	private void Update()
	{
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)GetTarget() == (Object)null)
		{
			return;
		}
		if (GetTarget().status.stuned || GetTarget().status.unmovable)
		{
			((Component)_collider).gameObject.SetActive(false);
			((Component)this).gameObject.SetActive(false);
		}
		if (_owner.lookingDirection == Character.LookingDirection.Right)
		{
			if (((Component)GetTarget()).transform.position.x < ((Component)this).transform.position.x)
			{
				((Behaviour)_collider).enabled = false;
			}
			else
			{
				((Behaviour)_collider).enabled = true;
			}
		}
		else if (((Component)GetTarget()).transform.position.x > ((Component)this).transform.position.x)
		{
			((Behaviour)_collider).enabled = false;
		}
		else
		{
			((Behaviour)_collider).enabled = true;
		}
	}
}
