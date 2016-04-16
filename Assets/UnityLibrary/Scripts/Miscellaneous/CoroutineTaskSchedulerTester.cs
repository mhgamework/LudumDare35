using System.Collections.Generic;
using UnityEngine;

namespace Miscellaneous
{
    public class CoroutineTaskSchedulerTester : MonoBehaviour
    {
        // .. ATTRIBUTES

        [SerializeField]
        private CoroutineScheduler scheduler = null;

        private string c = "C";

        // .. INITIALIZATION

        void Start()
        {
            var task_a = new CoroutineTask(RoutineA("A"));
            scheduler.AddTask(task_a);
            scheduler.AddTask(new CoroutineTask(RoutineB("B")));
            scheduler.AddTask(new CoroutineTask(RoutineC()));
            scheduler.AddTask(new CoroutineTask(RoutineD(task_a)));
        }

        // .. COROUTINES

        private IEnumerator<float> RoutineA(string param)
        {
            while (true)
            {
                Debug.Log(param);
                yield return 0.2f;
            }
        }

        private IEnumerator<float> RoutineB(string param)
        {
            while (true)
            {
                Debug.Log(param);
                yield return 1f;
            }
        }

        private IEnumerator<float> RoutineC()
        {
            while (true)
            {
                Debug.Log(c);
                yield return 0f;
            }
        }

        private IEnumerator<float> RoutineD(CoroutineTask task)
        {
            yield return 3f;
            task.Abort();
        }

    }
}
