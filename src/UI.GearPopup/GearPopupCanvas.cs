using Unity.Mathematics;
using UnityEngine;

namespace UI.GearPopup;

[RequireComponent(typeof(RectTransform))]
public class GearPopupCanvas : MonoBehaviour
{
	private const float _width = 474f;

	private const float _minViewportY = 0.4f;

	private const float _maxViewportY = 0.6f;

	private const float _padding = 5f;

	[SerializeField]
	private GearPopup _gearPopup;

	[SerializeField]
	private RectTransform _container;

	[SerializeField]
	private RectTransform _content;

	[SerializeField]
	private RectTransform _canvas;

	public GearPopup gearPopup => _gearPopup;

	private void Awake()
	{
		Close();
	}

	private void Update()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = _content.sizeDelta / 2f;
		val.x = 474f;
		val.x *= ((Transform)_container).lossyScale.x;
		val.y *= ((Transform)_container).lossyScale.y;
		val.x += 5f;
		val.y += 5f;
		float num = _canvas.sizeDelta.x * ((Transform)_canvas).localScale.x;
		float num2 = _canvas.sizeDelta.y * ((Transform)_canvas).localScale.y;
		Vector3 position = ((Transform)_container).position;
		position.x = math.clamp(position.x, val.x, num - val.x);
		position.y = math.clamp(position.y, val.y, num2 - val.y);
		((Transform)_container).position = position;
	}

	public void Open(Vector3 position)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		((Component)_container).gameObject.SetActive(true);
		Vector3 val = Camera.main.WorldToViewportPoint(position);
		val.y = Mathf.Clamp(val.y, 0.4f, 0.6f);
		Vector2 sizeDelta = _canvas.sizeDelta;
		sizeDelta.x *= ((Transform)_canvas).localScale.x;
		sizeDelta.y *= ((Transform)_canvas).localScale.y;
		Vector2 val2 = default(Vector2);
		((Vector2)(ref val2))._002Ector(val.x * sizeDelta.x, val.y * sizeDelta.y);
		((Transform)_container).position = Vector2.op_Implicit(val2);
	}

	public void Close()
	{
		((Component)_container).gameObject.SetActive(false);
	}
}
