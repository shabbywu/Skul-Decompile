using UnityEngine;

namespace GameResources;

public class MaterialResource : ScriptableObject
{
	[SerializeField]
	private Material _color;

	[SerializeField]
	private Material _colorOverlay;

	[SerializeField]
	private Material _character;

	[SerializeField]
	private Material _effect;

	[SerializeField]
	private Material _effect_darken;

	[SerializeField]
	private Material _effect_lighten;

	[SerializeField]
	private Material _effect_linearBurn;

	[SerializeField]
	private Material _effect_linearDodge;

	[SerializeField]
	private Material _minimap;

	[SerializeField]
	private Material _ui_grayScale;

	[SerializeField]
	private Material _darkEnemy;

	public static Material color { get; private set; }

	public static Material colorOverlay { get; private set; }

	public static Material character { get; private set; }

	public static Material effect { get; private set; }

	public static Material effect_darken { get; private set; }

	public static Material effect_lighten { get; private set; }

	public static Material effect_linearBurn { get; private set; }

	public static Material effect_linearDodge { get; private set; }

	public static Material minimap { get; private set; }

	public static Material ui_grayScale { get; private set; }

	public static Material darkEnemy { get; private set; }

	public void Initialize()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		((Object)this).hideFlags = (HideFlags)(((Object)this).hideFlags | 0x20);
		color = _color;
		colorOverlay = _colorOverlay;
		character = _character;
		effect = _effect;
		effect_darken = _effect_darken;
		effect_lighten = _effect_lighten;
		effect_linearBurn = _effect_linearBurn;
		effect_linearDodge = _effect_linearDodge;
		minimap = _minimap;
		ui_grayScale = _ui_grayScale;
		darkEnemy = _darkEnemy;
	}
}
