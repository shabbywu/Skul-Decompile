using UnityEngine;

namespace Characters.AI.Pope;

public class SacramentOrbPool : MonoBehaviour
{
	[SerializeField]
	private Transform _leftTop;

	[SerializeField]
	private SacramentOrb _orbPrefab;

	[Information("홀수", InformationAttribute.InformationType.Info, false)]
	[SerializeField]
	private int _width = 5;

	[SerializeField]
	private int _height = 5;

	[SerializeField]
	private float _distance = 5f;

	[SerializeField]
	private float _noise = 2f;

	private Vector3[] _originPositions;

	public void Initialize(Character character)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		_originPositions = (Vector3[])(object)new Vector3[_width * _height];
		for (int i = 0; i < _height; i++)
		{
			for (int j = 0; j < _width; j++)
			{
				SacramentOrb sacramentOrb = Object.Instantiate<SacramentOrb>(_orbPrefab, ((Component)this).transform);
				((Component)sacramentOrb).transform.position = Vector2.op_Implicit(new Vector2(_leftTop.position.x + _distance * (float)j, _leftTop.position.y + _distance * (float)i));
				sacramentOrb.Initialize(character);
				((Component)sacramentOrb).gameObject.SetActive(false);
				_originPositions[_width * i + j] = ((Component)sacramentOrb).transform.position;
			}
		}
	}

	public void Run()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		foreach (Transform item in ((Component)this).transform)
		{
			item.position = _originPositions[num++];
			item.Translate(Random.insideUnitSphere * _noise);
			((Component)item).gameObject.SetActive(true);
		}
	}

	public void Hide()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		foreach (Transform item in ((Component)this).transform)
		{
			((Component)item).gameObject.SetActive(false);
		}
	}
}
