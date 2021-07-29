using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudDataManager : MonoBehaviour
{
    private string myAPIKey;

    private void Start()
    {
        myAPIKey = "13de814c5d55";
        ES3Settings.pathToEasySaveFolder = "D:/python_test/";        
    }

    public IEnumerator UploadToCloud()
    {
        // Create a new ES3Cloud object with the URL to our ES3.php file.
        var cloud = new ES3Cloud("https://hippotnc.synology.me/ES3Cloud.php", "myAPIKey");                

        // Upload another local file, but make it global for all users.
        yield return StartCoroutine(cloud.UploadFile(JsonManager.GetInstance().userInfomation + ".json"));

        if (cloud.isError)
            Debug.LogError(cloud.error);
    }

}
