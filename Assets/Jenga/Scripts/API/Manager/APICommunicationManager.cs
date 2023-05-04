using JengaGame.API.Data;
using JengaGame.Utils.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using static JengaGame.API.Data.APIRequestData;

namespace JengaGame.API.Manager
{
    public class APICommunicationManager : MonoBehaviour
    {
        [SerializeField]
        private string apiRoot;

        [SerializeField]
        private string getAssessmentEndpoint;

        private Dictionary<Requests, string> requests = new Dictionary<Requests, string>();

        public static APICommunicationManager Instance = null;

        private Coroutine requestCoroutine = null;

        public delegate void AssessmentRequestCallbackDelegate(GetAssessmentData data);
        public delegate void GetRequestCallbackDelegate(string data);

        public enum Requests
        {
            getAssessment
        }

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);

                setupAwake();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void setupAwake()
        {

        }

        private void Start()
        {
            setupRequests();
        }

        private void setupRequests()
        {
            requests.Add(Requests.getAssessment, getAssessmentEndpoint);
        }

        public void GetAssessmentStack(AssessmentRequestCallbackDelegate callback = null)
        {
            if (requestCoroutine != null) return;

            requestCoroutine = StartCoroutine(
                GetRequest(
                    url: $"{apiRoot}/{requests[Requests.getAssessment]}",
                    callback: 
                    (string data) => {
                        GetAssessmentData assesmentData = new GetAssessmentData();
                        assesmentData.jengaPieces = JsonHelper.FromJson<JengaPieceData>(JsonHelper.FormatJson(data));

                        if(callback != null)
                        {
                            callback(assesmentData);
                        }

                        StopCoroutine(requestCoroutine);
                    }
                )
            );
        }

        private IEnumerator GetRequest(string url, GetRequestCallbackDelegate callback = null)
        {
            Debug.Log($"From url : {url}");

            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                yield return request.SendWebRequest();

                switch(request.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                    Debug.Log($"Connection failed! : {request.error}");
                    break;
                    case UnityWebRequest.Result.ProtocolError:
                    Debug.Log($"Protocol error! : {request.error}");
                    break;
                    case UnityWebRequest.Result.Success:

                    string apiData = request.downloadHandler.text;

                    if (callback != null)
                    {
                        callback(apiData);
                    }

                    break;
                    default:
                    Debug.Log($"Unknown error! : {request.error}");
                    break;
                }
            }
        }
    }
}
