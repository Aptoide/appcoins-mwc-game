using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MWCDataFetch : MonoBehaviour {

    public Advantage[] advantages;

    public const string DOC_ID = "1SeIJrLwGZUxOTYcEo9ikq066EeQ_0g-lFdLxxv1t9gM";
    public const string LOCAL_FILENAME = "MWC_copy";
	
    // Use this for initialization
	void Start () {
        Action<string> commCallback = (csv) =>
        {
            Debug.Log("The loaded data is " + csv);

            int advantageIndex = 0;

            List<List<string>> parsedValues = GoogleSheetsDataFetcher.ParseCSV(csv);
            foreach(List<string> list in parsedValues) {
                if (list.Count == 3) {
                    string title = list[0];
                    string text = list[1];
                    string imgUrl = list[2];

                    advantages[advantageIndex].Init(title, text, imgUrl);
                    advantageIndex++;
                } else {
                    Debug.LogError("Got data with wrong format! List count " + list.Count);
                }  
            }
        };

        //Fetch the data from the remote sheet
        if (InternetAvailability.HasInternet()) {
            StartCoroutine(GoogleSheetsDataFetcher.DownloadCSVCoroutine(DOC_ID, commCallback, true, LOCAL_FILENAME));    
        } else {
            LocalFetchContent(LOCAL_FILENAME, commCallback);
        }
	}
	
    void LocalFetchContent(string filename, Action<string> action) {
        string text = File.ReadAllText("Assets/ Resources/" + filename + ".csv");
        action(text);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
