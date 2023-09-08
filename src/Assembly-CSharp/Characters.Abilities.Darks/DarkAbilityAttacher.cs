using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Darks;

public sealed class DarkAbilityAttacher : MonoBehaviour
{
	[Subcomponent(typeof(DarkAbilityContainer))]
	[SerializeField]
	private DarkAbilityContainer _darkAbilityContainer;

	private Character _owner;

	private ICollection<DarkAbility> _abilities;

	private bool _initialized;

	private bool _attached;

	private string _cachedDisplayName;

	public bool attached => _attached;

	public DarkAbilityGauge gauge { get; set; }

	public string displayName
	{
		get
		{
			if (!string.IsNullOrEmpty(_cachedDisplayName))
			{
				return _cachedDisplayName;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (DarkAbility ability in _abilities)
			{
				stringBuilder.Append(ability.displayName);
			}
			_cachedDisplayName = stringBuilder.ToString();
			return _cachedDisplayName;
		}
	}

	public void Initialize(Character owner)
	{
		_owner = owner;
		_darkAbilityContainer.Initialize(owner);
		_initialized = true;
	}

	public void StartAttach()
	{
		((MonoBehaviour)this).StartCoroutine(CWaitForInitialize());
	}

	public void Attach()
	{
		_abilities = _darkAbilityContainer.GetDarkAbility();
		foreach (DarkAbility ability in _abilities)
		{
			ability.AttachTo(_owner, this);
		}
		_attached = true;
	}

	public void Detach()
	{
		foreach (DarkAbility ability in _abilities)
		{
			ability.RemoveFrom(_owner);
		}
	}

	private IEnumerator CWaitForInitialize()
	{
		while (!_initialized)
		{
			yield return null;
		}
		Attach();
	}
}
