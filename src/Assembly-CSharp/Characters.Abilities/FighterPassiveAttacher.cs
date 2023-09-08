using Characters.Abilities.Customs;
using Scenes;
using UnityEngine;

namespace Characters.Abilities;

public class FighterPassiveAttacher : AbilityAttacher
{
	[SerializeField]
	private FighterPassive _fighterPassive;

	private bool _storingTime;

	public bool rageReady => _fighterPassive.rageReady;

	public override void OnIntialize()
	{
		_fighterPassive.Initialize();
	}

	public override void StartAttach()
	{
		base.owner.ability.Add(_fighterPassive);
	}

	public override void StopAttach()
	{
		if (!((Object)(object)base.owner == (Object)null))
		{
			base.owner.ability.Remove(_fighterPassive);
		}
	}

	private void Update()
	{
		if (_fighterPassive.buffAttached && Scene<GameBase>.instance.uiManager.letterBox.visible)
		{
			if (!_storingTime)
			{
				_storingTime = true;
				Chronometer.global.DetachTimeScale(_fighterPassive);
				base.owner.chronometer.master.DetachTimeScale(_fighterPassive);
			}
		}
		else if (_storingTime)
		{
			_storingTime = false;
			if (_fighterPassive.buffAttached)
			{
				Chronometer.global.AttachTimeScale(_fighterPassive, _fighterPassive.timeScale);
				base.owner.chronometer.master.AttachTimeScale(_fighterPassive, 1f / _fighterPassive.timeScale);
			}
		}
	}

	public void Rage()
	{
		_fighterPassive.AttachRage();
	}

	public override string ToString()
	{
		return this.GetAutoName();
	}
}
