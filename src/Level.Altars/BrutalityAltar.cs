using Characters;
using Characters.Abilities.CharacterStat;
using GameResources;
using Services;
using Singletons;
using UnityEngine;

namespace Level.Altars;

public class BrutalityAltar : MonoBehaviour
{
	[GetComponent]
	[SerializeField]
	private Altar _altar;

	[SerializeField]
	private StatBonusComponent _statBonus;

	private string _floatingText;

	private void Awake()
	{
		_altar.onDestroyed += OnAltarDestroyed;
		_floatingText = Localization.GetLocalizedString("floating/altar/brutality");
	}

	private void OnAltarDestroyed()
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		Character player = Singleton<Service>.Instance.levelManager.player;
		Bounds bounds = ((Collider2D)player.collider).bounds;
		Singleton<Service>.Instance.floatingTextSpawner.SpawnBuff(_floatingText, Vector2.op_Implicit(new Vector2(((Bounds)(ref bounds)).center.x, ((Bounds)(ref bounds)).max.y + 1f)));
		player.ability.Add(_statBonus.ability);
	}
}
