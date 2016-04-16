using UnityEngine;

namespace Shapes
{
    public class CubicBezier2DController : ADrawableController
    {
        // .. ATTRIBUTES

        [SerializeField]
        private Vector2 P0 = Vector2.zero;
        [SerializeField]
        private Vector2 P1 = Vector2.zero;
        [SerializeField]
        private Vector2 P2 = Vector2.zero;
        [SerializeField]
        private Vector2 P3 = Vector2.zero;
        [SerializeField]
        private int Resolution = 25;

        private CubicBezier2D bezier;
        private bool isInitialized;

        // .. INITIALIZATION

        void Start()
        {
            TryInitialize();
        }

        private void TryInitialize()
        {
            if (isInitialized)
                return;

            bezier = new CubicBezier2D(P0, P1, P2, P3);
            bezier.Resolution = Resolution;

            isInitialized = true;
        }

        // .. OPERATIONS

        public override IDrawable2D GetDrawable()
        {
            TryInitialize();
            return bezier;
        }

        void Update()
        {
            bezier.P0 = P0;
            bezier.P1 = P1;
            bezier.P2 = P2;
            bezier.P3 = P3;
            bezier.Resolution = Resolution;
        }
    }
}
