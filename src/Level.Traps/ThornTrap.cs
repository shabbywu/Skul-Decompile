using Characters;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Level.Traps;

[ExecuteAlways]
public class ThornTrap : Trap
{
	[SerializeField]
	[GetComponent]
	private SpriteRenderer _spriteRenderer;

	[SerializeField]
	private Character _character;

	[SerializeField]
	private BoxCollider2D _collider;

	[SerializeField]
	private int _size = 1;

	[Subcomponent(typeof(OperationInfos))]
	[SerializeField]
	private OperationInfos _operationInfos;

	private void SetSize()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		Vector2 size = _spriteRenderer.size;
		size.x = _size * 2;
		_spriteRenderer.size = size;
		Vector2 size2 = _collider.size;
		size2.x = (float)(_size * 2) - 1.2f;
		_collider.size = size2;
	}

	private void Awake()
	{
		SetSize();
		_operationInfos.Initialize();
		_operationInfos.Run(_character);
	}

	private void Update()
	{
	}
}
