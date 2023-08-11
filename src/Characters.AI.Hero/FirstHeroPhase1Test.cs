using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Hero;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Hero;

public class FirstHeroPhase1Test : AIController
{
	[Subcomponent(typeof(BackSlashA))]
	[SerializeField]
	[Header("Slash")]
	private BackSlashA _basicSlash;

	[SerializeField]
	[Subcomponent(typeof(BackSlashB))]
	private BackSlashB _horizontalSlash;

	[Subcomponent(typeof(VerticalSlash))]
	[SerializeField]
	private VerticalSlash _verticalSlash;

	[Subcomponent(typeof(Landing))]
	[Header("Basic Skill")]
	[SerializeField]
	private Landing _landing;

	[SerializeField]
	[Subcomponent(typeof(Characters.AI.Behaviours.Hero.Dash))]
	[Header("Dash")]
	private Characters.AI.Behaviours.Hero.Dash _dash;

	[Subcomponent(typeof(DashBreakAway))]
	[SerializeField]
	private DashBreakAway _dashBreakAway;

	[Header("Template")]
	[SerializeField]
	private BehaviourTemplate _behaviourA;

	[SerializeField]
	private BehaviourTemplate _behaviourB;

	[SerializeField]
	private BehaviourTemplate _behaviourC;

	[SerializeField]
	private BehaviourTemplate _behaviourD;

	[SerializeField]
	private BehaviourTemplate _behaviourE;

	[SerializeField]
	private BehaviourTemplate _behaviourF;

	[SerializeField]
	private BehaviourTemplate _behaviourG;

	[SerializeField]
	private BehaviourTemplate _behaviourH;

	[SerializeField]
	private BehaviourTemplate _behaviourI;

	[SerializeField]
	private BehaviourTemplate _behaviourJ;

	[SerializeField]
	private BehaviourTemplate _behaviourK;

	[SerializeField]
	private BehaviourTemplate _behaviourL;

	[SerializeField]
	private BehaviourTemplate _behaviourM;

	[SerializeField]
	private BehaviourTemplate _behaviourN;

	[Subcomponent(typeof(SkipableIdle))]
	[SerializeField]
	[Header("Idle")]
	private SkipableIdle _skipableIdle;

	[Subcomponent(typeof(Idle))]
	[SerializeField]
	private Idle _idle;

	[Range(0f, 1f)]
	[Header("Behaviour Chance")]
	[SerializeField]
	private float _behaviourA_Chance = 0.5f;

	[SerializeField]
	[Range(0f, 1f)]
	private float _behaviourB_Chance = 0.5f;

	[SerializeField]
	[Range(0f, 1f)]
	private float _behaviourC_Chance = 0.5f;

	[Range(0f, 1f)]
	[SerializeField]
	private float _behaviourG_Chance = 0.5f;

	[Range(0f, 1f)]
	[SerializeField]
	private float _behaviourH_Chance = 0.5f;

	[Range(0f, 1f)]
	[SerializeField]
	private float _behaviourK_Chance = 0.5f;

	[Range(0f, 1f)]
	[SerializeField]
	private float _behaviourN_Chance = 0.5f;

	[Header("Trigger")]
	[Subcomponent(typeof(Trigger))]
	[SerializeField]
	private Trigger _trigger;

	private Behaviour[] _slash;

	private void Awake()
	{
		_slash = new Behaviour[3] { _basicSlash, _horizontalSlash, _verticalSlash };
	}

	private new void OnEnable()
	{
		((MonoBehaviour)this).StartCoroutine(CProcess());
	}

	protected override IEnumerator CProcess()
	{
		yield return CPlayStartOption();
		while (true)
		{
			if (_trigger.InShortRange(this))
			{
				if (_trigger.CanRunDashBreakAway(this))
				{
					yield return _dashBreakAway.CRun(this);
					continue;
				}
				if (_trigger.CanRunBehavourE(this))
				{
					yield return _behaviourE.CRun(this);
					yield return _idle.CRun(this);
					continue;
				}
				switch (Random.Range(0, 3))
				{
				case 0:
					yield return ExtensionMethods.Random<Behaviour>((IEnumerable<Behaviour>)_slash).CRun(this);
					if (MMMaths.Chance(_behaviourA_Chance))
					{
						yield return _behaviourA.CRun(this);
					}
					yield return _skipableIdle.CRun(this);
					break;
				case 1:
					yield return _landing.CRun(this);
					if (MMMaths.Chance(_behaviourB_Chance))
					{
						yield return _behaviourB.CRun(this);
						if (MMMaths.Chance(_behaviourC_Chance))
						{
							yield return _behaviourC.CRun(this);
						}
					}
					yield return _skipableIdle.CRun(this);
					break;
				case 2:
					yield return _behaviourD.CRun(this);
					yield return _skipableIdle.CRun(this);
					break;
				}
				continue;
			}
			if (_trigger.InMiddleRange(this))
			{
				if (_trigger.CanRunBehavourJ(this))
				{
					yield return _behaviourJ.CRun(this);
					if (MMMaths.Chance(_behaviourK_Chance))
					{
						yield return _behaviourK.CRun(this);
					}
					yield return _idle.CRun(this);
					continue;
				}
				switch (Random.Range(0, 3))
				{
				case 0:
					yield return _dash.CRun(this);
					yield return _behaviourF.CRun(this);
					yield return _skipableIdle.CRun(this);
					break;
				case 1:
					yield return _landing.CRun(this);
					if (MMMaths.Chance(_behaviourG_Chance))
					{
						yield return _behaviourG.CRun(this);
						if (MMMaths.Chance(_behaviourH_Chance))
						{
							yield return _behaviourH.CRun(this);
						}
					}
					yield return _skipableIdle.CRun(this);
					break;
				case 2:
					yield return _behaviourJ.CRun(this);
					yield return _skipableIdle.CRun(this);
					break;
				}
				continue;
			}
			if (MMMaths.RandomBool())
			{
				yield return _dash.CRun(this);
				yield return _behaviourL.CRun(this);
			}
			else
			{
				yield return _behaviourM.CRun(this);
				if (MMMaths.Chance(_behaviourN_Chance))
				{
					yield return _behaviourN.CRun(this);
				}
			}
			yield return _skipableIdle.CRun(this);
		}
	}
}
