using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobSerializer
{
    Queue<IJob> jobQueue = new Queue<IJob>();
    object _lock = new object();
    bool _flush = false;

    public void Push(IJob job)
    {
        lock(_lock)
        {
            jobQueue.Enqueue(job);
        }
        if (_flush == false)
            Flush();
    }
    public void Push(Action action)
    {
        Push(new Job(action));
    }

    public void Push<T1>(Action<T1> action, T1 t1)
    {
        Push(new Job<T1>(action, t1));
    }

    public void Push<T1, T2>(Action<T1, T2> action, T1 t1, T2 t2)
    {
        Push(new Job<T1, T2>(action, t1, t2));
    }

    public void Push<T1, T2, T3>(Action<T1, T2, T3> action, T1 t1, T2 t2, T3 t3)
    {
        Push(new Job<T1, T2, T3>(action, t1, t2, t3));
    }

    public void Flush()
    {
        _flush = true;
        while (true)
        {
            IJob job = Pop();
            if (job == null)
                return;
            job.Execute();
        }
    }
    IJob Pop()
    {
        lock(_lock)
        {
            if(jobQueue.Count == 0)
            {
                _flush =false;
                return null;
            }
            return jobQueue.Dequeue();
        }
    }
}
