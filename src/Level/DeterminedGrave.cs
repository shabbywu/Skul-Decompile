using System.Collections;
using Characters;
using Characters.Gear.Weapons;
using Services;
using Singletons;
using UnityEngine;

namespace Level;

public class DeterminedGrave : InteractiveObject
{
	[GetComponent]
	[SerializeField]
	private Animator _animator;

	[SerializeField]
	private Weapon _weapon;

	public Weapon droppedWeapon { get; private set; }

	public override void OnActivate()
	{
		_animator.Play(InteractiveObject._activateHash);
	}

	public override void OnDeactivate()
	{
		_animator.Play(InteractiveObject._deactivateHash);
	}

	public override void InteractWith(Character character)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		PersistentSingleton<SoundManager>.Instance.PlaySound(_interactSound, ((Component)this).transform.position);
		((MonoBehaviour)this).StartCoroutine(CDelayedDrop());
		Deactivate();
		IEnumerator CDelayedDrop()
		{
			yield return Chronometer.global.WaitForSeconds(0.4f);
			droppedWeapon = Singleton<Service>.Instance.levelManager.DropWeapon(_weapon, ((Component)this).transform.position);
		}
	}
}
