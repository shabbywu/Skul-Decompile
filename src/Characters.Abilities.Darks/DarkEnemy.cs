using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Operations;
using Data;
using FX;
using GameResources;
using Level;
using Scenes;
using Services;
using Singletons;
using UI.Hud;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Darks;

public sealed class DarkEnemy : MonoBehaviour
{
	[SerializeField]
	private Stat.Values _bonusStatValues;

	[SerializeField]
	private Character _character;

	[SerializeField]
	private CharacterDieEffect _characterDieEffect;

	[SerializeField]
	[Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _introOperations;

	private Stat.Values _baseStatValues = new Stat.Values(new Stat.Value(Stat.Category.Percent, Stat.Kind.StoppingResistance, -1.0));

	private DarkAbilityAttacher _attacher;

	private const string minimapColorString = "#68009F";

	private Color _minimapColor;

	private void OnEnable()
	{
		ColorUtility.TryParseHtmlString("#68009F", ref _minimapColor);
		_character.stat.AttachValues(_baseStatValues);
		_character.stat.AttachValues(_bonusStatValues);
		_introOperations.Initialize();
		_character.cinematic.Attach((object)this);
		_characterDieEffect.particleInfo = null;
		_characterDieEffect.effect = null;
		_character.health.onDiedTryCatch += HandleOnDied;
		GameObject attach = _character.attach;
		if (!((Object)(object)attach == (Object)null))
		{
			LineText componentInChildren = attach.GetComponentInChildren<LineText>();
			if (!((Object)(object)componentInChildren == (Object)null))
			{
				Object.Destroy((Object)(object)((Component)((Component)componentInChildren).transform.parent).gameObject);
			}
		}
	}

	private void HandleOnDied()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		Singleton<Service>.Instance.levelManager.DropCurrency(GameData.Currency.Type.HeartQuartz, 1, 1, Vector2.op_Implicit(MMMaths.RandomPointWithinBounds(((Collider2D)_character.collider).bounds)));
		if (DarkEnemySelector.instance.dieEffects != null)
		{
			EffectInfo[] dieEffects = DarkEnemySelector.instance.dieEffects;
			for (int i = 0; i < dieEffects.Length; i++)
			{
				dieEffects[i]?.Spawn(((Component)_character).transform.position);
			}
		}
		if (DarkEnemySelector.instance.dieSound != null)
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(DarkEnemySelector.instance.dieSound, ((Component)_character).transform.position);
		}
	}

	private void OnDestroy()
	{
		_character.stat.DetachValues(_baseStatValues);
		_character.stat.DetachValues(_bonusStatValues);
		_character.health.onDiedTryCatch -= HandleOnDied;
		if (!Service.quitting)
		{
			Scene<GameBase>.instance.uiManager.headupDisplay.darkEnemyHealthBar.Close();
			Scene<GameBase>.instance.uiManager.headupDisplay.darkEnemyHealthBar.RemoveTarget(_character);
		}
	}

	public void Initialize(DarkAbilityAttacher attacher)
	{
		((Behaviour)this).enabled = true;
		_attacher = attacher;
	}

	private IEnumerator CRunIntroVisualEffect()
	{
		MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
		SpriteRenderer renderer = _character.spriteEffectStack.mainRenderer;
		int id = Shader.PropertyToID("_DissolveLevels");
		float elapsed = 0f;
		float duration = 1f;
		if (DarkEnemySelector.instance.introSound != null)
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(DarkEnemySelector.instance.introSound, ((Component)_character).transform.position);
		}
		for (; elapsed < duration; elapsed += ((ChronometerBase)_character.chronometer.master).deltaTime)
		{
			((Renderer)renderer).GetPropertyBlock(propertyBlock);
			propertyBlock.SetFloat(id, 1f - elapsed / duration);
			((Renderer)renderer).SetPropertyBlock(propertyBlock);
			yield return null;
		}
		((Renderer)renderer).GetPropertyBlock(propertyBlock);
		propertyBlock.SetFloat(id, 0f);
		((Renderer)renderer).SetPropertyBlock(propertyBlock);
	}

	public void RunIntro()
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		_character.cinematic.Detach((object)this);
		((Renderer)_character.spriteEffectStack.mainRenderer).material = MaterialResource.darkEnemy;
		if (DarkEnemySelector.instance.introEffects != null)
		{
			EffectInfo[] introEffects = DarkEnemySelector.instance.introEffects;
			for (int i = 0; i < introEffects.Length; i++)
			{
				introEffects[i]?.Spawn(((Component)_character).transform.position);
			}
		}
		((MonoBehaviour)this).StartCoroutine(CRunIntroVisualEffect());
		_attacher.StartAttach();
		if ((Object)(object)_character.attach != (Object)null)
		{
			ChangeMinimapAgent();
			CharacterHealthBarAttacher component = ((Component)_character).GetComponent<CharacterHealthBarAttacher>();
			if ((Object)(object)component != (Object)null)
			{
				component.SetHealthBar(DarkEnemySelector.instance.healthbar);
				DarkAbilityGaugeBar componentInChildren = ((Component)component).GetComponentInChildren<DarkAbilityGaugeBar>();
				if ((Object)(object)componentInChildren != (Object)null)
				{
					componentInChildren.Initialize(_attacher);
				}
			}
		}
		OpenHUDHealthBar();
		((MonoBehaviour)this).StartCoroutine(_introOperations.CRun(_character));
	}

	private void ChangeMinimapAgent()
	{
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		string value = "Minimap Agent";
		int num = -1;
		for (int i = 0; i < _character.attach.transform.childCount; i++)
		{
			if (((Object)_character.attach.transform.GetChild(i)).name.Equals(value, StringComparison.OrdinalIgnoreCase))
			{
				num = i;
				break;
			}
		}
		if (num != -1)
		{
			((Component)_character.attach.transform.GetChild(num)).GetComponent<SpriteRenderer>().color = _minimapColor;
		}
	}

	private void OpenHUDHealthBar()
	{
		if ((Object)(object)_attacher == (Object)null)
		{
			Debug.LogError((object)"Attacher의 위치가 잘못되었습니다.");
			return;
		}
		HeadupDisplay hud = Scene<GameBase>.instance.uiManager.headupDisplay;
		hud.darkEnemyHealthBar.Open(_character, _attacher.displayName);
		hud.darkEnemyHealthBar.AddTarget(_character, _attacher.displayName);
		_character.health.onDiedTryCatch += delegate
		{
			IDictionary<Character, string> attached = hud.darkEnemyHealthBar.attached;
			hud.darkEnemyHealthBar.RemoveTarget(_character);
			if (hud.darkEnemyHealthBar.attached.Count == 0)
			{
				hud.darkEnemyHealthBar.Close();
			}
			else
			{
				hud.darkEnemyHealthBar.Set(ExtensionMethods.Random<KeyValuePair<Character, string>>((IEnumerable<KeyValuePair<Character, string>>)attached).Key);
			}
		};
	}
}
