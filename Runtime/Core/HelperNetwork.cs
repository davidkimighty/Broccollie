using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace CollieMollie.Core
{
    public static partial class Helper
    {
        private const int REQUEST_DELAY = 10;

        public static async Task<T> Get<T>(string endpoint)
        {
            UnityWebRequest getRequest = CreateRequest(endpoint, RequestType.GET);
            getRequest.SendWebRequest();

            while (!getRequest.isDone)
                await Task.Delay(REQUEST_DELAY);
            return JsonUtility.FromJson<T>(getRequest.downloadHandler.text);
        }

        public static async Task<T> Post<T>(string endpoint, object payload)
        {
            UnityWebRequest postRequest = CreateRequest(endpoint, RequestType.POST, payload);
            postRequest.SendWebRequest();

            while (!postRequest.isDone)
                await Task.Delay(REQUEST_DELAY);
            return JsonUtility.FromJson<T>(postRequest.downloadHandler.text);
        }

        private static UnityWebRequest CreateRequest(string url, RequestType type, object data = null)
        {
            UnityWebRequest request = new UnityWebRequest(url, type.ToString());

            if (data != null)
            {
                byte[] bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            }

            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            return request;
        }

        private static void AttachHeader(UnityWebRequest request, string key, string value)
        {
            request.SetRequestHeader(key, value);
        }

        public enum RequestType { GET, POST }
    }
}
