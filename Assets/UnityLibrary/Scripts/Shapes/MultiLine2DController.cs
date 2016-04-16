using UnityEngine;

namespace Shapes
{
    public class MultiLine2DController : ADrawableController
    {
        // .. ATTRIBUTES

        [SerializeField]
        private Vector2[] Points = null;

        private MultiLine2D MultiLine;
        private bool initialized;

        // .. INITIALIZATION

        void Start()
        {
            TryInitialize();
        }

        private void TryInitialize()
        {
            if (initialized)
                return;

            MultiLine = new MultiLine2D(new Line2D(Points[0], Points[1]));
            for (int i = 2; i < Points.Length; i++)
            {
                MultiLine.AddPoint(Points[i]);
            }

            initialized = true;
        }

        // .. OPERATIONS

        public override IDrawable2D GetDrawable()
        {
            TryInitialize();
            return MultiLine;
        }

        
    }
}
