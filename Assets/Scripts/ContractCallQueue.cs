using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


    public class ContractCallQueue : MonoBehaviour
    {
        private Queue<Func<Task>> taskQueue = new Queue<Func<Task>>();
        private bool isRunning = false;
    
        public static ContractCallQueue Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);
        }

        public void Enqueue(Func<Task> contractCall)
        {
            taskQueue.Enqueue(contractCall);
            if (!isRunning)
            {
                StartCoroutine(ProcessQueue());
            }
        }

        private IEnumerator ProcessQueue()
        {
            isRunning = true;
            while (taskQueue.Count > 0)
            {
                var taskFunc = taskQueue.Dequeue();
                Task task = taskFunc();

                while (!task.IsCompleted)
                    yield return null;

                if (task.IsFaulted)
                    Debug.LogError($"Contract call failed: {task.Exception?.Message}");
            }
            isRunning = false;
        }
    }


