using System.Collections.Generic;
using UnityEngine;

namespace Miscellaneous
{
    public class CoroutineTask
    {
        // .. ATTRIBUTES

        private IEnumerator<float> task;
        private bool aborted;

        private float lastWaitTimeStart;
        private float currentWaitTime;

        // .. INITIALIZATION

        public CoroutineTask(IEnumerator<float> enumerator)
        {
            task = enumerator;
        }

        // .. OPERATIONS

        /// <summary>
        /// Perform this task until its next yield instruction.
        /// </summary>
        /// <returns>Whether any yield instructions are left.</returns>
        public bool DoStep()
        {
            if (aborted)
                return false;

            if (Time.realtimeSinceStartup - lastWaitTimeStart < currentWaitTime)
                return true;

            if (!task.MoveNext())
                return false;

            lastWaitTimeStart = Time.realtimeSinceStartup;
            currentWaitTime = task.Current;

            return true;
        }

        /// <summary>
        /// Abort this task.
        /// </summary>
        public void Abort()
        {
            aborted = true;
        }

    }
}
