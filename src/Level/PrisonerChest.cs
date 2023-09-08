using System;
using System.Collections;
using System.Linq;
using Characters;
using Characters.Abilities.Weapons;
using Characters.Gear.Weapons;
using Data;
using FX;
using GameResources;
using Runnables;
using Runnables.Triggers;
using Services;
using Singletons;
using UnityEngine;

namespace Level;

public class PrisonerChest : InteractiveObject
{
	private const int _randomSeed = 1177618293;

	private const string _cursedChestTextKey = "theKing/cursedChest/line/idle";

	[Space]
	[SerializeField]
	private SoundInfo _encounterSound;

	private bool _encounterSoundPlayed;

	[Header("LineText")]
	[SerializeField]
	private LineText _lineText;

	[SerializeField]
	private Transform _lineTextPosition;

	[GetComponent]
	[SerializeField]
	private Animator _animator;

	[SerializeField]
	private PrisonerSkillScroll _skillScroll;

	[SerializeField]
	[Space]
	private DroppedCell _cellPrefab;

	[SerializeField]
	private CustomFloat _cellCount;

	[SerializeField]
	[Header("Grade Weights (단위 : 0.5,  순서÷2가 등급보너스임)")]
	[Range(0f, 100f)]
	private int[] _weights;

	[SerializeField]
	[Space]
	private EffectInfo _spawnEffect;

	[Trigger.Subcomponent]
	[SerializeField]
	private Trigger _activationTrigger;

	[SerializeField]
	private Runnable _activateCutscene;

	private Weapon _weapon;

	private PrisonerSkillInfosByGrade _skills;

	private SkillInfo _skillInfo;

	private CoroutineReference _lineTextCoroutineReference;

	public Weapon weapon => _weapon;

	public PrisonerSkillInfosByGrade skills => _skills;

	public SkillInfo skillInfo => _skillInfo;

	protected override void Awake()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		base.Awake();
		_spawnEffect.Spawn(((Component)this).transform.position);
	}

	public void SetSkillInfo(Weapon weapon, PrisonerSkillInfosByGrade skills, SkillInfo skillInfo)
	{
		_weapon = weapon;
		_skills = skills;
		_skillInfo = skillInfo;
	}

	public int GetGradeBonus()
	{
		Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
		double num = new Random(GameData.Save.instance.randomSeed + 1177618293 + (int)currentChapter.type * 256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex).NextDouble() * (double)_weights.Sum();
		for (int i = 0; i < _weights.Length; i++)
		{
			num -= (double)_weights[i];
			if (num <= 0.0)
			{
				return i;
			}
		}
		return 0;
	}

	private void Start()
	{
		_lineTextCoroutineReference = ((MonoBehaviour)(object)this).StartCoroutineWithReference(CStartLineText());
	}

	public override void OnActivate()
	{
		base.OnActivate();
		_animator.Play(InteractiveObject._activateHash);
	}

	public override void OnDeactivate()
	{
		base.OnDeactivate();
		_animator.Play(InteractiveObject._deactivateHash);
	}

	public override void OpenPopupBy(Character character)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		if (_activationTrigger.IsSatisfied())
		{
			if (!_encounterSoundPlayed)
			{
				_encounterSoundPlayed = true;
				PersistentSingleton<SoundManager>.Instance.PlaySound(_encounterSound, ((Component)this).transform.position);
			}
			base.OpenPopupBy(character);
		}
	}

	public override void InteractWithByPressing(Character character)
	{
		if (_activationTrigger.IsSatisfied())
		{
			_lineTextCoroutineReference.Stop();
			character.status.RemoveStun();
			_activateCutscene.Run();
			Deactivate();
		}
	}

	private IEnumerator CStartLineText()
	{
		if ((Object)(object)_lineText == (Object)null || (Object)(object)_lineTextPosition == (Object)null)
		{
			yield break;
		}
		string[] texts = Localization.GetLocalizedStringArray("theKing/cursedChest/line/idle");
		if (texts.Length == 0)
		{
			yield break;
		}
		while (true)
		{
			yield return Chronometer.global.WaitForSeconds(Random.Range(5, 10));
			string text = texts.Random();
			((Component)_lineText).transform.position = _lineTextPosition.position;
			_lineText.Display(text, 2f);
		}
	}
}
