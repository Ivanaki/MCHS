using System;
using MCHS.Scripts;
using MySteamVR;
using Pathfinding;
using Pathfinding.Util;
using R3;
using UnityEngine;

namespace MCHS
{
    public class EnemyAnimator : MonoBehaviour
    {
	    [SerializeField] private Transform _endTarget;
	    [SerializeField] private Bol _bol;
	    
        public Animator anim;
        
		/// <summary>
		/// The natural movement speed is the speed that the animations are designed for.
		///
		/// One can for example configure the animator to speed up the animation if the agent moves faster than this, or slow it down if the agent moves slower than this.
		/// </summary>
		public float naturalSpeed = 5f;

		bool isAtEndOfPath;

		IAstarAI ai;
		Transform tr;
		
		private AIDestinationSetter _destinationSetter;
		private FollowerEntity _followerEntity;

		const string NormalizedSpeedKey = "NormalizedSpeed";
		static int NormalizedSpeedKeyHash = Animator.StringToHash(NormalizedSpeedKey);
		private static readonly int Start = Animator.StringToHash("Start");
		private static readonly int End = Animator.StringToHash("End");

		private void Awake ()
		{
			TryGetComponent(out ai);
			tr = GetComponent<Transform>();
			if (anim != null && !HasParameter(anim, NormalizedSpeedKey)) {
				Debug.LogError($"No '{NormalizedSpeedKey}' parameter found on the animator. The animator must have a float parameter called '{NormalizedSpeedKey}'", this);
				enabled = false;
			}

			TryGetComponent(out _destinationSetter);
			TryGetComponent(out _followerEntity);
		}

		static bool HasParameter (Animator animator, string paramName) {
			foreach (AnimatorControllerParameter param in animator.parameters) if (param.name == paramName) return true;
			return false;
		}

		/// <summary>
		/// Called when the end of path has been reached.
		/// An effect (<see cref="endOfPathEffect)"/> is spawned when this function is called
		/// However, since paths are recalculated quite often, we only spawn the effect
		/// when the current position is some distance away from the previous spawn-point
		/// </summary>

		private void Update () {
			// Calculate the velocity relative to this transform's orientation
			if (ai != null)
			{
				Vector3 relVelocity = tr.InverseTransformDirection(ai.velocity);
				relVelocity.y = 0;

				// Speed relative to the character size
				anim.SetFloat(NormalizedSpeedKeyHash, relVelocity.magnitude / (naturalSpeed * anim.transform.lossyScale.x));
			}
		}

		private bool flag = true;
		
		private void FixedUpdate()
		{
			if (transform.position.z > 31f && flag)
			{
				if (_endTarget != null)
				{
					if (_destinationSetter != null)
					{
						flag = false;
						EndEnemy();
					}
				}
			}
		}


		public void StartEnemy()
		{
			if (_bol != null)
			{
				_bol.StartBol();
				_bol.Heal.Subscribe(value =>
				{
					if (value)
					{
						ContinueStart();
					}
				});
			}
			else
			{
				ContinueStart();
			}
		}

		private void ContinueStart()
		{
			anim.SetTrigger(Start);
			if(_followerEntity != null)
				_followerEntity.enabled = true;
			if (_destinationSetter != null)
			{
				_destinationSetter.enabled = true;
				_destinationSetter.target = FindPoint.instance.transform;
			}
		}

		public void EndEnemy()
		{
			anim.SetTrigger(End);

			if (_endTarget != null)
			{
				if (_destinationSetter != null)
				{
					_destinationSetter.target = _endTarget;
				}
			}
		}
    }
}