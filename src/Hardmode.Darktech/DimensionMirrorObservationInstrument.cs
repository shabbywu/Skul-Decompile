using System.Collections;
using Characters;
using Characters.Gear.Weapons;
using Characters.Operations;
using Characters.Operations.Fx;
using Data;
using GameResources;
using Runnables;
using Scenes;
using Services;
using Singletons;
using UI;
using UnityEditor;
using UnityEngine;

namespace Hardmode.Darktech;

public sealed class DimensionMirrorObservationInstrument : InteractiveObject
{
	private readonly string _introName = "Intro";

	private readonly int _introHash = Animator.StringToHash("Intro");

	private readonly string _skulName = "Skul";

	private readonly string _heroSkulName = "HeroSkul";

	[SerializeField]
	private Animator _animator;

	[SerializeField]
	private Weapon _weapon;

	[Subcomponent(typeof(OperationInfo))]
	[SerializeField]
	private OperationInfo.Subcomponents _onChange;

	[SerializeField]
	private Runnable _onChangetoHero;

	[SerializeField]
	[Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _onHeroToSkul;

	private DarktechData.Type _type = DarktechData.Type.ObservationInstrument;

	[Subcomponent(typeof(PlaySound))]
	[SerializeField]
	private PlaySound _loopSound;

	private bool _introWaiting;

	private string name => Localization.GetLocalizedString(string.Format("darktech/equipment/{0}/{1}", _type, "name"));

	private string body => Localization.GetLocalizedString($"darktech/equipment/{_type}/body");

	private void Start()
	{
		_onChange.Initialize();
		_onHeroToSkul.Initialize();
		if (!Singleton<DarktechManager>.Instance.IsUnlocked(_type))
		{
			((Component)this).gameObject.SetActive(false);
		}
		if (Singleton<DarktechManager>.Instance.IsActivated(_type))
		{
			ActivateMachine();
		}
	}

	private void Update()
	{
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)_character == (Object)null)
		{
			return;
		}
		Character player = Singleton<Service>.Instance.levelManager.player;
		if (((Object)player.playerComponents.inventory.weapon.current).name.Equals(_skulName) || ((Object)player.playerComponents.inventory.weapon.current).name.Equals(_heroSkulName))
		{
			_uiObject.SetActive(true);
		}
		else
		{
			_uiObject.SetActive(false);
		}
		if (!_introWaiting)
		{
			return;
		}
		AnimatorStateInfo currentAnimatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
		if (((AnimatorStateInfo)(ref currentAnimatorStateInfo)).IsName(_introName))
		{
			currentAnimatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
			if (((AnimatorStateInfo)(ref currentAnimatorStateInfo)).normalizedTime > 0f)
			{
				_loopSound.Run(Singleton<Service>.Instance.levelManager.player);
				_introWaiting = false;
			}
		}
	}

	public void ActivateMachine()
	{
		_animator.Play(_introHash, 0, 0f);
		Singleton<DarktechManager>.Instance.ActivateDarktech(_type);
		_introWaiting = true;
	}

	public override void InteractWith(Character character)
	{
		Character player = Singleton<Service>.Instance.levelManager.player;
		if (((Object)player.playerComponents.inventory.weapon.current).name.Equals(_skulName) || ((Object)player.playerComponents.inventory.weapon.current).name.Equals(_heroSkulName))
		{
			if (!Singleton<DarktechManager>.Instance.IsActivated(DarktechData.Type.ObservationInstrument))
			{
				_animator.Play(_introHash, 0, 0f);
				Singleton<DarktechManager>.Instance.ActivateDarktech(_type);
				((MonoBehaviour)this).StartCoroutine(CTalk());
			}
			else
			{
				((MonoBehaviour)this).StartCoroutine(CTalk());
			}
		}
	}

	private IEnumerator CTalk()
	{
		SystemDialogue ui = Scene<GameBase>.instance.uiManager.systemDialogue;
		yield return LetterBox.instance.CAppear();
		yield return ui.CShow(name, body);
		Character player = Singleton<Service>.Instance.levelManager.player;
		((MonoBehaviour)this).StartCoroutine(_onChange.CRun(player));
		if (GameData.Generic.skinIndex == 1)
		{
			LetterBox.instance.Disappear();
			GameData.Generic.skinIndex = 0;
			Singleton<Service>.Instance.levelManager.skulSpawnAnimaionEnable = false;
			((MonoBehaviour)this).StartCoroutine(_onHeroToSkul.CRun(player));
			player.playerComponents.inventory.weapon.UpdateSkin();
		}
		else
		{
			GameData.Generic.skinIndex = 1;
			_onChangetoHero.Run();
		}
		GameData.Generic.SaveSkin();
	}
}
