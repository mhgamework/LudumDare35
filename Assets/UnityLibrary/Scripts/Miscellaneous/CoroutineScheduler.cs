using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Miscellaneous
{
    public class CoroutineScheduler : MonoBehaviour
    {
        // .. ATTRIBUTES

        private float maxTimeStepMillis = 1 / 60f * 1000f;

        private List<CoroutineTask> tasks = new List<CoroutineTask>();

        // .. INITIALIZATION

        void Start()
        {
            StartCoroutine("MainRoutine");
        }

        // .. OPERATIONS

        public void AddTask(CoroutineTask task)
        {
            tasks.Add(task);
        }

        // .. COROUTINES

        private IEnumerator MainRoutine()
        {
            var stopwatch = new Stopwatch();
            while (true)
            {
                stopwatch.Reset();
                stopwatch.Start();

                int i = 0;
                while (true)
                {
                    var task_i = tasks[i];
                    if (!task_i.DoStep())
                    {
                        tasks.RemoveAt(i);
                        i--;
                    }

                    if (stopwatch.ElapsedMilliseconds > maxTimeStepMillis)
                        break;

                    i++;
                    i = i % tasks.Count;
                }

                /*
                while (stopwatch.ElapsedMilliseconds < maxTimeStepMillis)
                {
                    //spin
                }*/

                yield return null;
            }
        }
    }
}
