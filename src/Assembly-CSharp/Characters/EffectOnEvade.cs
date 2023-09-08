using GameResources;
using Services;
using Singletons;
using UnityEngine;

namespace Characters;

public class EffectOnEvade : MonoBehaviour
{
	[SerializeField]
	[GetComponent]
	private Character _character;

	private const string _floatingTextKey = "floating/evade";

	private const string _floatingTextColor = "#a3a3a3";

	public void Awake()
	{
		_character.onEvade += SpawnFloatingText;
	}

	private void SpawnFloatingText(ref Damage damage)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = MMMaths.RandomPointWithinBounds(((Collider2D)_character.collider).bounds);
		Singleton<Service>.Instance.floatingTextSpawner.SpawnEvade(Localization.GetLocalizedString("floating/evade"), Vector2.op_Implicit(val), "#a3a3a3");
	}
}
