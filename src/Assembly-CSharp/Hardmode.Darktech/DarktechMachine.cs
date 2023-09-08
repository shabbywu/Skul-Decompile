using System.Collections;
using Characters;
using Characters.Operations.Fx;
using GameResources;
using Scenes;
using Services;
using Singletons;
using UI;
using UnityEditor;
using UnityEngine;

namespace Hardmode.Darktech;

public sealed class DarktechMachine : InteractiveObject
{
	private readonly string _introName = "Intro";

	private readonly int _introHash = Animator.StringToHash("Intro");

	[SerializeField]
	private DarktechData _darktech;

	[SerializeField]
	private Animator _animator;

	[SerializeField]
	[Subcomponent(typeof(PlaySound))]
	private PlaySound _loopSound;

	private bool _introWaiting;

	private string name => Localization.GetLocalizedString(string.Format("darktech/equipment/{0}/{1}", _darktech.type, "name"));

	private string body => Localization.GetLocalizedString($"darktech/equipment/{_darktech.type}/body");

	private void Start()
	{
		if (!Singleton<DarktechManager>.Instance.IsUnlocked(_darktech.type))
		{
			((Component)this).gameObject.SetActive(false);
		}
		if (Singleton<DarktechManager>.Instance.IsActivated(_darktech.type))
		{
			ActivateMachine();
		}
	}

	private void Update()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
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
		Singleton<DarktechManager>.Instance.ActivateDarktech(_darktech.type);
		_introWaiting = true;
	}

	public override void InteractWith(Character character)
	{
		if (!Singleton<DarktechManager>.Instance.IsActivated(_darktech.type))
		{
			ActivateMachine();
		}
		else
		{
			((MonoBehaviour)this).StartCoroutine(CTalk());
		}
	}

	private IEnumerator CTalk()
	{
		SystemDialogue ui = Scene<GameBase>.instance.uiManager.systemDialogue;
		yield return LetterBox.instance.CAppear();
		yield return ui.CShow(name, body);
		LetterBox.instance.Disappear();
	}
}
