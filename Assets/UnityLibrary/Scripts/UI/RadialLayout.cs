using UnityEngine;

namespace UI
{
    public class RadialLayout : MonoBehaviour
    {
        // .. ATTRIBUTES

        [SerializeField]
        private Transform[] ItemsToLayout = null;
        [SerializeField]
        private float Radius = 1f;

        // .. INITIALIZATION

        void Start()
        {
            var item_count = ItemsToLayout.Length;
            float angle_inc = Mathf.Deg2Rad * (360f / (float)item_count);
            for (int i = 0; i < item_count; i++)
            {
                ItemsToLayout[i].localPosition = new Vector3(Mathf.Cos(angle_inc * i), Mathf.Sin(angle_inc * i)) * Radius;
            }
        }
    }
}
