using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace ElementsBook
{
    public class ElementBookResponse
    {
        public ElementData[] elementData;
    }
    
    public class NetworkElementProvider : IElementProvider
    {
        public Task<List<ElementData>> GetElements()
        {
            var taskCompletionSource = new TaskCompletionSource<List<ElementData>>();

            var request = UnityWebRequest.Get(Constants.ApiUrl + "api/elementsbook");
            request.timeout = Constants.RpcTimeoutSeconds;
            var operation = request.SendWebRequest();
            operation.completed += asyncOperation =>
            {
                try
                {
                    // TODO Fail fast on 4xx/5xx responses
                    Debug.Log(request.downloadHandler.text);
                    var response = JsonUtility.FromJson<ElementBookResponse>(request.downloadHandler.text);
                    taskCompletionSource.TrySetResult(response.elementData.ToList());
                }
                catch (Exception e)
                {
                    taskCompletionSource.TrySetException(e);
                }
            };

            return taskCompletionSource.Task;
        }
    }
}