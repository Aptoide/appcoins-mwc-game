using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public struct CSVScoreEntry {
    public string name;
    public int score;
}

public class ScoreManager : MonoBehaviour {

    public const string SCORE_DOC_ID = "1lXH8ha690B-zZWmSIl8t6g5XgvgpBRjw0lty5BxtP4Y";
    public const string LOCAL_FILENAME = "sheetScores";

    private SendToGoogle _sendToGoogle;
    private CSVWriter _csvWriter;

    private List<CSVScoreEntry> _sortedScores;

	// Use this for initialization
	void Awake () {
        _sendToGoogle = GetComponent<SendToGoogle>();
        _csvWriter = GetComponent<CSVWriter>();
        //Invoke("Test", 3);

        LoadScores();
	}

    public void PostNewScore(string name, string mail, int score, bool wantsMore) {
        //NOTE: Make strings safe by removing commas
        name = name.Replace(",", ".");
        mail = mail.Replace(",", ".");


        string[] values = new string[4];
        values[0] = name;
        values[1] = mail;
        values[2] = score.ToString();
        values[3] = wantsMore ? "YES" : "NO";

        _csvWriter.AddNewEntry(values);

        if (InternetAvailability.HasInternet())
        {
            _sendToGoogle.Send(name, mail, wantsMore ? "YES" : "NO", score.ToString());
        } 
    }

    void LoadScores() {
        _sortedScores = new List<CSVScoreEntry>();

        //TODO check for internet connection and get the scores from google sheets 
        //if (InternetAvailability.HasInternet()) {
        //    ;
        //    //            StartCoroutine(GoogleSheetsDataFetcher.DownloadCSVCoroutine(SCORE_DOC_ID, commCallback, true, LOCAL_FILENAME));    
        //} else {
        //Check for file existence and load the stored scores;
        string filename = _csvWriter.filenameNoExtension;
        if (System.IO.File.Exists(_csvWriter.GetPath(filename)))
            {
            StreamReader reader = new StreamReader(_csvWriter.GetPath(filename));

                reader.ReadLine(); //Discard first line because it's the csv columns names
                while (true)
                {
                    string line = reader.ReadLine();
                    if (line == null)
                    {
                        break;
                    }

                    ProcessScoreLine(line);
                }

                _sortedScores.Sort((x, y) => y.score.CompareTo(x.score));

                reader.Close();
                reader.Dispose();
            }   
        //}
    }

    public void UpdateScores() {
        LoadScores();
    }

    void ProcessScoreLine(string line) {
        Debug.Log("Got score line of " + line);

        string[] parts = line.Split(',');

        //index 1 is name and index 3 is score
        CSVScoreEntry entry = new CSVScoreEntry();
        entry.name = parts[1];
        entry.score = Convert.ToInt32(parts[3]);

        Debug.Log("Adding csv score entry with name: " + entry.name + " score: " + entry.score);
        _sortedScores.Add(entry);
    }

    public List<CSVScoreEntry> GetSortedScores() {
        return _sortedScores;
    }

    public void DeleteScoresFile() {
        string filename = _csvWriter.filenameNoExtension;
        if (System.IO.File.Exists(_csvWriter.GetPath(filename))) {
            System.IO.File.Delete(_csvWriter.GetPath(filename));
        }
    }
}
