using System.Collections;
using System.Linq;
using Characters;
using Characters.Abilities.Weapons;
using Characters.Gear.Weapons;
using Data;
using FX;
using Services;
using Singletons;
using UI;
using UnityEngine;

namespace Level.Npc.FieldNpcs;

public class DarkPriest : FieldNpc
{
	[SerializeField]
	private NpcType _npcType;

	[Space]
	[SerializeField]
	private EffectInfo _frontEffect;

	[SerializeField]
	private EffectInfo _behindEffect;

	[SerializeField]
	private SoundInfo _sound;

	[SerializeField]
	private float _effectShowingDuration;

	[SerializeField]
	private SkillChangingEffect _skillChangingEffect;

	private int _goldCostIndex;

	protected override NpcType _type => _npcType;

	private int[] _goldCosts => Singleton<Service>.Instance.levelManager.currentChapter.currentStage.fieldNpcSettings.darkPriestGoldCosts;

	private int _goldCost => _goldCosts[_goldCostIndex];

	protected override void Interact(Character character)
	{
		base.Interact(character);
		((MonoBehaviour)this).StartCoroutine(CGreetingAndConfirm(character, _goldCost));
	}

	private IEnumerator CGreetingAndConfirm(Character character, object confirmArg = null)
	{
		yield return LetterBox.instance.CAppear();
		string[] scripts = ((_phase == Phase.Initial) ? base._greeting : base._regreeting);
		_phase = Phase.Greeted;
		_npcConversation.skippable = true;
		int lastIndex = scripts.Length - 1;
		for (int i = 0; i < lastIndex; i++)
		{
			yield return _npcConversation.CConversation(scripts[i]);
		}
		_npcConversation.skippable = true;
		_npcConversation.body = ((confirmArg == null) ? scripts[lastIndex] : string.Format(scripts[lastIndex], confirmArg));
		_npcConversation.OpenCurrencyBalancePanel(GameData.Currency.Type.Gold);
		yield return _npcConversation.CType();
		yield return (object)new WaitForSecondsRealtime(0.3f);
		_npcConversation.OpenConfirmSelector(delegate
		{
			OnConfirmed(character);
		}, base.Close);
	}

	private void OnConfirmed(Character character)
	{
		_npcConversation.CloseCurrencyBalancePanel();
		if (GameData.Currency.gold.Consume(_goldCost))
		{
			if (_goldCostIndex < _goldCosts.Length - 1)
			{
				_goldCostIndex++;
			}
			((MonoBehaviour)this).StartCoroutine(CRerollSkills());
		}
		else
		{
			((MonoBehaviour)this).StartCoroutine(CNoMoneyAndClose());
		}
		IEnumerator CNoMoneyAndClose()
		{
			_npcConversation.skippable = true;
			yield return _npcConversation.CConversation(base._noMoney);
			LetterBox.instance.Disappear();
		}
		IEnumerator CRerollSkills()
		{
			Weapon current = character.playerComponents.inventory.weapon.current;
			Sprite[] oldSkills = current.currentSkills.Select((SkillInfo skill) => skill.cachedIcon).ToArray();
			if (((Object)current).name.Equals("Prisoner_2"))
			{
				int level = ((Component)current.currentSkills[0]).GetComponent<PrisonerSkill>().level;
				int level2 = ((Component)current.currentSkills[1]).GetComponent<PrisonerSkill>().level;
				current.RerollSkills();
				SkillInfo value = ((Component)current.currentSkills[0]).GetComponent<PrisonerSkill>().parent.skillInfos[level];
				SkillInfo value2 = ((Component)current.currentSkills[1]).GetComponent<PrisonerSkill>().parent.skillInfos[level2];
				current.currentSkills[0] = value;
				current.currentSkills[1] = value2;
				current.SetSkillButtons();
			}
			else
			{
				current.RerollSkills();
			}
			Sprite[] newSkills = current.currentSkills.Select((SkillInfo skill) => skill.cachedIcon).ToArray();
			_npcConversation.portrait = null;
			_npcConversation.visible = false;
			_frontEffect.Spawn(((Component)character).transform.position);
			_behindEffect.Spawn(((Component)character).transform.position);
			PersistentSingleton<SoundManager>.Instance.PlaySound(_sound, ((Component)this).transform.position);
			((Component)_skillChangingEffect).transform.position = ((Component)character).transform.position + Vector3.up;
			_skillChangingEffect.Play(oldSkills, newSkills);
			yield return (object)new WaitForSeconds(_effectShowingDuration);
			LetterBox.instance.Disappear();
		}
	}
}
