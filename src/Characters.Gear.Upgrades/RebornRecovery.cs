using Characters.Abilities;
using Characters.Player;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Gear.Upgrades;

public sealed class RebornRecovery : UpgradeAbility
{
	[SerializeField]
	private UpgradeObject _upgrade;

	[SerializeField]
	private ReviveComponent _revive;

	private Character _owner;

	private bool _attached;

	public override void Attach(Character target)
	{
		_owner = target;
		_attached = true;
		_revive.Initialize();
		_owner.ability.Add(_revive.ability);
	}

	private void Update()
	{
		if (_attached && !_owner.ability.Contains(_revive.ability))
		{
			DestroyAbility();
		}
	}

	public override void Detach()
	{
		if (!Service.quitting)
		{
			_owner.ability.Remove(_revive.ability);
		}
	}

	private void DestroyAbility()
	{
		_owner.ability.Remove(_revive.ability);
		UpgradeInventory upgrade = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.upgrade;
		int num = upgrade.IndexOf(_upgrade);
		if (num != -1)
		{
			upgrade.Remove(num);
		}
	}
}
