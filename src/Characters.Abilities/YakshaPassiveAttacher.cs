using Characters.Abilities.Customs;
using UnityEngine;

namespace Characters.Abilities;

public class YakshaPassiveAttacher : AbilityAttacher
{
	[SerializeField]
	private YakshaPassive _yakshaPassive;

	public override void OnIntialize()
	{
		_yakshaPassive.owner = base.owner;
		_yakshaPassive.Initialize();
	}

	public override void StartAttach()
	{
		base.owner.ability.Add(_yakshaPassive);
	}

	public override void StopAttach()
	{
		if (!((Object)(object)base.owner == (Object)null))
		{
			base.owner.ability.Remove(_yakshaPassive);
		}
	}

	public void AddStack()
	{
		_yakshaPassive.AddStack();
	}

	public override string ToString()
	{
		return this.GetAutoName();
	}
}
