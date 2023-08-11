using System.Collections;
using System.Collections.Generic;
using Characters;
using Data;
using GameResources;
using Scenes;
using Services;
using Singletons;
using UI;
using UnityEngine;

namespace Level.Npc.FieldNpcs;

public abstract class FieldNpc : InteractiveObject
{
	protected enum Phase
	{
		Initial,
		Greeted,
		Gave
	}

	public delegate void OnReleaseDelegate();

	public delegate void OnCageDestroyedDelegate();

	protected static readonly int _idleHash = Animator.StringToHash("Idle");

	protected static readonly int _idleCageHash = Animator.StringToHash("Idle_Cage");

	[SerializeField]
	private Sprite _portrait;

	[SerializeField]
	protected Animator _animator;

	[SerializeField]
	private Collider2D _interactRange;

	[SerializeField]
	private PressingButton _releasePressingButton;

	[SerializeField]
	private GameObject _talkUiObject;

	protected Phase _phase;

	protected NpcConversation _npcConversation;

	private bool _release;

	public bool encountered
	{
		get
		{
			return GameData.Progress.fieldNpcEncountered.GetData(_type);
		}
		set
		{
			GameData.Progress.fieldNpcEncountered.SetData(_type, value);
		}
	}

	protected abstract NpcType _type { get; }

	public NpcType type => _type;

	protected string _displayName => Localization.GetLocalizedString($"npc/{_type}/name");

	protected string[] _greeting => Localization.GetLocalizedStringArray($"npc/{_type}/greeting");

	protected string[] _regreeting => Localization.GetLocalizedStringArray($"npc/{_type}/regreeting");

	protected string[] _confirmed => Localization.GetLocalizedStringArray($"npc/{_type}/confirmed");

	protected string[] _noMoney => Localization.GetLocalizedStringArray($"npc/{_type}/noMoney");

	protected string[] _chat => ExtensionMethods.Random<string[]>((IEnumerable<string[]>)Localization.GetLocalizedStringArrays($"npc/{_type}/chat"));

	public string cageDestroyedLine => Localization.GetLocalizedString($"npc/{_type}/line/resqued");

	public string normalLine => ExtensionMethods.Random<string>((IEnumerable<string>)Localization.GetLocalizedStringArray($"npc/{_type}/line"));

	public bool release => _release;

	public event OnReleaseDelegate onRelease;

	public event OnCageDestroyedDelegate onCageDestroyed;

	protected override void Awake()
	{
		base.Awake();
		_npcConversation = Scene<GameBase>.instance.uiManager.npcConversation;
		_npcConversation.name = _displayName;
		_npcConversation.skippable = true;
		_npcConversation.portrait = _portrait;
		_animator.Play(_idleCageHash, 0, 0f);
		((Behaviour)_interactRange).enabled = false;
	}

	public void SetCage(Cage cage)
	{
		cage.onDestroyed += OnCageDestroyed;
	}

	protected void OnCageDestroyed()
	{
		((Behaviour)_interactRange).enabled = true;
		this.onCageDestroyed?.Invoke();
	}

	public void Flip()
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		((Component)_animator).transform.localScale = new Vector3(-1f, 1f, 1f);
	}

	private void Release()
	{
		encountered = true;
		_release = true;
		_animator.Play(_idleHash, 0, 0f);
		_uiObject.SetActive(false);
		_talkUiObject.SetActive(true);
		_uiObject = _talkUiObject;
		this.onRelease?.Invoke();
	}

	protected virtual void Start()
	{
		Singleton<Service>.Instance.levelManager.player.health.onTookDamage += StopConversation;
	}

	protected virtual void OnDestroy()
	{
		if (!Service.quitting)
		{
			Singleton<Service>.Instance.levelManager.player.health.onTookDamage -= StopConversation;
			Close();
		}
	}

	private void StopConversation(in Damage originalDamage, in Damage tookDamage, double damageDealt)
	{
		_releasePressingButton.StopPressing();
		((MonoBehaviour)this).StopAllCoroutines();
		Close();
		if (_release)
		{
			OnStopConversation();
		}
	}

	protected virtual void OnStopConversation()
	{
	}

	protected void Close()
	{
		_npcConversation.portrait = null;
		_npcConversation.visible = false;
		LetterBox.instance.Disappear();
	}

	public override void InteractWithByPressing(Character character)
	{
		Release();
		Interact(character);
	}

	public override void InteractWith(Character character)
	{
		if (_release)
		{
			Interact(character);
		}
	}

	protected virtual void Interact(Character character)
	{
		_npcConversation.name = _displayName;
		_npcConversation.portrait = _portrait;
	}

	protected IEnumerator CGreeting()
	{
		_npcConversation.skippable = true;
		yield return _npcConversation.CConversation(_greeting);
	}

	protected IEnumerator CChat()
	{
		yield return LetterBox.instance.CAppear();
		_npcConversation.skippable = true;
		yield return _npcConversation.CConversation(_chat);
		LetterBox.instance.Disappear();
	}
}
