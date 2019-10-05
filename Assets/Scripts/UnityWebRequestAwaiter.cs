using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

public class UnityWebRequestAwaiter : INotifyCompletion
{
    private readonly UnityWebRequestAsyncOperation _asyncOp;
    private Action _continuation;

    public UnityWebRequestAwaiter(UnityWebRequestAsyncOperation asyncOp)
    {
        _asyncOp = asyncOp;
        asyncOp.completed += RequestCompleted;
    }

    public bool IsCompleted => _asyncOp.isDone;

    public void GetResult() { }

    public void OnCompleted(Action continuation)
    {
        _continuation = continuation;
    }
    
    private void RequestCompleted(AsyncOperation obj)
    {
        _continuation();
    }
}

public static class ExtensionMethods
{
    public static UnityWebRequestAwaiter GetAwaiter(this UnityWebRequestAsyncOperation asyncOp)
    {
        return new UnityWebRequestAwaiter(asyncOp);
    }
}
