using UnityEngine;

namespace Level.BlackMarket;

public abstract class Npc : MonoBehaviour
{
	protected static readonly int _activateHash = Animator.StringToHash("Activate");

	protected static readonly int _activate2Hash = Animator.StringToHash("Activate2");

	protected static readonly int _deactivateHash = Animator.StringToHash("Deactivate");

	[SerializeField]
	protected Animator _animator;

	[SerializeField]
	protected GameObject _minimapAgent;

	public void Activate()
	{
		if (MMMaths.RandomBool() && _animator.HasState(0, _activate2Hash))
		{
			_animator.Play(_activate2Hash);
		}
		else
		{
			_animator.Play(_activateHash);
		}
		_minimapAgent.SetActive(true);
		OnActivate();
	}

	public void Deactivate()
	{
		_animator.Play(_deactivateHash);
		_minimapAgent.SetActive(false);
		OnDeactivate();
	}

	protected abstract void OnActivate();

	protected abstract void OnDeactivate();
}
