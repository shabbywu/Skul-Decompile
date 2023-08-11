using UnityEngine;

namespace FX.SpriteEffects;

public abstract class SpriteEffect
{
	public class Default : SpriteEffect
	{
		public static readonly Default instance = new Default();

		private readonly MaterialPropertyBlock _propertyBlock;

		private Default()
			: base(0)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Expected O, but got Unknown
			_propertyBlock = new MaterialPropertyBlock();
		}

		internal override void Apply(Renderer renderer)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			renderer.GetPropertyBlock(_propertyBlock);
			_propertyBlock.SetColor(_baseColor, Color.white);
			_propertyBlock.SetColor(_overlayColor, Color.clear);
			_propertyBlock.SetFloat(_outlineEnabled, 0f);
			_propertyBlock.SetFloat(_grayScaleLerp, 0f);
			renderer.SetPropertyBlock(_propertyBlock);
		}

		internal override void Expire()
		{
		}

		internal override bool Update(float deltaTime)
		{
			return true;
		}
	}

	public static readonly Default @default = Default.instance;

	protected static readonly int _baseColor = Shader.PropertyToID("_BaseColor");

	protected static readonly int _overlayColor = Shader.PropertyToID("_OverlayColor");

	protected static readonly int _outlineEnabled = Shader.PropertyToID("_IsOutlineEnabled");

	protected static readonly int _outlineColor = Shader.PropertyToID("_OutlineColor");

	protected static readonly int _outlineSize = Shader.PropertyToID("_OutlineSize");

	protected static readonly int _alphaThreshold = Shader.PropertyToID("_AlphaThreshold");

	protected static readonly int _grayScaleLerp = Shader.PropertyToID("_GrayScaleLerp");

	protected const string _outsideMaterialKeyword = "SPRITE_OUTLINE_OUTSIDE";

	public readonly int priority;

	public SpriteEffect(int priority)
	{
		this.priority = priority;
	}

	internal abstract void Apply(Renderer renderer);

	internal abstract bool Update(float deltaTime);

	internal abstract void Expire();
}
