using UnityEngine;

namespace ExtensionMethods
{
    public static class ExtensionMethodsRectTransform
    {
        // -- PUBLIC

        // .. EXENSION_METHODS

        public static void MoveAnchoredPosition(
            this RectTransform rect_transform,
            Vector2 movement
            )
        {
            rect_transform.anchoredPosition = rect_transform.anchoredPosition + movement;
        }

        // ~~

        public static void AdjustSizeDelta(
            this RectTransform rect_transform,
            Vector2 adjustment
            )
        {
            rect_transform.sizeDelta = rect_transform.sizeDelta + adjustment;
        }

        // ~~

        public static void ChangePivotWithoutMoving(
            this RectTransform rect_transform,
            Vector2 new_pivot
            )
        {
            Vector2
                previous_pivot;

            previous_pivot = rect_transform.pivot;
            rect_transform.anchoredPosition += new Vector2( rect_transform.rect.width * ( new_pivot.x - previous_pivot.x ), rect_transform.rect.height * ( new_pivot.y - previous_pivot.y ) );
            rect_transform.pivot = new_pivot;
        }
    }
}