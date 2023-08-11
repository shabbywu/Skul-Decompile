using Characters;
using Services;
using Singletons;
using UnityEngine;

namespace Level;

public class BossGate : InteractiveObject
{
	[SerializeField]
	[GetComponent]
	private Animator _animator;

	private bool _used;

	public override void OnActivate()
	{
		base.OnActivate();
		Animator animator = _animator;
		if (animator != null)
		{
			animator.Play(InteractiveObject._activateHash);
		}
	}

	public override void OnDeactivate()
	{
		base.OnDeactivate();
		Animator animator = _animator;
		if (animator != null)
		{
			animator.Play(InteractiveObject._deactivateHash);
		}
	}

	public override void InteractWith(Character character)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		if (!_used)
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(_interactSound, ((Component)this).transform.position);
			_used = true;
			Singleton<Service>.Instance.levelManager.LoadNextMap();
		}
	}
}
