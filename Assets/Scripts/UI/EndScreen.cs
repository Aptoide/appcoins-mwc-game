using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndScreen : MonoBehaviour {
    [SerializeField]
    private Image _timerImg;
    [SerializeField]
    private Sprite[] _timerImages;
    [SerializeField]
    private GameObject[] _liveImages;
    [SerializeField]
    private Sprite activeAdvantageSprite;
    [SerializeField]
    private Sprite inactiveAdvantageSprite;

    [SerializeField]
    private GameObject _btnSubmitScore;

    [SerializeField]
    private TextMeshProUGUI _txtFinalScore;
    [SerializeField]
    private TextMeshProUGUI _txtAdvantage1Score;
    [SerializeField]
    private TextMeshProUGUI _txtAdvantage2Score;
    [SerializeField]
    private TextMeshProUGUI _txtAdvantage3Score;
    [SerializeField]
    private TextMeshProUGUI _txtTimeScore;
    [SerializeField]
    private TextMeshProUGUI _txtLivesScore;
    [SerializeField]
    private Image _imgAdvantage1;
    [SerializeField]
    private Image _imgAdvantage2;
    [SerializeField]
    private Image _imgAdvantage3;

	// Use this for initialization
	void Start () {
	    	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void SolveScoreForAdvantage(int advantageScore, Image advantageImage, TextMeshProUGUI advantageText) {
        if (advantageScore > 0)
            advantageImage.sprite = activeAdvantageSprite;
        else
            advantageImage.sprite = inactiveAdvantageSprite;
        
        advantageText.text = "x " + advantageScore.ToString();

    }

    public void Setup()
    {
        _btnSubmitScore.SetActive(true);
        //_btnScoreSubmitted.SetActive(false);

        RewardsController rewardsController = GameManager.Instance.GetRewardsController();

        int advantage1Score = rewardsController.ScoreForRing(1);
        int advantage2Score = rewardsController.ScoreForRing(2);
        int advantage3Score = rewardsController.ScoreForRing(3);
        int portalTotalScore = advantage1Score + advantage2Score + advantage3Score;

        int timeScore = portalTotalScore > 0 ? ((int)rewardsController.timeLeft) * GameManager.Instance.scoreFromTime : 0; //Only count if throws were made
        int livesLeft = GameManager.Instance.GetAttemptsLeft();
        int livesScore = portalTotalScore > 0 ? livesLeft * GameManager.Instance.scoreFromLives : 0; //Only count if throws were made

        _txtTimeScore.text = "x " + timeScore.ToString();
        _txtLivesScore.text = "x " + livesScore.ToString();

        //Disable the images according to amount of lives left
        switch(livesLeft) {
            case 0:
            case 3:
                _liveImages[0].SetActive(false);
                _liveImages[1].SetActive(false);
                _liveImages[2].SetActive(true); //This one is greyd out
                break;
            case 1:
                _liveImages[0].SetActive(true);
                _liveImages[1].SetActive(false);
                _liveImages[2].SetActive(false);
                break;
            case 2:
                _liveImages[0].SetActive(true);
                _liveImages[1].SetActive(true);
                _liveImages[2].SetActive(false);
                break;
        }

        SolveScoreForAdvantage(advantage1Score, _imgAdvantage1, _txtAdvantage1Score);
        SolveScoreForAdvantage(advantage2Score, _imgAdvantage2, _txtAdvantage2Score);
        SolveScoreForAdvantage(advantage3Score, _imgAdvantage3, _txtAdvantage3Score);

        if (timeScore == 0) {
            _timerImg.sprite = _timerImages[0];
        } else {
            _timerImg.sprite = _timerImages[1];
        }

        int finalScore = timeScore + portalTotalScore + livesScore;
        GameManager.Instance.UpdateLatestScore(finalScore);
        _txtFinalScore.text = "x " + finalScore.ToString();
    }

    public void OnRetryButtonPressed() {
        GameManager.Instance.StartGame();
    }

    public void OnNewGameButtonPressed()
    {
        GameManager.Instance.GoToMainMenu();
    }

    public void OnSubmitButtonPressed() {
        GameManager.Instance.GoToScoreSubmission();
    }

    public void OnScoreSuccessfullySubmitted() {
        _btnSubmitScore.SetActive(false);
        //_btnScoreSubmitted.SetActive(true);
    }   
}
