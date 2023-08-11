using System;
using Characters.Gear;
using Characters.Gear.Items;
using Characters.Gear.Synergy.Inscriptions;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Triggers;

[Serializable]
public sealed class OnInscriptionItemDestroyed : Trigger
{
	[SerializeField]
	private Inscription.Key[] _keys;

	public override void Attach(Character character)
	{
		Singleton<Service>.Instance.gearManager.onItemInstanceChanged += HandleItemInstanceChanged;
		HandleItemInstanceChanged();
	}

	private void HandleItemInstanceChanged()
	{
		Inscription.Key[] keys = _keys;
		foreach (Inscription.Key key in keys)
		{
			foreach (Item item in Singleton<Service>.Instance.gearManager.GetItemInstanceByKeyword(key))
			{
				item.onDiscard -= TryInvoke;
				item.onDiscard += TryInvoke;
			}
		}
	}

	public void TryInvoke(Characters.Gear.Gear gear)
	{
		if (gear.destructible)
		{
			Invoke();
		}
	}

	public override void Detach()
	{
		Singleton<Service>.Instance.gearManager.onItemInstanceChanged -= HandleItemInstanceChanged;
		Inscription.Key[] keys = _keys;
		foreach (Inscription.Key key in keys)
		{
			foreach (Item item in Singleton<Service>.Instance.gearManager.GetItemInstanceByKeyword(key))
			{
				item.onDiscard -= TryInvoke;
			}
		}
	}
}
