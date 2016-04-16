using System.Collections;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// Animates a circular loading indicator which consists of bouncable elements.
    /// </summary>
    public class LoadingIndicator : MonoBehaviour
    {
        // .. ATTRIBUTES

        [SerializeField]
        private Bouncer[] Bouncables = null;
        [SerializeField]
        private float TimePerBounce = 0.25f;
        [SerializeField]
        private float TimeBetweenBounces = 0.25f;
        [SerializeField]
        private bool Rotate = false;
        [SerializeField]
        [Tooltip("In degrees per second")]
        private float RotateSpeed = 5f;

        private Transform thisTransform;

        // .. INITIALIZATION

        void OnEnable()
        {
            StopAllCoroutines();
            StartCoroutine("UpdateAnimation");
            StartCoroutine("UpdateRotation");
        }

        // .. COROUTINES

        IEnumerator UpdateAnimation()
        {
            int bounceable_index = 0;
            while (true)
            {
                Bouncables[bounceable_index].Bounce(TimePerBounce);

                bounceable_index++;
                bounceable_index = bounceable_index % Bouncables.Length;

                yield return new WaitForSeconds(TimeBetweenBounces);
            }
        }

        IEnumerator UpdateRotation()
        {
            if (thisTransform == null)
                thisTransform = GetComponent<Transform>();

            while (true)
            {
                if (Rotate)
                    thisTransform.eulerAngles += new Vector3(0, 0, RotateSpeed * Time.deltaTime);

                yield return null;
            }
        }


    }
}
