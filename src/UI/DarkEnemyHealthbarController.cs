using System;
using System.Collections.Generic;
using Characters;
using GameResources;
using Services;
using Singletons;
using TMPro;
using UnityEngine;

namespace UI;

public sealed class DarkEnemyHealthbarController : MonoBehaviour
{
	private struct Target
	{
		public Character character;

		public string abilityName;

		public Target(Character character, string abilityName)
		{
			this.character = character;
			this.abilityName = abilityName;
		}
	}

	[SerializeField]
	private CharacterHealthBar _healthBar;

	[SerializeField]
	private HangingPanelAnimator _animator;

	[SerializeField]
	private TextMeshProUGUI _name;

	[SerializeField]
	private TextMeshProUGUI _ability;

	private IDictionary<Character, string> _attached = new Dictionary<Character, string>();

	public IDictionary<Character, string> attached => _attached;

	public void Open(Character character, string abilityName)
	{
		_healthBar.Initialize(character);
		((TMP_Text)_name).text = Localization.GetLocalizedString($"enemy/name/{character.key}");
		((TMP_Text)_ability).text = abilityName;
		((Component)_animator).gameObject.SetActive(true);
		_animator.Appear();
		Character player = Singleton<Service>.Instance.levelManager.player;
		player.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(player.onGaveDamage, new GaveDamageDelegate(HandelOnGaveDamage));
		Character player2 = Singleton<Service>.Instance.levelManager.player;
		player2.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(player2.onGaveDamage, new GaveDamageDelegate(HandelOnGaveDamage));
	}

	public void AddTarget(Character character, string abilityName)
	{
		if (!_attached.ContainsKey(character))
		{
			_attached.Add(character, abilityName);
		}
		else
		{
			_attached[character] = abilityName;
		}
	}

	public void RemoveTarget(Character character)
	{
		_attached.Remove(character);
	}

	private void HandelOnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
	{
		Character character = target.character;
		if (!((Object)(object)character == (Object)null) && character.type == Character.Type.Named && !character.health.dead)
		{
			Set(character);
		}
	}

	public void Set(Character character)
	{
		((TMP_Text)_name).text = Localization.GetLocalizedString($"enemy/name/{character.key}");
		((TMP_Text)_ability).text = attached[character];
		_healthBar.Initialize(character);
	}

	public void Close()
	{
		if ((Object)(object)Singleton<Service>.Instance.levelManager.player != (Object)null)
		{
			Character player = Singleton<Service>.Instance.levelManager.player;
			player.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(player.onGaveDamage, new GaveDamageDelegate(HandelOnGaveDamage));
		}
		if (((Component)_healthBar).gameObject.activeSelf)
		{
			_animator.Disappear();
		}
	}
}
