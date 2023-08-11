using FX;
using UnityEngine;

namespace Characters.Operations;

public class SetEmbraceLaser : CharacterOperation
{
	[MinMaxSlider(0f, 180f)]
	[SerializeField]
	private Vector2 _radianRange;

	[SerializeField]
	private Transform _signContainer;

	[SerializeField]
	private Transform _laserContainer;

	[SerializeField]
	private CompositeCollider2D _range;

	private LineEffect[] _signs;

	private LineEffect[] _lasers;

	private void Awake()
	{
		_signs = new LineEffect[_signContainer.childCount];
		_lasers = new LineEffect[_laserContainer.childCount];
		for (int i = 0; i < _signContainer.childCount; i++)
		{
			_signs[i] = ((Component)_signContainer.GetChild(i)).GetComponent<LineEffect>();
			_lasers[i] = ((Component)_laserContainer.GetChild(i)).GetComponent<LineEffect>();
		}
	}

	public override void Run(Character owner)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		float num = Random.Range(0, 360);
		if (_lasers == null || _lasers.Length == 0)
		{
			Debug.LogError((object)"LineEffects is null or length is lower than or equals zero");
			return;
		}
		((Component)_signs[0]).transform.rotation = Quaternion.AngleAxis(num, Vector3.forward);
		((Component)_lasers[0]).transform.rotation = Quaternion.AngleAxis(num, Vector3.forward);
		((Component)_lasers[0]).gameObject.SetActive(true);
		float num2 = 360 / _lasers.Length;
		for (int i = 1; i < _lasers.Length; i++)
		{
			float num3 = Random.Range(_radianRange.x, _radianRange.y);
			((Component)_signs[i]).transform.rotation = Quaternion.AngleAxis(num + num3, Vector3.forward);
			((Component)_lasers[i]).transform.rotation = Quaternion.AngleAxis(num + num3, Vector3.forward);
			num += num2;
			((Component)_lasers[i]).gameObject.SetActive(true);
		}
		_range.GenerateGeometry();
		LineEffect[] lasers = _lasers;
		for (int j = 0; j < lasers.Length; j++)
		{
			((Component)lasers[j]).gameObject.SetActive(false);
		}
	}
}
