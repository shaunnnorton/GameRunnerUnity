using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public TMP_InputField LoginUsername;
    public TMP_InputField LoginPassword;
    public TMP_InputField CreateUsername;
    public TMP_InputField CreatePassword;
    public TMP_InputField CreateGames;
    private string Username;
    private string Password;
    public GameObject PlayScreen;
    public GameObject WelcomScreen;
    public GameObject LoginScreen;
    public GameObject CreateScreen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDisable()
    {
        PlayerPrefs.SetString("Username", Username);
        PlayerPrefs.SetString("Password", Password);

    }


    IEnumerator CreateUser()
    {
        if(CreateUsername.text.Length>3 && CreatePassword.text.Length>3 && CreateGames.text.Length > 5)
        {
            string API_KEY = "TURKEYDAY";
            string NAME = CreateUsername.text;
            string PASSWORD = CreatePassword.text;
            WWWForm data = new WWWForm();
            data.AddField("KEY", API_KEY);
            data.AddField("NAME", NAME);
            data.AddField("PASSWORD", PASSWORD);

            UnityWebRequest request;
            request = UnityWebRequest.Post("http://127.0.0.1:5000/API/Create/User", data);

            yield return request.SendWebRequest();

            data.AddField("GAMES", CreateGames.text);

            request = UnityWebRequest.Post("http://127.0.0.1:5000/API/Add/Games", data);
            request.downloadHandler = new DownloadHandlerBuffer();

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                // Show results as text
                /*Debug.Log(request.downloadHandler.text);*/
                var ggg = JSON.Parse(request.downloadHandler.text);
                Debug.Log(ggg["Data"]);
                if (ggg["Response"] == "SUCCESS")
                {
                    Username = NAME;
                    Password = PASSWORD;
                    CreateScreen.SetActive(false);
                    PlayScreen.SetActive(true);
                }


            }

           

        }
    }

    IEnumerator Login()
    {
        string NAME = LoginUsername.text;
        string PASSWORD = LoginPassword.text;
        WWWForm data = new WWWForm();
        data.AddField("NAME", NAME);
        data.AddField("PASSWORD", PASSWORD);

        UnityWebRequest request;

        request = UnityWebRequest.Post("http://127.0.0.1:5000/API/User", data);
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            // Show results as text
            /*Debug.Log(request.downloadHandler.text);*/
            var ggg = JSON.Parse(request.downloadHandler.text);
            if (ggg["Response"] == "SUCCESS")
            {
                Username = NAME;
                Password = PASSWORD;
                LoginScreen.SetActive(false);
                PlayScreen.SetActive(true);
            }
            Debug.Log(ggg["Data"]);


        }
    }

    public void SetDefaults()
    {
        Username = "Default";
        Password = "PASSWORD";
        WelcomScreen.SetActive(false);
        PlayScreen.SetActive(true);
    }

    public void Play()
    {
        SceneManager.LoadScene("WorldScene");
    }

}
