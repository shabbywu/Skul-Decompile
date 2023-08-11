using GameResources;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Blessing;

public class Blessing : MonoBehaviour, IAbility, IAbilityInstance
{
	[SerializeField]
	[Header("임시")]
	private string _notificationText;

	[SerializeField]
	private string _floatingTextkey;

	[SerializeField]
	private string _activatedNameKey;

	[SerializeField]
	private string _activatedChatKey;

	[SerializeField]
	private AnimationClip _holyGrail;

	[SerializeField]
	private Sprite _icon;

	[SerializeField]
	private float _duration;

	[AbilityAttacher.Subcomponent]
	[SerializeField]
	private AbilityAttacher.Subcomponents _abilityAttacher;

	private float _remainTime;

	public float duration => _duration;

	public int iconPriority { get; set; }

	public bool removeOnSwapWeapon { get; set; }

	public Character owner { get; private set; }

	public IAbility ability => this;

	public float remainTime
	{
		get
		{
			return _remainTime;
		}
		set
		{
			_remainTime = value;
		}
	}

	public bool attached { get; private set; }

	public Sprite icon => _icon;

	public float iconFillAmount => 1f - _remainTime / _duration;

	public bool iconFillInversed => false;

	public bool iconFillFlipped => false;

	public int iconStacks => 0;

	public bool expired => remainTime <= 0f;

	public AnimationClip clip => _holyGrail;

	public string activatedNameKey => _activatedNameKey;

	public string activatedChatKey => _activatedChatKey;

	public void Apply(Character target)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		Bounds bounds = ((Collider2D)target.collider).bounds;
		float x = ((Bounds)(ref bounds)).center.x;
		bounds = ((Collider2D)target.collider).bounds;
		Vector2 val = default(Vector2);
		((Vector2)(ref val))._002Ector(x, ((Bounds)(ref bounds)).max.y + 0.5f);
		Singleton<Service>.Instance.floatingTextSpawner.SpawnBuff(Localization.GetLocalizedString(_floatingTextkey), Vector2.op_Implicit(val));
		owner = target;
		((Component)this).transform.parent = ((Component)owner).transform;
		((Component)this).transform.localPosition = Vector3.zero;
		_abilityAttacher.Initialize(owner);
		owner.ability.Add(this);
	}

	public void Attach()
	{
		attached = true;
		remainTime = duration;
		_abilityAttacher.StartAttach();
	}

	public IAbilityInstance CreateInstance(Character owner)
	{
		return this;
	}

	public void Initialize()
	{
	}

	public void Refresh()
	{
		remainTime = duration;
	}

	public void UpdateTime(float deltaTime)
	{
		remainTime -= deltaTime;
	}

	public void Detach()
	{
		attached = false;
		_abilityAttacher.StopAttach();
	}

	private void OnDestroy()
	{
		_icon = null;
		_holyGrail = null;
		_abilityAttacher.StopAttach();
	}
}
