using System;
using Scenes;
using UnityEngine;

public class CastleParallax : MonoBehaviour
{
	[Serializable]
	private class Element
	{
		[Serializable]
		internal class Reorderable : ReorderableArray<Element>
		{
		}

		[SerializeField]
		private SpriteRenderer _spriteRenderer;

		[SerializeField]
		private float _verticalScroll;

		[SerializeField]
		private float _hotizontalAutoScroll;

		private Vector2 _spriteSize;

		internal void Initialize()
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			if (_hotizontalAutoScroll != 0f)
			{
				_spriteRenderer.drawMode = (SpriteDrawMode)2;
				_spriteRenderer.tileMode = (SpriteTileMode)0;
				Bounds bounds = _spriteRenderer.sprite.bounds;
				_spriteSize = Vector2.op_Implicit(((Bounds)(ref bounds)).size);
				_spriteSize.x *= 2f;
				if (_hotizontalAutoScroll >= 0f)
				{
					_spriteRenderer.size = _spriteSize;
				}
				else
				{
					_spriteRenderer.size = -_spriteSize;
				}
			}
		}

		internal void Update(Vector3 delta, float deltaTime)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			((Component)_spriteRenderer).transform.Translate(0f, _verticalScroll * delta.y, 0f);
			if (_hotizontalAutoScroll != 0f)
			{
				Vector2 size = _spriteRenderer.size;
				size.x += _hotizontalAutoScroll * deltaTime;
				if (_hotizontalAutoScroll > 0f && size.x >= _spriteSize.x * 2f)
				{
					size.x = _spriteSize.x;
				}
				if (_hotizontalAutoScroll < 0f && size.x <= _spriteSize.x * 2f)
				{
					size.x = _spriteSize.x * 2f * 2f;
				}
				_spriteRenderer.size = size;
			}
		}
	}

	[SerializeField]
	private Transform _origin;

	[SerializeField]
	private Element.Reorderable _elements;

	private CameraController _cameraController;

	private void Awake()
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		Element[] values = _elements.values;
		for (int i = 0; i < values.Length; i++)
		{
			values[i].Initialize();
		}
		_cameraController = Scene<GameBase>.instance.cameraController;
		UpdateElements(((Component)_cameraController).transform.position - _origin.position, 0f);
	}

	private void Update()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		UpdateElements(_cameraController.delta, Chronometer.global.deltaTime);
	}

	private void UpdateElements(Vector3 delta, float deltaTime)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		Element[] values = _elements.values;
		for (int i = 0; i < values.Length; i++)
		{
			values[i].Update(delta, deltaTime);
		}
	}
}
