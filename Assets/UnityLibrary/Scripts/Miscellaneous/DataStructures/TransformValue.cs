using UnityEngine;

namespace Miscellaneous.DataStructures
{
    /// <summary>
    /// Stores (snapshot) the state of a transform.
    /// </summary>
    public class TransformValue
    {
        // .. ATTRIBUTES

        public Vector3 Position { get; private set; }
        public Vector3 LocalPosition { get; private set; }
        public Quaternion Rotation { get; private set; }
        public Quaternion LocalRotation { get; private set; }
        public Vector3 LossyScale { get; private set; }
        public Vector3 LocalScale { get; private set; }

        private Matrix4x4 localToWorld;
        private Matrix4x4 worldToLocal;

        // .. INITIALIZATION

        public TransformValue(Transform transform)
        {
            Position = transform.position;
            LocalPosition = transform.localPosition;
            Rotation = transform.rotation;
            LocalRotation = transform.localRotation;
            LossyScale = transform.lossyScale;
            LocalScale = transform.localScale;

            localToWorld = transform.localToWorldMatrix;
            worldToLocal = transform.worldToLocalMatrix;
        }

        /// <summary>
        /// Transforming vectors not supported when using this TransformValue constructor!
        /// </summary>
        /// <param name="local_pos"></param>
        /// <param name="local_rot"></param>
        /// <param name="local_scale"></param>
        public TransformValue(Vector3 local_pos, Quaternion local_rot, Vector3 local_scale)
        {
            Debug.LogWarning("Transforming vectors not supported when using this TransformValue constructor!");

            LocalPosition = local_pos;
            LocalRotation = local_rot;
            LocalScale = local_scale;
        }


        // .. OPERATIONS

        /// <summary>
        /// Transforms position from world space to local space.
        /// </summary>
        /// <returns></returns>
        public Vector3 InverseTransformPoint(Vector3 position)
        {
            return worldToLocal.MultiplyPoint(position);
        }

        /// <summary>
        /// Transforms position from local space to world space.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Vector3 TransformPoint(Vector3 position)
        {
            return localToWorld.MultiplyPoint(position);
        }

        public Vector3 InverseTransformDirection(Vector3 direction)
        {
            return worldToLocal.MultiplyVector(direction);
        }

        public Vector3 TransformDirection(Vector3 direction)
        {
            return localToWorld.MultiplyVector(direction);
        }
    }
}
