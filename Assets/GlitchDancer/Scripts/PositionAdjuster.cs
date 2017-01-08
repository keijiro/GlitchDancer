using UnityEngine;

namespace GlitchDancer
{
    public class PositionAdjuster : MonoBehaviour
    {
        public void ResetPosition()
        {
            var animator = GetComponent<Animator>();
            var mask = new MatchTargetWeightMask(Vector3.one, 0);
            var time = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            animator.MatchTarget(Vector3.zero, Quaternion.identity, AvatarTarget.Root, mask, time, time + 0.01f);
        }
    }
}
