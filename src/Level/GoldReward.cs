using System;
using Characters;
using Services;
using Singletons;
using UnityEngine;

namespace Level;

public class GoldReward : InteractiveObject, ILootable
{
	[GetComponent]
	[SerializeField]
	private Animator _animator;

	public bool looted { get; private set; }

	public event Action onLoot;

	public override void OnActivate()
	{
		base.OnActivate();
		_animator.Play(InteractiveObject._activateHash);
	}

	public override void OnDeactivate()
	{
		base.OnDeactivate();
		_animator.Play(InteractiveObject._deactivateHash);
	}

	public override void InteractWith(Character character)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		this.onLoot?.Invoke();
		looted = true;
		PersistentSingleton<SoundManager>.Instance.PlaySound(_interactSound, ((Component)this).transform.position);
		Vector2Int goldrewardAmount = Singleton<Service>.Instance.levelManager.currentChapter.currentStage.goldrewardAmount;
		int amount = Random.Range(((Vector2Int)(ref goldrewardAmount)).x, ((Vector2Int)(ref goldrewardAmount)).y);
		Singleton<Service>.Instance.levelManager.DropGold(amount, 40, ((Component)this).transform.position);
		Deactivate();
	}
}
