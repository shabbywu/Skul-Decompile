using System.Collections;
using UnityEngine;

namespace Characters.Abilities;

public sealed class ShieldAttacher : AbilityAttacher
{
	private enum Type
	{
		GreaterThanOrEqual,
		LessThan
	}

	[SerializeField]
	private Type _type;

	[SerializeField]
	[Range(0f, 100f)]
	private int _shieldAmount;

	[SerializeField]
	private float _checkInterval = 0.1f;

	[SerializeField]
	[AbilityComponent.Subcomponent]
	private AbilityComponent _abilityComponent;

	private bool _attached;

	private CoroutineReference _checkReference;

	public override void OnIntialize()
	{
		_abilityComponent.Initialize();
	}

	public override void StartAttach()
	{
		_checkReference = ((MonoBehaviour)(object)this).StartCoroutineWithReference(CCheck());
	}

	public override void StopAttach()
	{
		if (!((Object)(object)base.owner == (Object)null))
		{
			_checkReference.Stop();
			base.owner.ability.Remove(_abilityComponent.ability);
		}
	}

	private IEnumerator CCheck()
	{
		while (true)
		{
			yield return Chronometer.global.WaitForSeconds(_checkInterval);
			Check();
		}
	}

	private void Check()
	{
		if (!base.owner.health.shield.hasAny)
		{
			Detach();
		}
		else if ((_type == Type.GreaterThanOrEqual && base.owner.health.shield.amount >= (double)_shieldAmount) || (_type == Type.LessThan && base.owner.health.shield.amount < (double)_shieldAmount))
		{
			Attach();
		}
		else
		{
			Detach();
		}
	}

	private void Attach()
	{
		if (!_attached)
		{
			_attached = true;
			base.owner.ability.Add(_abilityComponent.ability);
		}
	}

	private void Detach()
	{
		if (_attached)
		{
			_attached = false;
			base.owner.ability.Remove(_abilityComponent.ability);
		}
	}

	public override string ToString()
	{
		return this.GetAutoName();
	}
}
