using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class ScoreSubmissionPopup : MonoBehaviour {

    public UnityEvent onScoreSuccessfullySubmited;

    [SerializeField]
    private TextMeshProUGUI _txtScore;

    [SerializeField]
    private TMP_InputField _inputName;

    [SerializeField]
    private TMP_InputField _inputMail;

    [SerializeField]
    private Toggle _wantsToKnowMore;

    private int _score;

	// Use this for initialization
	void Awake () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetupForScore(int score)
    {
        _score = score;
        _txtScore.text = score.ToString();
        _inputName.text = "";
        _inputMail.text = "";
        _wantsToKnowMore.isOn = false;
    }

    public void OnSumbitButtonPressed() {
        GameManager.Instance.GetScoreManager().PostNewScore(_inputName.text, _inputMail.text, _score, _wantsToKnowMore.isOn);
        onScoreSuccessfullySubmited.Invoke();

        Invoke("OnCloseButtonPressed",.5f); //Auto close popup!
    }

    public void OnCloseButtonPressed() {
        gameObject.SetActive(false);
    }
}
