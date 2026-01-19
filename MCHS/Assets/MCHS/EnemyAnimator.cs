using Pathfinding;
using Pathfinding.Util;
using UnityEngine;

namespace MCHS
{
    public class EnemyAnimator : MonoBehaviour
    {
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

		const string NormalizedSpeedKey = "NormalizedSpeed";
		static int NormalizedSpeedKeyHash = Animator.StringToHash(NormalizedSpeedKey);

		private void Awake () {
			ai = GetComponent<IAstarAI>();
			tr = GetComponent<Transform>();
			if (anim != null && !HasParameter(anim, NormalizedSpeedKey)) {
				Debug.LogError($"No '{NormalizedSpeedKey}' parameter found on the animator. The animator must have a float parameter called '{NormalizedSpeedKey}'", this);
				enabled = false;
			}
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
			Vector3 relVelocity = tr.InverseTransformDirection(ai.velocity);
			relVelocity.y = 0;

			// Speed relative to the character size
			anim.SetFloat(NormalizedSpeedKeyHash, relVelocity.magnitude / (naturalSpeed * anim.transform.lossyScale.x));
		}
    }
}