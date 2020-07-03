using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WWWImageFetcher : MonoBehaviour {

    private static WWWImageFetcher _instance;

    private void Awake()
    {
        _instance = this;
    }

    public static void FillImageFromURL(Image img, string url)
    {
        _instance.StartCoroutine(_instance.setImage(img,url)); //balanced parens CAS
    }

    IEnumerator setImage(Image img, string url)
    {
        WWW www = new WWW(url);
        yield return www;

        img.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));

        www.Dispose();
        www = null;
    }
}
