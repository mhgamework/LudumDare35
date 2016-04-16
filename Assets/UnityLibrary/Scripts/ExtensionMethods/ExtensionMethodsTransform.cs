using System.Collections.Generic;
using Miscellaneous.DataStructures;
using UnityEngine;

namespace ExtensionMethods
{
    public static class ExtensionMethodsTransform
    {
        public static void Reset(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }

        public static void Lerp(this Transform transform, Transform start, Transform end, float t)
        {
            transform.localPosition = Vector3.Lerp(start.localPosition, end.localPosition, t);
            transform.localRotation = Quaternion.Lerp(start.localRotation, end.localRotation, t);
            transform.localScale = Vector3.Lerp(start.localScale, end.localScale, t);
        }

        public static void Lerp(this Transform transform, TransformValue start, TransformValue end, float t)
        {
            transform.localPosition = Vector3.Lerp(start.LocalPosition, end.LocalPosition, t);
            transform.localRotation = Quaternion.Lerp(start.LocalRotation, end.LocalRotation, t);
            transform.localScale = Vector3.Lerp(start.LocalScale, end.LocalScale, t);
        }

        public static Matrix4x4 LocalToWorldMatrixWithoutScale(this Transform transform)
        {
            Matrix4x4 matrix = new Matrix4x4();
            matrix.SetTRS(transform.position, transform.rotation, Vector3.one);
            return matrix;
        }

        public static Matrix4x4 WorldToLocalMatrixWithoutScale(this Transform transform)
        {
            return LocalToWorldMatrixWithoutScale(transform).inverse;
        }

        public static Transform FindChildRecursive(this Transform parent, string child_name)
        {
            if (parent.gameObject.name == child_name)
            {
                return parent;
            }

            for (int index = 0; index < parent.childCount; index++)
            {
                Transform
                    found_child_transform;

                found_child_transform = parent.GetChild(index).FindChildRecursive(child_name);

                if (found_child_transform != null)
                {
                    return found_child_transform;
                }
            }

            return null;
        }

        public static List<Transform> GetDirectChildren(this Transform transform)
        {
            var children = new List<Transform>();
            for (int i = 0; i < transform.childCount; i++)
            {
                children.Add(transform.GetChild(i));
            }
            return children;
        }
    }
}