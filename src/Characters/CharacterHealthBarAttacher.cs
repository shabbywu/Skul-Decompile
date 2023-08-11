using UnityEngine;

namespace Characters;

public class CharacterHealthBarAttacher : MonoBehaviour
{
	[SerializeField]
	[GetComponent]
	private Character _character;

	[SerializeField]
	private CharacterHealthBar _healthBar;

	[SerializeField]
	private Transform _parent;

	[SerializeField]
	private Vector2 _offset;

	private CharacterHealthBar _instanitated;

	private void Start()
	{
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)_character != (Object)null)
		{
			if ((Object)(object)_instanitated != (Object)null)
			{
				Object.Destroy((Object)(object)((Component)_instanitated).gameObject);
			}
			_instanitated = Object.Instantiate<CharacterHealthBar>(_healthBar, _parent ?? ((Component)this).transform);
			((Component)_instanitated).transform.position = ((Component)this).transform.position + MMMaths.Vector2ToVector3(_offset);
			_instanitated.Initialize(_character);
			_instanitated.SetWidth(_character.collider.size.x * 32f);
		}
	}

	private void OnDrawGizmosSelected()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		if (!Application.isPlaying)
		{
			Gizmos.DrawIcon(((Component)this).transform.position + MMMaths.Vector2ToVector3(_offset), "healthbar");
		}
	}

	public void SetHealthBar(CharacterHealthBar newHealthBar)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)_instanitated != (Object)null)
		{
			Object.Destroy((Object)(object)((Component)_instanitated).gameObject);
		}
		_instanitated = Object.Instantiate<CharacterHealthBar>(newHealthBar, _parent ?? ((Component)this).transform);
		((Component)_instanitated).transform.position = ((Component)this).transform.position + MMMaths.Vector2ToVector3(_offset);
		_instanitated.Initialize(_character);
		_instanitated.SetWidth(_character.collider.size.x * 32f);
	}
}
