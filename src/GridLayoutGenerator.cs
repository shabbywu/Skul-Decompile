using FX;
using UnityEngine;

[ExecuteInEditMode]
public class GridLayoutGenerator : MonoBehaviour
{
	[SerializeField]
	private int _width;

	[SerializeField]
	private int _height;

	[SerializeField]
	private float _distanceX;

	[SerializeField]
	private float _distanceY;

	[SerializeField]
	private GameObject _prefab;

	[Tooltip("여러개 중 랜덤한 것 생성")]
	[SerializeField]
	private GameObject[] _prefabs;

	[SerializeField]
	private PositionNoise _positionNoise;

	[SerializeField]
	private CustomFloat _rotationValue;

	[SerializeField]
	private CustomFloat _scaleValue = new CustomFloat(1f);

	[SerializeField]
	private CustomFloat _scaleXValue = new CustomFloat(1f);

	[SerializeField]
	private CustomFloat _scaleYValue = new CustomFloat(1f);

	public void Generate()
	{
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		RemoveAll();
		float num = _distanceX * (float)(_width - 1) / 2f;
		GameObject val = _prefab;
		for (int i = 0; i < _height; i++)
		{
			for (int j = 0; j < _width; j++)
			{
				if (_prefabs != null && _prefabs.Length != 0)
				{
					val = _prefabs.Random();
				}
				GameObject obj = Object.Instantiate<GameObject>(val, ((Component)this).transform);
				obj.transform.position = Vector2.op_Implicit(new Vector2(((Component)this).transform.position.x + _distanceX * (float)j - num, ((Component)this).transform.position.y + _distanceY * (float)i));
				obj.transform.localRotation = Quaternion.Euler(0f, 0f, _rotationValue.value);
				Vector3 localScale = Vector3.one * _scaleValue.value;
				localScale.x *= _scaleXValue.value;
				localScale.y *= _scaleYValue.value;
				obj.transform.localScale = localScale;
			}
		}
		Noise();
	}

	private void RemoveAll()
	{
		for (int num = ((Component)this).transform.childCount - 1; num >= 0; num--)
		{
			Object.DestroyImmediate((Object)(object)((Component)((Component)this).transform.GetChild(num)).gameObject);
		}
	}

	public void Shuffle()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		foreach (Transform item in ((Component)this).transform)
		{
			item.SetSiblingIndex(Random.Range(0, ((Component)this).transform.childCount - 1));
		}
	}

	public void Noise()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		foreach (Transform item in ((Component)this).transform)
		{
			Transform transform = ((Component)item).transform;
			transform.position += _positionNoise.Evaluate();
		}
	}
}
