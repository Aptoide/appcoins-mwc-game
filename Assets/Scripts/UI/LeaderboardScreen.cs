using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardScreen : MonoBehaviour {

    [SerializeField]
    private GameObject _leaderboardEntryPrefab;
    [SerializeField]
    private Transform _scrollParent;

    [SerializeField]
    private GameObject _submissionPopupObj;
    private ScoreSubmissionPopup _submissionPopup;

	// Use this for initialization
	void Awake () {
        _submissionPopup = _submissionPopupObj.GetComponent<ScoreSubmissionPopup>();
        _submissionPopup.onScoreSuccessfullySubmited.AddListener(GameManager.Instance.GetUIManager().OnScoreSuccessfullySubmitted);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void ListScores() {
        //Clear list
        foreach (Transform child in _scrollParent)
        {
            GameObject.Destroy(child.gameObject);
        }

        ScoreManager scoreManager = GameManager.Instance.GetScoreManager();
        scoreManager.UpdateScores();
        List<CSVScoreEntry> scores = scoreManager.GetSortedScores();

        int place = 1;
        foreach (CSVScoreEntry entry in scores)
        {
            CreateScoreEntry(place,entry);
            place++;
        }
    }

    public void SetupForScore(int score) {
        //Fill in the scores scroll from scoreManager
        ListScores();

        if (_submissionPopup == null)
            _submissionPopup = _submissionPopupObj.GetComponent<ScoreSubmissionPopup>();

        _submissionPopupObj.SetActive(true);
        _submissionPopup.SetupForScore(score);
    }

    public void OnBackButtonPressed() {
        GameManager.Instance.GoBack();
    }

    public void OnScoreSuccessfullySubmitted() {
        ListScores();
    }

    void CreateScoreEntry(int place, CSVScoreEntry entry) {
        GameObject obj = (GameObject)Instantiate(_leaderboardEntryPrefab, _scrollParent);
        LeaderboardEntry leaderboardEntry = obj.GetComponent<LeaderboardEntry>();
        leaderboardEntry.SetupWithScoreEntry(place, entry);
    }

    public void OnRefreshScores() {
        ListScores();
    }
}
