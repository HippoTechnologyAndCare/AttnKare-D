using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudDataManager : MonoBehaviour
{
    private string myAPIKey;

    private void Start()
    {
        
        myAPIKey = "13de814c5d55";                
    }

    public IEnumerator UploadRoutine()
    {
        // Create a new ES3Cloud object with the URL to our ES3.php file.
        var cloud = new ES3Cloud("https://hippotnc.synology.me/ES3Cloud.php", "myAPIKey");                

        // Upload another local file, but make it global for all users.
        //yield return StartCoroutine(cloud.UploadFile("D:/python_test/" + JsonManager.GetInstance().userInfomation + ".json"));
        yield return cloud.UploadFile(JsonManager.GetInstance().userInformation + ".json");

        if (cloud.isError)
            Debug.LogError(cloud.error);

        else
            Debug.Log("Uploaded");
    }

}
