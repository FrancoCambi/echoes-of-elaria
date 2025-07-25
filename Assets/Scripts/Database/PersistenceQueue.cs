using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PersistenceQueue : MonoBehaviour
{
    private static PersistenceQueue instance;
    public static PersistenceQueue Instance
    {
        get
        {
            return instance != null ? instance : FindAnyObjectByType<PersistenceQueue>();
        }
    }

    private readonly Queue<Func<Task>> _queue = new();
    private bool _isProcessing = false;

    public void Enqueue(Func<Task> task)
    {
        lock (_queue)
        {
            _queue.Enqueue(task);
        }

        if (!_isProcessing)
        {
            _ = ProcessQueue();
        }
    }

    private async Task ProcessQueue()
    {
        _isProcessing = true;

        while (true)
        {
            Func<Task> nextTask = null;

            lock (_queue)
            {
                if (_queue.Count == 0)
                {
                    _isProcessing = false;
                    return;
                }

                nextTask = _queue.Dequeue();
            }

            try
            {
                await nextTask();
            }
            catch (Exception ex)
            {
                Debug.LogError($"[PersistenceQueue] Error during save task: {ex.Message}");
            }
        }
    }
}
