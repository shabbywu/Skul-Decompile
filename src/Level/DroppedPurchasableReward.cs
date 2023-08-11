using System;
using Characters;
using Data;
using GameResources;
using Singletons;
using UnityEngine;

namespace Level;

public abstract class DroppedPurchasableReward : InteractiveObject, IPurchasable
{
	[SerializeField]
	private DropMovement _dropMovement;

	[SerializeField]
	private DroppedEffect _droppedEffect;

	[SerializeField]
	private SpriteRenderer _spriteRenderer;

	protected const string _prefix = "DroppedPurchasableReward";

	public int price { get; set; }

	public GameData.Currency.Type priceCurrency { get; set; }

	protected string _keyBase => "DroppedPurchasableReward/" + ((Object)this).name;

	public virtual string displayName => Localization.GetLocalizedString(_keyBase + "/name");

	public virtual string description => Localization.GetLocalizedString(_keyBase + "/desc");

	public DropMovement dropMovement => _dropMovement;

	public event Action<Character> onLoot;

	protected override void Awake()
	{
		base.Awake();
		if ((Object)(object)_dropMovement == (Object)null)
		{
			Activate();
		}
		else
		{
			_dropMovement.onGround += Activate;
		}
	}

	public override void InteractWith(Character character)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		base.InteractWith(character);
		GameData.Currency currency = GameData.Currency.currencies[priceCurrency];
		if (!currency.Has(price))
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(_interactFailSound, ((Component)this).transform.position);
			return;
		}
		if (!currency.Consume(price))
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(_interactFailSound, ((Component)this).transform.position);
			return;
		}
		PersistentSingleton<SoundManager>.Instance.PlaySound(_interactSound, ((Component)this).transform.position);
		price = 0;
		this.onLoot?.Invoke(character);
	}
}
