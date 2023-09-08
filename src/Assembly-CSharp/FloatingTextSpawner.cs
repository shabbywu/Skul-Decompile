using System.Collections;
using Characters;
using GameResources;
using Scenes;
using UI;
using UnityEngine;

public class FloatingTextSpawner : MonoBehaviour
{
	private const int _countLimit = 100;

	private const string _buffDefaultColorString = "#F2F2F2";

	public FloatingText floatingTextPrefab;

	private const int _buffTextLimit = 10;

	public BuffText _buffTextPrefab;

	private Vector3 _playerTakingDamageScale = new Vector3(1.25f, 1.25f, 1f);

	private Color _playerTakingDamageColor;

	private Color _criticalPhysicalAttackColor;

	private Color _criticalMagicAttackColor;

	private Color _physicalAttackColor;

	private Color _magicAttackColor;

	private Color _fixedAttackColor;

	private Color _healColor;

	private string breakShieldText => Localization.GetLocalizedString("floating/breakShield");

	private void Awake()
	{
		ColorUtility.TryParseHtmlString("#FF0D06", ref _playerTakingDamageColor);
		ColorUtility.TryParseHtmlString("#E3FF00", ref _criticalPhysicalAttackColor);
		ColorUtility.TryParseHtmlString("#17FFDB", ref _criticalMagicAttackColor);
		ColorUtility.TryParseHtmlString("#FF8000", ref _physicalAttackColor);
		ColorUtility.TryParseHtmlString("#17C4FF", ref _magicAttackColor);
		ColorUtility.TryParseHtmlString("#F2F2F2", ref _fixedAttackColor);
		ColorUtility.TryParseHtmlString("#18FF00", ref _healColor);
	}

	public FloatingText Spawn(string text, Vector3 position)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		FloatingText floatingText = floatingTextPrefab.Spawn();
		floatingText.Initialize(text, position);
		return floatingText;
	}

	public void SpawnPlayerTakingDamage(in Damage damage)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		SpawnPlayerTakingDamage(damage.amount, damage.hitPoint);
	}

	public void SpawnPlayerTakingDamage(double amount, Vector2 position)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		if (Scene<GameBase>.instance.uiManager.hideOption != UIManager.HideOption.HideAll && !(amount < 1.0))
		{
			FloatingText floatingText = Spawn(amount.ToString("0"), Vector2.op_Implicit(position + new Vector2(0f, 0.5f)));
			floatingText.color = _playerTakingDamageColor;
			((MonoBehaviour)(object)floatingText).Modify(GameObjectModifier.TranslateBySpeedAndAcc(9f, -12f, 2.5f));
			floatingText.sortingOrder = 500;
			((Component)floatingText).transform.localScale = _playerTakingDamageScale;
			if (MMMaths.RandomBool())
			{
				((MonoBehaviour)(object)floatingText).Modify(GameObjectModifier.TranslateUniformMotion(0.2f, 0f, 0f));
			}
			else
			{
				((MonoBehaviour)(object)floatingText).Modify(GameObjectModifier.TranslateUniformMotion(-0.2f, 0f, 0f));
			}
			floatingText.Despawn(0.6f);
		}
	}

	public void SpawnTakingDamage(in Damage damage)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		if (Scene<GameBase>.instance.uiManager.hideOption == UIManager.HideOption.HideAll || damage.amount < 1.0)
		{
			return;
		}
		FloatingText floatingText = Spawn(damage.ToString(), Vector2.op_Implicit(damage.hitPoint + new Vector2(0f, 0.5f)));
		switch (damage.attribute)
		{
		case Damage.Attribute.Physical:
			floatingText.color = _physicalAttackColor;
			floatingText.sortingOrder = 100;
			break;
		case Damage.Attribute.Magic:
			floatingText.color = _magicAttackColor;
			floatingText.sortingOrder = 200;
			break;
		case Damage.Attribute.Fixed:
			floatingText.color = _fixedAttackColor;
			floatingText.sortingOrder = 100;
			break;
		}
		if (damage.critical)
		{
			((MonoBehaviour)(object)floatingText).Modify(GameObjectModifier.LerpScale(1.4f, 1.6f, 0.4f));
			switch (damage.attribute)
			{
			case Damage.Attribute.Physical:
				floatingText.color = _criticalPhysicalAttackColor;
				floatingText.sortingOrder = 300;
				break;
			case Damage.Attribute.Magic:
				floatingText.color = _criticalMagicAttackColor;
				floatingText.sortingOrder = 400;
				break;
			}
		}
		((MonoBehaviour)(object)floatingText).Modify(GameObjectModifier.TranslateBySpeedAndAcc(10f, -17f, 3f));
		if (MMMaths.RandomBool())
		{
			((MonoBehaviour)(object)floatingText).Modify(GameObjectModifier.TranslateUniformMotion(0.2f, 0f, 0f));
		}
		else
		{
			((MonoBehaviour)(object)floatingText).Modify(GameObjectModifier.TranslateUniformMotion(-0.2f, 0f, 0f));
		}
		floatingText.Despawn(0.5f);
	}

	public void SpawnHeal(double amount, Vector3 position)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		if (Scene<GameBase>.instance.uiManager.hideOption != UIManager.HideOption.HideAll && !(amount <= 0.0))
		{
			FloatingText floatingText = Spawn(amount.ToString("0"), position);
			floatingText.color = _healColor;
			((MonoBehaviour)(object)floatingText).Modify(GameObjectModifier.Scale(1.1f));
			((MonoBehaviour)(object)floatingText).Modify(GameObjectModifier.TranslateBySpeedAndAcc(7.5f, -7.5f, 1f));
			floatingText.sortingOrder = 1;
			if (MMMaths.RandomBool())
			{
				((MonoBehaviour)(object)floatingText).Modify(GameObjectModifier.TranslateUniformMotion(0.2f, 0f, 0f));
			}
			else
			{
				((MonoBehaviour)(object)floatingText).Modify(GameObjectModifier.TranslateUniformMotion(-0.2f, 0f, 0f));
			}
			floatingText.Despawn(0.5f);
		}
	}

	public void SpawnBuff(string text, Vector3 position, string colorValue = "#F2F2F2")
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		if (Scene<GameBase>.instance.uiManager.hideOption != UIManager.HideOption.HideAll)
		{
			Color color = default(Color);
			ColorUtility.TryParseHtmlString(colorValue, ref color);
			BuffText buffText = _buffTextPrefab.Spawn();
			buffText.Initialize(text, position);
			buffText.color = color;
			buffText.sortingOrder = 1;
			((MonoBehaviour)this).StartCoroutine(CMove(((Component)buffText).transform, 0.5f, 2f));
			((MonoBehaviour)this).StartCoroutine(CFadeOut(buffText, 1.5f, 0.5f));
			buffText.Despawn(2f);
		}
	}

	public void SpawnStatus(string text, Vector3 position, string colorValue)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		if (Scene<GameBase>.instance.uiManager.hideOption != UIManager.HideOption.HideAll)
		{
			Color color = default(Color);
			ColorUtility.TryParseHtmlString(colorValue, ref color);
			FloatingText floatingText = Spawn(text, position);
			floatingText.color = color;
			floatingText.sortingOrder = 1;
			((MonoBehaviour)(object)floatingText).Modify(GameObjectModifier.Scale(0.75f));
			((MonoBehaviour)this).StartCoroutine(CMove(((Component)floatingText).transform, 0.5f, 1f));
			((MonoBehaviour)this).StartCoroutine(CFadeOut(floatingText, 0.5f, 0.5f));
			floatingText.Despawn(1f);
		}
	}

	public void SpawnEvade(string text, Vector3 position, string colorValue)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		if (Scene<GameBase>.instance.uiManager.hideOption != UIManager.HideOption.HideAll)
		{
			Color color = default(Color);
			ColorUtility.TryParseHtmlString(colorValue, ref color);
			FloatingText floatingText = Spawn(text, position);
			floatingText.color = color;
			floatingText.sortingOrder = 1;
			((MonoBehaviour)(object)floatingText).Modify(GameObjectModifier.Scale(1f));
			((MonoBehaviour)this).StartCoroutine(CMove(((Component)floatingText).transform, 0.5f, 1f));
			((MonoBehaviour)this).StartCoroutine(CFadeOut(floatingText, 0.5f, 0.5f));
			floatingText.Despawn(1f);
		}
	}

	public void SpawnBreakShield(Vector3 position, string colorValue = "#F2F2F2")
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		if (Scene<GameBase>.instance.uiManager.hideOption != UIManager.HideOption.HideAll)
		{
			Color color = default(Color);
			ColorUtility.TryParseHtmlString(colorValue, ref color);
			BuffText buffText = _buffTextPrefab.Spawn();
			buffText.Initialize(breakShieldText, position);
			buffText.color = color;
			buffText.sortingOrder = 1;
			((MonoBehaviour)this).StartCoroutine(CMove(((Component)buffText).transform, 0.5f, 2f));
			((MonoBehaviour)this).StartCoroutine(CFadeOut(buffText, 1.5f, 0.5f));
			buffText.Despawn(2f);
		}
	}

	private IEnumerator CMove(Transform transform, float distance, float duration)
	{
		float elapsed = 0f;
		float speed = distance / duration;
		while (elapsed <= duration)
		{
			float deltaTime = Chronometer.global.deltaTime;
			float num = speed * deltaTime;
			elapsed += deltaTime;
			transform.Translate(Vector2.op_Implicit(Vector2.up * num));
			yield return null;
		}
	}

	private IEnumerator CFadeOut(FloatingText spawned, float delay, float duration)
	{
		yield return Chronometer.global.WaitForSeconds(delay);
		spawned.FadeOut(duration);
	}

	private IEnumerator CFadeOut(BuffText spawned, float delay, float duration)
	{
		yield return Chronometer.global.WaitForSeconds(delay);
		spawned.FadeOut(duration);
	}
}
