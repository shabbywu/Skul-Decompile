using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Abilities;

public class InMapAbilityAttacher : AbilityAttacher
{
	private enum AttachType
	{
		Reset,
		Refresh
	}

	[SerializeField]
	private AttachType _attachType;

	[SerializeField]
	private Map.Type[] _exceptTypes;

	[AbilityComponent.Subcomponent]
	[SerializeField]
	private AbilityComponent _abilityComponent;

	public override void OnIntialize()
	{
		_abilityComponent.Initialize();
	}

	public override void StartAttach()
	{
		Singleton<Service>.Instance.levelManager.onMapLoaded += ResetAbility;
	}

	public override void StopAttach()
	{
		if (!Service.quitting && !((Object)(object)base.owner == (Object)null))
		{
			Singleton<Service>.Instance.levelManager.onMapLoaded -= ResetAbility;
			base.owner.ability.Remove(_abilityComponent.ability);
		}
	}

	private void ResetAbility()
	{
		Map.Type type = Map.Instance.type;
		if (_exceptTypes != null)
		{
			Map.Type[] exceptTypes = _exceptTypes;
			for (int i = 0; i < exceptTypes.Length; i++)
			{
				if (exceptTypes[i] == type)
				{
					return;
				}
			}
		}
		switch (_attachType)
		{
		case AttachType.Reset:
			base.owner.ability.Remove(_abilityComponent.ability);
			base.owner.ability.Add(_abilityComponent.ability);
			break;
		case AttachType.Refresh:
			base.owner.ability.Add(_abilityComponent.ability);
			break;
		}
	}

	public override string ToString()
	{
		return this.GetAutoName();
	}
}
