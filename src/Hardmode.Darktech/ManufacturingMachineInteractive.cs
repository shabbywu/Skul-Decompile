using System.Collections;
using System.Collections.Generic;
using Characters;
using Characters.Gear;
using Characters.Operations;
using Characters.Operations.Fx;
using CutScenes;
using Data;
using GameResources;
using InControl;
using Runnables;
using Scenes;
using Services;
using Singletons;
using TMPro;
using UI;
using UnityEditor;
using UnityEngine;
using UserInput;

namespace Hardmode.Darktech;

public sealed class ManufacturingMachineInteractive : InteractiveObject
{
	private readonly int _deactiveHash = Animator.StringToHash("SMM_Deactivate");

	private readonly int _introHash = Animator.StringToHash("SMM_Intro");

	private readonly int _endHash = Animator.StringToHash("SMM_End");

	private readonly int _mode2Hash = Animator.StringToHash("SMM_Mode_2");

	private readonly int _intro7Hash = Animator.StringToHash("SMM2_Intro");

	private readonly int _end7Hash = Animator.StringToHash("SMM2_End");

	private readonly int _mode7_2Hash = Animator.StringToHash("SMM2_Mode_2");

	[SerializeField]
	private DarktechData.Type _darktechType;

	[SerializeField]
	private Gear.Type _type;

	[SerializeField]
	private int _selectCount;

	[SerializeField]
	private Animator _animator;

	[SerializeField]
	private Animator _displayAnimator;

	[SerializeField]
	private SpriteRenderer _display;

	[SerializeField]
	private Transform _spawnPoint;

	[SerializeField]
	private TMP_Text _priceText;

	[SerializeField]
	private GameObject _searchGuide;

	private List<GearReference> _gearList;

	private int _currentIndex;

	private int _remainSelectCount;

	private int _price;

	private GameObject _cacheUIObject;

	private GameObject[] _cacheUIObjects;

	[SerializeField]
	[Subcomponent(typeof(PlaySound))]
	private PlaySound _loopSound;

	[Subcomponent(typeof(PlaySound))]
	[SerializeField]
	private PlaySound _moveSound;

	[Subcomponent(typeof(PlaySound))]
	[SerializeField]
	private PlaySound _selectSound;

	[SerializeField]
	[Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _onSelect;

	[SerializeField]
	private Runnable _enhancedIntro;

	[SerializeField]
	private SpriteRenderer[] _displays;

	[Range(0f, 1f)]
	[SerializeField]
	private float _animationDuration = 0.3f;

	[SerializeField]
	private AnimationCurve _animationCurve;

	private const float shakeDuration = 0.08f;

	private const float shakeIntensity = 0.05f;

	private int _introAnimation;

	private int _endAnimation;

	private int _mode2Animation;

	private bool _running;

	private string name => Localization.GetLocalizedString(string.Format("darktech/equipment/{0}/{1}", _darktechType, "name"));

	private string body => Localization.GetLocalizedString($"darktech/equipment/{_darktechType}/body");

	private string keyGuide => Localization.GetLocalizedString("label/get");

	private bool isEnhanced
	{
		get
		{
			if (_type == Gear.Type.Weapon)
			{
				return GameData.HardmodeProgress.clearedLevel >= 7;
			}
			return false;
		}
	}

	private bool needEnhancedCutscene
	{
		get
		{
			if (_type == Gear.Type.Weapon && isEnhanced)
			{
				return !GameData.Progress.cutscene.GetData(CutScenes.Key.강화두개골제조기_Intro);
			}
			return false;
		}
	}

	private void Start()
	{
		_remainSelectCount = _selectCount;
		_price = ((_type == Gear.Type.Weapon) ? Singleton<DarktechManager>.Instance.setting.두개골제조기가격 : Singleton<DarktechManager>.Instance.setting.보급품제조기가격);
		_cacheUIObject = _uiObject;
		_cacheUIObjects = _uiObjects;
		_onSelect.Initialize();
		if (!Singleton<DarktechManager>.Instance.IsActivated(_darktechType))
		{
			_uiObject = _searchGuide;
			_uiObjects = (GameObject[])(object)new GameObject[0];
			_animator.Play(_deactiveHash, 0, 0f);
		}
		if (needEnhancedCutscene)
		{
			_uiObject = _searchGuide;
			_uiObjects = (GameObject[])(object)new GameObject[0];
			_animator.Play(_deactiveHash, 0, 0f);
		}
		_introAnimation = (isEnhanced ? _intro7Hash : _introHash);
		_endAnimation = (isEnhanced ? _end7Hash : _endHash);
		_mode2Animation = (isEnhanced ? _mode7_2Hash : _mode2Hash);
		((MonoBehaviour)this).StartCoroutine(CLoad());
	}

	private IEnumerator CLoad()
	{
		while (!Singleton<Service>.Instance.gearManager.initialized)
		{
			yield return null;
		}
		_gearList = new List<GearReference>(Singleton<Service>.Instance.gearManager.GetGearListByRarity(_type, (Rarity)0));
		if (isEnhanced && _type == Gear.Type.Weapon)
		{
			_gearList.AddRange(Singleton<Service>.Instance.gearManager.GetGearListByRarity(_type, (Rarity)1));
		}
		if (!needEnhancedCutscene && Singleton<DarktechManager>.Instance.IsActivated(_darktechType))
		{
			ActivateMachine();
		}
	}

	public void ActivateMachine()
	{
		Singleton<DarktechManager>.Instance.ActivateDarktech(_darktechType);
		_uiObject.gameObject.SetActive(false);
		_uiObject = _cacheUIObject;
		_uiObjects = _cacheUIObjects;
		_animator.Play(_introAnimation, 0, 0f);
		Load();
		_loopSound.Run(Singleton<Service>.Instance.levelManager.player);
	}

	public void ActiavateEnhanvedMachine()
	{
		_uiObject.gameObject.SetActive(false);
		_uiObject = _cacheUIObject;
		_uiObjects = _cacheUIObjects;
		SpriteRenderer[] displays = _displays;
		for (int i = 0; i < displays.Length; i++)
		{
			((Component)displays[i]).gameObject.SetActive(true);
		}
		Load();
		_loopSound.Run(Singleton<Service>.Instance.levelManager.player);
	}

	public void TempDisplayOff()
	{
		SpriteRenderer[] displays = _displays;
		for (int i = 0; i < displays.Length; i++)
		{
			((Component)displays[i]).gameObject.SetActive(false);
		}
	}

	public void TempDisplayOn()
	{
		SpriteRenderer[] displays = _displays;
		for (int i = 0; i < displays.Length; i++)
		{
			((Component)displays[i]).gameObject.SetActive(true);
		}
	}

	public override void InteractWith(Character character)
	{
		if (!Singleton<DarktechManager>.Instance.IsActivated(_darktechType))
		{
			return;
		}
		if (needEnhancedCutscene)
		{
			StartEnhancedIntro(character);
		}
		else if (_remainSelectCount >= _selectCount || GameData.Currency.darkQuartz.Has(_price))
		{
			if (_remainSelectCount > 0)
			{
				Select();
			}
			else
			{
				((MonoBehaviour)this).StartCoroutine(CTalk());
			}
		}
	}

	private void StartEnhancedIntro(Character character)
	{
		_enhancedIntro.Run();
	}

	private IEnumerator CTalk()
	{
		SystemDialogue ui = Scene<GameBase>.instance.uiManager.systemDialogue;
		yield return LetterBox.instance.CAppear();
		yield return ui.CShow(name, body);
		LetterBox.instance.Disappear();
	}

	public void Select()
	{
		if (!_running)
		{
			if (_remainSelectCount < _selectCount)
			{
				GameData.Currency.darkQuartz.Consume(_price);
			}
			_selectSound.Run(Singleton<Service>.Instance.levelManager.player);
			((MonoBehaviour)this).StartCoroutine(_onSelect.CRun(Singleton<Service>.Instance.levelManager.player));
			_remainSelectCount--;
			((MonoBehaviour)this).StartCoroutine(CDropGear());
			_gearList.RemoveAt(_currentIndex);
			Down();
			if (_remainSelectCount <= 0)
			{
				ClosePopup();
				_uiObject = _searchGuide;
				_uiObjects = (GameObject[])(object)new GameObject[0];
				_uiObject.gameObject.SetActive(true);
				OpenPopupBy(_character);
				_animator.Play(_endAnimation, 0, 0f);
				HideDisplay();
			}
			else if (_remainSelectCount == 1)
			{
				_animator.Play(_mode2Animation, 0, 0f);
			}
		}
	}

	private void HideDisplay()
	{
		SpriteRenderer[] displays = _displays;
		foreach (SpriteRenderer obj in displays)
		{
			obj.sprite = null;
			((Component)obj).gameObject.SetActive(false);
		}
		_display.sprite = null;
	}

	public override void OnDeactivate()
	{
		HideDisplay();
	}

	private IEnumerator CDropGear()
	{
		GearRequest request = _gearList[_currentIndex].LoadAsync();
		while (!request.isDone)
		{
			yield return null;
		}
		Singleton<Service>.Instance.levelManager.DropGear(request, _spawnPoint.position);
	}

	private void Update()
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		if (base.popupVisible && base.activated && Singleton<DarktechManager>.Instance.IsActivated(_darktechType) && !needEnhancedCutscene && _remainSelectCount > 0)
		{
			string arg = GameData.Currency.darkQuartz.colorCode;
			if (!GameData.Currency.darkQuartz.Has(_price))
			{
				arg = ColorUtility.ToHtmlStringRGB(Color.red);
			}
			if (_remainSelectCount < _selectCount)
			{
				_priceText.text = $"{keyGuide} (<color=#{arg}>{_price}</color>)";
			}
			else
			{
				_priceText.text = keyGuide;
			}
			if (((OneAxisInputControl)KeyMapper.Map.Up).WasPressed)
			{
				Up();
			}
			else if (((OneAxisInputControl)KeyMapper.Map.Down).WasPressed)
			{
				Down();
			}
		}
	}

	private void Load()
	{
		int index = (_currentIndex + 1) % _gearList.Count;
		int currentIndex = _currentIndex;
		int index2 = ((_currentIndex == 0) ? (_gearList.Count - 1) : (_currentIndex - 1));
		_displays[0].sprite = _gearList[index2].icon;
		_displays[1].sprite = _gearList[currentIndex].icon;
		_displays[2].sprite = _gearList[index].icon;
		((Renderer)_displays[0]).enabled = false;
		((Renderer)_displays[2]).enabled = false;
	}

	private void Up()
	{
		if (!_running)
		{
			_moveSound.Run(Singleton<Service>.Instance.levelManager.player);
			_currentIndex = (_currentIndex + 1) % _gearList.Count;
			((MonoBehaviour)this).StartCoroutine(CUp());
		}
	}

	private void Down()
	{
		if (!_running)
		{
			_moveSound.Run(Singleton<Service>.Instance.levelManager.player);
			_currentIndex--;
			if (_currentIndex < 0)
			{
				_currentIndex += _gearList.Count;
			}
			((MonoBehaviour)this).StartCoroutine(CDown());
		}
	}

	public void InitialGearSetting(int _startGearSetting)
	{
		_currentIndex = _startGearSetting;
		Load();
	}

	private IEnumerator CShake(Vector3 origin, Transform target)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		float elapsed = 0f;
		while (elapsed < 0.08f)
		{
			target.position = Vector2.op_Implicit(new Vector2(origin.x, origin.y + Random.Range(-0.05f, 0.05f)));
			elapsed += ((ChronometerBase)Chronometer.global).deltaTime;
			yield return null;
		}
		target.position = origin;
	}

	private IEnumerator CUp()
	{
		_running = true;
		float elapsed = 0f;
		Vector3 upPoint = ((Component)_displays[2]).transform.position;
		Vector3 fromMid = ((Component)_displays[1]).transform.position;
		Vector3 toDown = ((Component)_displays[0]).transform.position;
		Vector3 fromUp = ((Component)_displays[2]).transform.position;
		Vector3 toMid = ((Component)_displays[1]).transform.position;
		((Renderer)_displays[0]).enabled = true;
		((Renderer)_displays[2]).enabled = true;
		while (elapsed < _animationDuration)
		{
			((Component)_displays[1]).transform.position = Vector2.op_Implicit(Vector2.Lerp(Vector2.op_Implicit(fromMid), Vector2.op_Implicit(toDown), _animationCurve.Evaluate(elapsed / _animationDuration)));
			((Component)_displays[2]).transform.position = Vector2.op_Implicit(Vector2.Lerp(Vector2.op_Implicit(fromUp), Vector2.op_Implicit(toMid), _animationCurve.Evaluate(elapsed / _animationDuration)));
			elapsed += ((ChronometerBase)Chronometer.global).deltaTime;
			yield return null;
		}
		((Component)_displays[0]).transform.position = upPoint;
		((Component)_displays[1]).transform.position = toDown;
		((Component)_displays[2]).transform.position = toMid;
		yield return CShake(toMid, ((Component)_displays[2]).transform);
		SpriteRenderer val = _displays[0];
		SpriteRenderer val2 = _displays[1];
		SpriteRenderer val3 = _displays[2];
		_displays[0] = val2;
		_displays[1] = val3;
		_displays[2] = val;
		Load();
		_running = false;
	}

	private IEnumerator CDown()
	{
		_running = true;
		float elapsed = 0f;
		Vector3 downPoint = ((Component)_displays[0]).transform.position;
		Vector3 fromDown = ((Component)_displays[0]).transform.position;
		Vector3 toMid = ((Component)_displays[1]).transform.position;
		Vector3 fromMid = ((Component)_displays[1]).transform.position;
		Vector3 toUp = ((Component)_displays[2]).transform.position;
		((Renderer)_displays[0]).enabled = true;
		((Renderer)_displays[2]).enabled = true;
		while (elapsed < _animationDuration)
		{
			((Component)_displays[0]).transform.position = Vector2.op_Implicit(Vector2.Lerp(Vector2.op_Implicit(fromDown), Vector2.op_Implicit(toMid), _animationCurve.Evaluate(elapsed / _animationDuration)));
			((Component)_displays[1]).transform.position = Vector2.op_Implicit(Vector2.Lerp(Vector2.op_Implicit(fromMid), Vector2.op_Implicit(toUp), _animationCurve.Evaluate(elapsed / _animationDuration)));
			elapsed += ((ChronometerBase)Chronometer.global).deltaTime;
			yield return null;
		}
		((Component)_displays[0]).transform.position = toMid;
		((Component)_displays[1]).transform.position = toUp;
		((Component)_displays[2]).transform.position = downPoint;
		yield return CShake(toMid, ((Component)_displays[0]).transform);
		SpriteRenderer val = _displays[0];
		SpriteRenderer val2 = _displays[1];
		SpriteRenderer val3 = _displays[2];
		_displays[0] = val3;
		_displays[1] = val;
		_displays[2] = val2;
		Load();
		_running = false;
	}
}
