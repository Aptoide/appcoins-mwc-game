using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Advantage : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI  _title;

    [SerializeField]
    private TextMeshProUGUI _text;

    [SerializeField]
    private Image _image;

    public void Init(string title, string text, string imgURL)
    {
        _title.text = title;
        _text.text = text;

        //This was to debug appearance
        //if (InternetAvailability.HasInternet())
            //WWWImageFetcher.FillImageFromURL(_image, imgURL);
    }
}
