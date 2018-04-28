using UnityEngine;
using System.Collections;
using IBM.Watson.DeveloperCloud.Logging;
using IBM.Watson.DeveloperCloud.Services.SpeechToText.v1;
using IBM.Watson.DeveloperCloud.Utilities;
using IBM.Watson.DeveloperCloud.DataTypes;
using System.Collections.Generic;
using UnityEngine.UI;
using IBM.Watson.DeveloperCloud.Services.Assistant.v1;
using FullSerializer;
using IBM.Watson.DeveloperCloud.Connection;

public class WatsonAssistant : MonoBehaviour {

    private string assistantUsername = "644f8f2c-0be8-4e05-a3be-bfe3c54db63b";
    private string assistantPassword = "6eOSuZVtxIFa";
    private string assisstantUrl = "https://gateway.watsonplatform.net/assistant/api";
    private string _workspaceId = "b42ee794-c019-4a0d-acd2-9e4d1d016767";

    private Assistant _assistant;

    private string testString = "marco";
    MessageRequest request;

    // Use this for initialization
    void Start ()
    {
        Credentials assistantCredentials = new Credentials(assistantUsername, assistantPassword, assisstantUrl);//my code
        _assistant = new Assistant(assistantCredentials); //my code
        //request.Input = <testString,object>;
        //_assistant.Message(OnMessage, OnFail, _workspaceId, "marco");
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnFail(RESTConnector.Error error, Dictionary<string, object> customData)
    {
        Log.Error("ExampleAssistant.OnFail()", "Error received: {0}", error.ToString());
    }

    //  Send a simple message using a string
    private void Message()
    {
        request = new MessageRequest();
        request.Input = new Dictionary<string, object>();
        request.Input.Add("Hi", true);
        if (!_assistant.Message(OnMessage, OnFail, _workspaceId, request) )
            Log.Debug("ExampleAssistant.Message()", "Failed to message!");
    }

    private void OnMessage(object resp, Dictionary<string, object> customData)
    {
        Log.Debug("ExampleAssistant.OnMessage()", "Assistant: Message Response: {0}", customData["json"].ToString());
    }

}
