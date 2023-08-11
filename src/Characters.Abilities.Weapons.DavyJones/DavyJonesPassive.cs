using System;
using System.Collections.Generic;
using System.Text;
using Characters.Actions;
using Characters.Gear.Weapons;
using Characters.Operations;
using Data;
using Scenes;
using UI.Hud;
using UnityEngine;

namespace Characters.Abilities.Weapons.DavyJones;

[Serializable]
public sealed class DavyJonesPassive : Ability, IDavyJonesCannonBallSave, IDavyJonesCannonBallCollection
{
	public class Instance : AbilityInstance<DavyJonesPassive>
	{
		public override Sprite icon
		{
			get
			{
				if (iconStacks != 0)
				{
					return base.icon;
				}
				return null;
			}
		}

		public override int iconStacks => ability._reloadCount - 1;

		public Instance(Character owner, DavyJonesPassive ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			Scene<GameBase>.instance.uiManager.headupDisplay.davyJonesHud.ShowHUD();
			ability.UpdateHUD();
			PushSwapBonusCannonBall();
		}

		protected override void OnDetach()
		{
			Scene<GameBase>.instance.uiManager.headupDisplay.davyJonesHud.HideAll();
		}

		private void PushSwapBonusCannonBall()
		{
			if (ability._magazine.Count <= 2)
			{
				ability.Push(CannonBallType.Normal, 2);
			}
		}
	}

	[Serializable]
	private struct SkillInfoCannonBallMap
	{
		[SerializeField]
		internal CannonBallType _cannonBallType;

		[SerializeField]
		internal SkillInfo _skillInfo;
	}

	[SerializeField]
	private GameObject _emptyMagazineSign;

	[SerializeField]
	private int _enhanceCycle = 4;

	[SerializeField]
	private Weapon _weapon;

	[SerializeField]
	private Characters.Actions.Action[] _cannonBallActions;

	[SerializeField]
	private SkillInfoCannonBallMap[] _skillInfoCannonBallMaps;

	[CharacterOperation.Subcomponent]
	[SerializeField]
	[Header("강화 관련")]
	private CharacterOperation.Subcomponents _onBeforeEnhanceReload;

	[CharacterOperation.Subcomponent]
	[SerializeField]
	private CharacterOperation.Subcomponents _onEnhanceReload;

	[Header("탄창 관련")]
	[SerializeField]
	[CharacterOperation.Subcomponent]
	private CharacterOperation.Subcomponents _onMagazineCount0;

	[SerializeField]
	[CharacterOperation.Subcomponent]
	private CharacterOperation.Subcomponents _onMagazineCount1;

	[SerializeField]
	[CharacterOperation.Subcomponent]
	private CharacterOperation.Subcomponents _onMagazineCount2or3;

	private Character _owner;

	private int _reloadCount;

	private const int _maxCount = 4;

	private Queue<CannonBallType> _magazine;

	public bool isEmpty => _magazine.Count == 0;

	public CannonBallType? Top()
	{
		if (_magazine.Count == 0)
		{
			return null;
		}
		return _magazine.Peek();
	}

	public void Push(CannonBallType cannon, int count)
	{
		bool flag = Convert.ToInt32(cannon) % 2 == 1;
		if (_reloadCount == _enhanceCycle - 1)
		{
			_onBeforeEnhanceReload.Run(_owner);
		}
		if (_reloadCount >= _enhanceCycle && !flag)
		{
			_onEnhanceReload.Run(_owner);
			for (int i = 0; i < count; i++)
			{
				Push(Convert.ToInt32(cannon) + 1);
			}
			_reloadCount = 1;
		}
		else
		{
			for (int j = 0; j < count; j++)
			{
				Push(cannon);
			}
			_reloadCount++;
		}
	}

	private void Push(CannonBallType cannon)
	{
		if (_magazine.Count >= 4)
		{
			_magazine.Dequeue();
		}
		_magazine.Enqueue(cannon);
		_emptyMagazineSign.SetActive(false);
		UpdateHUD();
	}

	private void Push(int index)
	{
		foreach (CannonBallType value in Enum.GetValues(typeof(CannonBallType)))
		{
			if (Convert.ToInt32(value) == index)
			{
				Push(value);
				break;
			}
		}
	}

	public void Pop()
	{
		if (_magazine.Count > 0)
		{
			_magazine.Dequeue();
			switch (_magazine.Count)
			{
			case 0:
				_emptyMagazineSign.SetActive(true);
				_onMagazineCount0.Run(_owner);
				break;
			case 1:
				_onMagazineCount1.Run(_owner);
				break;
			default:
				_onMagazineCount2or3.Run(_owner);
				break;
			}
		}
		UpdateHUD();
	}

	private void UpdateHUD()
	{
		DavyJonesHud davyJonesHud = Scene<GameBase>.instance.uiManager.headupDisplay.davyJonesHud;
		davyJonesHud.HideAllCannonBall();
		int num = 0;
		foreach (CannonBallType item in _magazine)
		{
			davyJonesHud.SetCannonBall(num, item);
			num++;
		}
	}

	public override void Initialize()
	{
		base.Initialize();
		_magazine = new Queue<CannonBallType>(4);
		GameData.Save instance = GameData.Save.instance;
		if (instance.currentWeapon.Equals(((Object)_weapon).name) || instance.nextWeapon.Equals(((Object)_weapon).name))
		{
			return;
		}
		Push(CannonBallType.Normal, 2);
		for (int i = 0; i < _weapon.currentSkills.Count; i++)
		{
			SkillInfo skillInfo = _weapon.currentSkills[i];
			for (int j = 0; j < _skillInfoCannonBallMaps.Length; j++)
			{
				if (_skillInfoCannonBallMaps[j]._skillInfo.key == skillInfo.key)
				{
					Push(_skillInfoCannonBallMaps[j]._cannonBallType);
					break;
				}
			}
		}
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		_owner = owner;
		return new Instance(owner, this);
	}

	public float MakeSaveData()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(_magazine.Count);
		CannonBallType[] array = _magazine.ToArray();
		for (int i = 0; i < 4; i++)
		{
			if (i < array.Length)
			{
				stringBuilder.Append(Convert.ToInt32(array[i]));
			}
			else
			{
				stringBuilder.Append(0);
			}
		}
		return Convert.ToInt32(stringBuilder.ToString());
	}

	public void Load(float data)
	{
		string text = Convert.ToString((int)data);
		Debug.Log((object)text);
	}
}
