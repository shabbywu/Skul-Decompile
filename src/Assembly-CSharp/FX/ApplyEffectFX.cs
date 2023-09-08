using UnityEngine;

namespace FX;

public class ApplyEffectFX : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer _renderer;

	[SerializeField]
	private int _hue;

	private MaterialPropertyBlock _propertyBlock;

	private void Awake()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		_propertyBlock = new MaterialPropertyBlock();
		((Renderer)_renderer).GetPropertyBlock(_propertyBlock);
		_propertyBlock.SetInt(EffectInfo.huePropertyID, _hue);
		((Renderer)_renderer).SetPropertyBlock(_propertyBlock);
	}
}
