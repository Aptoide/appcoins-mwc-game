using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PreGameScreen : MonoBehaviour {

    [SerializeField]
    private Sprite[] _numbers;

    [SerializeField]
    private Image _cleanBg;

    [SerializeField]
    private Image _gradientBg;

    [SerializeField]
    private Image _numberImg;

    [SerializeField]
    private TextMeshProUGUI _textUnleash;
    [SerializeField]
    private TextMeshProUGUI _textYourApps;

    private float _timeBetweenNumbers = 0.75f;

    public void Setup() {
        StopAllCoroutines();
        StartCoroutine("ShowGameCountdown");
    }

    IEnumerator ShowGameCountdown() {
        //Setup. gradient background and image on, all the rest off
        _numberImg.gameObject.SetActive(true);
        _gradientBg.gameObject.SetActive(true);
        _cleanBg.gameObject.SetActive(false);
        _textUnleash.gameObject.SetActive(false);
        _textYourApps.gameObject.SetActive(false);

        _numberImg.sprite = _numbers[0];
        yield return new WaitForSeconds(_timeBetweenNumbers);
        _numberImg.sprite = _numbers[1];
        yield return new WaitForSeconds(_timeBetweenNumbers);
        _numberImg.sprite = _numbers[2];
        yield return new WaitForSeconds(_timeBetweenNumbers);

        //Setup. gradient background and image off, clean background and text on
        _numberImg.gameObject.SetActive(false);
        _gradientBg.gameObject.SetActive(false);
        _cleanBg.gameObject.SetActive(true);
        _textUnleash.gameObject.SetActive(true);
        _textYourApps.gameObject.SetActive(true);

        yield return new WaitForSeconds(_timeBetweenNumbers * 2);

        GameManager.Instance.StartGame();
        yield break;
    }

	// Update is called once per frame
	void Update () {
		
	}
}
