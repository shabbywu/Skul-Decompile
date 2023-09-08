using UnityEngine;

namespace Characters.AI.YggdrasillElderEnt;

public class SweepHandController : MonoBehaviour
{
	[SerializeField]
	private Health _ownerHealth;

	[SerializeField]
	private SweepHand _left;

	[SerializeField]
	private SweepHand _right;

	private SweepHand _current;

	public bool left { get; private set; }

	private void Awake()
	{
		_ownerHealth.onDie += Stop;
	}

	public void Select()
	{
		left = MMMaths.RandomBool();
		_current = (left ? _left : _right);
	}

	public void Attack()
	{
		if ((Object)(object)_current != (Object)null)
		{
			_current.Attack();
		}
	}

	public void Stop()
	{
		if ((Object)(object)_current != (Object)null)
		{
			_current.Stop();
		}
	}

	public void ReplaceHands()
	{
		if (!((Object)(object)_current == (Object)null))
		{
			Stop();
			_current = (((Object)(object)_current == (Object)(object)_left) ? _right : _left);
		}
	}
}
