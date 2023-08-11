using GameResources;
using UnityEngine;

public class MaterialProperty : MonoBehaviour
{
	private static readonly int _huePropertyID = Shader.PropertyToID("_Hue");

	[Range(-180f, 180f)]
	public int hue;

	[GetComponent]
	[SerializeField]
	private SpriteRenderer _renderer;

	private void Start()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		MaterialPropertyBlock val = new MaterialPropertyBlock();
		((Renderer)_renderer).sharedMaterial = MaterialResource.effect;
		((Renderer)_renderer).GetPropertyBlock(val);
		val.SetInt(_huePropertyID, hue);
		((Renderer)_renderer).SetPropertyBlock(val);
	}
}
