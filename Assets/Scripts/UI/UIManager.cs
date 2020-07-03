using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum ScreenState{
    MAIN_MENU,
    PRE_GAME,
    GAME,
    END_SCREEN,
    LEADERBOARD,
}

public class UIManager : MonoBehaviour {

    [SerializeField]
    private GameObject _mainScreenObj;

    [SerializeField]
    private GameObject _preGameScreenObj;
    private PreGameScreen _preGameScreen;

    [SerializeField]
    private GameObject _gameScreenObj;

    [SerializeField]
    private GameObject _endScreenObj;
    private EndScreen _endScreen;

    [SerializeField]
    private GameObject _leaderboardScreenObj;
    private LeaderboardScreen _leaderboardScreen;

    [SerializeField]
    private Slider _slider;

    [SerializeField]
    private GameObject _attemptPrefab;

    [SerializeField]
    private Transform _attemptsParent;

    [SerializeField]
    private TextMeshProUGUI _scoreText;

    private List<GameObject> _attemptsUI;

    private int _gameTime;
    private bool _startedTimer
;
    private ScreenState _state;
    private GameObject _activeScreen;
    private float _timestampTimeStart;
    private int _remainingAttempts;
    private int _totalAttempts;
    private int score = 0;
    public float duration = 0.5f;

    private void Awake()
    {
        _attemptsUI = new List<GameObject>();
        _endScreen = _endScreenObj.GetComponent<EndScreen>();
        _leaderboardScreen = _leaderboardScreenObj.GetComponent<LeaderboardScreen>();
        _preGameScreen = _preGameScreenObj.GetComponent<PreGameScreen>();

        //disable all screens
        _mainScreenObj.SetActive(false);
        _preGameScreenObj.SetActive(false);
        _gameScreenObj.SetActive(false);
        _endScreenObj.SetActive(false);
        _leaderboardScreenObj.SetActive(false);
    }

    private void ChangeState(ScreenState state) {
        if (_activeScreen != null) {
            _activeScreen.SetActive(false);
        }

        switch(state) {
            case ScreenState.MAIN_MENU:
                _activeScreen = _mainScreenObj;
                break;
            case ScreenState.PRE_GAME:
                _activeScreen = _preGameScreenObj;
                break;
            case ScreenState.GAME:
                _activeScreen = _gameScreenObj;
                break;
            case ScreenState.END_SCREEN:
                _activeScreen = _endScreenObj;
                break;
            case ScreenState.LEADERBOARD:
                _activeScreen = _leaderboardScreenObj;
                break;
        }
        _activeScreen.SetActive(true);

    }

    private void Update()
    { 
        if (_startedTimer)
        {
            _slider.value -= Time.deltaTime;
        }
    }

    public void GoToMainMenu() {
        ChangeState(ScreenState.MAIN_MENU);
    }

    public void GoToPreGame() {
        ChangeState(ScreenState.PRE_GAME);
        _preGameScreen.Setup(); //Needs to happen AFTER change state so that the obj is active
    }

    public void GoToGame(int totalAttempts)
    {
        //Remove previous ones
        foreach(Transform child in _attemptsParent) {
            Destroy(child.gameObject);
        }
            
        _attemptsUI.Clear();

        _totalAttempts = totalAttempts;
        _remainingAttempts = totalAttempts;

        //Create attempt prefabs and add them to attempts parent
        for (int i = 0; i < totalAttempts - 1; i++) {
            GameObject attempt = Instantiate(_attemptPrefab, _attemptsParent) as GameObject;
            _attemptsUI.Add(attempt);
        }
        ChangeState(ScreenState.GAME);
    }

    public void LostAttempt() {
        //Hide another life
        _attemptsUI[_totalAttempts - _remainingAttempts].GetComponent<AttemptUI>().OnLost();
        _remainingAttempts--;
    }

    void LeaveGame() {
        _startedTimer = false;
    }

    public void GoToEndScreen() {
        LeaveGame();
        _endScreen.Setup();
        ChangeState(ScreenState.END_SCREEN);
    }

    public void GoBackToEndScreen()
    {
        ChangeState(ScreenState.END_SCREEN);
    }

    public void GoToLeaderboard(int score) {
        _leaderboardScreen.SetupForScore(score);
        ChangeState(ScreenState.LEADERBOARD);
    }

    public void RefreshLeaderboard() {
        _leaderboardScreen.OnRefreshScores();
    }

    public void StartTimer(int time)
    {
        _gameTime = time;
        _slider.maxValue = _gameTime;
        _slider.value = _gameTime;
        _timestampTimeStart = Time.time;
        _startedTimer = true;
    }

    public bool TimeHasEnded()
    {
        return _slider.value <= 0 && _startedTimer;
    }

    public float GetTimeLeft() {
        return _slider.value;
    }

    public void UpdateScore(int totalScore) {
        CountTo(totalScore);
        StartCount(totalScore);
    }

    public void OnScoreSuccessfullySubmitted()
    {
        _endScreen.OnScoreSuccessfullySubmitted();
        _leaderboardScreen.OnScoreSuccessfullySubmitted();

    }

    public void StartCount(int target)
    {
        StopCoroutine("CountTo");
        StartCoroutine("CountTo", target);
    }

    public IEnumerator CountTo(int target)
    {
        int start = score;
        for (float timer = 0; timer < duration; timer += Time.deltaTime)
        {
            float progress = timer / duration;
            score = (int)Mathf.Lerp(start, target, progress);
            yield return null;
            _scoreText.text = score.ToString();
        }
        score = target;
        _scoreText.text = score.ToString();
    }
}
