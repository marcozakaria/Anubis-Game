using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IBM.Watson.DeveloperCloud.Services.LanguageTranslator.v2;
using IBM.Watson.DeveloperCloud.Logging;
using IBM.Watson.DeveloperCloud.Utilities;
using IBM.Watson.DeveloperCloud.Connection;

public class LangyageTranslatorDemoScript : MonoBehaviour {

    public Text textfield;
    public LanguageTranslator myLangTrans;

    private string TranslationModel = "en-es";

    // Use this for initialization
    void Start ()
    {
        LogSystem.InstallDefaultReactors();

        Credentials languageTranslatorCredentials = new Credentials()
        {
            Username = "9f81b835-d03e-4c1c-8550-02ee72956579",
            Password = "8otmVboVGDh7",
            Url = "https://gateway.watsonplatform.net/language-translator/api"
        };

        myLangTrans = new LanguageTranslator(languageTranslatorCredentials);

        //Translate("My name is marco , i live in Egypt");

    }

    public void Translate(string text)
    {
        myLangTrans.GetTranslation(OnTranslate,OnFail,text, TranslationModel);
        
    }
	
    private void OnTranslate(Translations response, Dictionary<string,object> customData)
    {
        textfield.text = response.translations[0].translation;
    }
    private void OnFail(RESTConnector.Error error,Dictionary<string, object> customData)
    {
        Log.Debug("Language translation error.OnFail", "Error: {0}", error.ToString());
    }
	
}
