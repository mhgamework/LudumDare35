using System.Collections;
using Miscellaneous.Easing;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(GameObjectAnimationScale))]
    public class Bouncer : MonoBehaviour
    {
        // .. ATTRIBUTES

        [SerializeField]
        private float BounceFactor = 1.25f;
        [SerializeField]
        private float AnimationTime = 0.25f;

        private GameObjectAnimationScale scaleAnimator;


        // .. OPERATIONS

        public void Bounce()
        {
            StopCoroutine("UpdateBounce");
            StartCoroutine("UpdateBounce", AnimationTime);
        }

        public void Bounce(float animation_time)
        {
            StopCoroutine("UpdateBounce");
            StartCoroutine("UpdateBounce", animation_time);
        }


        // .. COROUTINES

        private IEnumerator UpdateBounce(float animation_time)
        {
            if (scaleAnimator == null)
                scaleAnimator = GetComponent<GameObjectAnimationScale>();

            float grow_time = animation_time * 0.5f;
            float shrink_time = animation_time * 0.5f;
            Transform this_tranform = GetComponent<Transform>();

            scaleAnimator.StartAnimation(EasingFunctions.TYPE.BackInCubic, this_tranform.localScale, Vector3.one * BounceFactor, grow_time);
            yield return new WaitForSeconds(grow_time);
            scaleAnimator.StartAnimation(EasingFunctions.TYPE.Out, this_tranform.localScale, Vector3.one, shrink_time);
        }


    }
}
