using System;
using FX;
using GameResources;
using UnityEngine;

namespace Characters.Operations.Summon.Custom;

[Serializable]
public class ApplyDimensionalistEffect : IBDCharacterSetting
{
	[SerializeField]
	private EffectInfo _loopEffect;

	[SerializeField]
	private EffectInfo _despawnEffect;

	public void ApplyTo(Character character)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		_despawnEffect.Spawn(((Component)character).transform.position);
		if (_loopEffect != null)
		{
			((Component)_loopEffect.Spawn(((Component)character).transform.position, character)).transform.parent = ((Component)character).transform;
		}
		MaterialPropertyBlock val = new MaterialPropertyBlock();
		SpriteRenderer mainRenderer = character.spriteEffectStack.mainRenderer;
		((Renderer)mainRenderer).material = MaterialResource.darkEnemy;
		((Renderer)mainRenderer).GetPropertyBlock(val);
		val.SetFloat(Shader.PropertyToID("_UseInverseColor"), 0f);
		val.SetFloat(Shader.PropertyToID("_FirstGrayScale"), 1f);
		val.SetFloat(Shader.PropertyToID("_UseAdderColor"), 1f);
		val.SetFloat(Shader.PropertyToID("_DarkAreaAdderLevel"), -0.05f);
		val.SetFloat(Shader.PropertyToID("_DarkAreaPower"), 0.08f);
		val.SetFloat(Shader.PropertyToID("_RedLerpMax"), 1f);
		val.SetFloat(Shader.PropertyToID("_BlueLerpMax"), 1f);
		val.SetFloat(Shader.PropertyToID("_RedLerpMax2"), 0f);
		val.SetColor(Shader.PropertyToID("_ColorBurn"), new Color(0.932489f, 0.8932005f, 0.9811321f));
		val.SetColor(Shader.PropertyToID("_AdderColor"), new Color(0.6739612f, 0.5707546f, 1f));
		val.SetColor(Shader.PropertyToID("_DarkAreaColor"), new Color(1f, 1f, 1f));
		((Renderer)mainRenderer).SetPropertyBlock(val);
		CharacterDieEffect component = ((Component)character).GetComponent<CharacterDieEffect>();
		component.effect = _despawnEffect;
		component.particleInfo = null;
	}
}
