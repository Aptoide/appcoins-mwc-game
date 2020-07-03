using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Purchasing;
using System.Text.RegularExpressions;

[Serializable]
public class Receipt {
  public string Store;
    public string TransactionID;
    public Payload payload;
}

[Serializable]
public class Payload {
    public string  ItemType;
    public string ProductId;
    public  string GameOrderId;
    public string OrderQueryToken;
    public StorePurchase storePurchase;
    public string transactionId;
    public string storeSpecificId;
}

[Serializable]
public class StorePurchase {
    public string cpOrderId;
    public string currency;
    public string developerPayload;
    public string orderId;
    public string payload;
    public string price;
    public string productId;
    public string purchaseToken;
    public string signature;
    public int totalAmount;
}

public class MainMenuScreen : MonoBehaviour {

    [SerializeField]
    private GameObject _purchaseBtn;

	// Use this for initialization
	void Start () {
        if (GameManager.Instance.HasPurchasedFullTrajectory()) {
            _purchaseBtn.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnPlayButtonPressed() {
        GameManager.Instance.StartCountdown();
    }

    public void OnPurchaseSuccessful(Product p) {
        Debug.Log("purchase successful with receipt: " + p.receipt);

        StartCoroutine(PerformServerSideCheck(p.receipt));
    }

    IEnumerator PerformServerSideCheck(string receiptJSONStr)
    {
        string packageName = Application.identifier;

        receiptJSONStr = receiptJSONStr.Replace("\\", "");
        receiptJSONStr = receiptJSONStr.Replace("}\",\"", "},\"");
        receiptJSONStr = receiptJSONStr.Replace("}\"}", "}}");
        receiptJSONStr = receiptJSONStr.Replace("\"Payload\":\"{", "\"Payload\":{");
        receiptJSONStr = receiptJSONStr.Replace("\"StorePurchaseJsonString\":\"{", "\"StorePurchaseJsonString\":{");

        Debug.Log("Going to attempt server side check!" + receiptJSONStr);
        Match m = Regex.Match(receiptJSONStr, "purchaseToken\":\"([a-zA-Z0-9.]+)"); //Format is catappult.inapp.purchase.RnNLOUNSYkREMjhXd3IwSg
        string purchaseToken = "";
        
        if (m.Groups.Count > 0) {
            purchaseToken = m.Groups[1].Captures[0].Value;
        }
        
        m = Regex.Match(receiptJSONStr, "storeSpecificId\":\"(\\w+)");
        string sku = "";

        if (m.Groups.Count > 0)
        {
            sku = m.Groups[1].Captures[0].Value;
        }
        //Receipt r = JsonUtility.FromJson<Receipt>(receiptJSONStr);
        //r.payload = JsonUtility.FromJson<Payload>(receiptJSONStr);
        //r.payload.storePurchase = JsonUtility.FromJson<StorePurchase>(receiptJSONStr);

        //Debug.Log("payload token: " + r.payload.OrderQueryToken + "\n payload store specific id: " + r.payload.storeSpecificId);
        Debug.Log("payload token: " + purchaseToken + "\n payload store specific id: " + sku);
        //if (r.payload.OrderQueryToken != null && r.payload.storeSpecificId != null)
        if (purchaseToken != "" && sku != "")
        {
            WWWForm form = new WWWForm();
            //form.AddField("token", r.payload.OrderQueryToken);
            //form.AddField("product", r.payload.storeSpecificId);
            form.AddField("token", purchaseToken);
            form.AddField("product", sku);

            string url = "https://validators.aptoide.com/purchase/" + packageName + "/check";

            Debug.Log("Going to post:\n" + form + "\n to " + url);

            using (UnityWebRequest www = UnityWebRequest.Post(url, form))
            {
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    //Need to check response
                    string response = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);

                    m = Regex.Match(response, "status\": \"(\\w+)");
                    string status = "";

                    if (m.Groups.Count > 0)
                    {
                        status = m.Groups[1].Captures[0].Value;
                    }

                    if (status == "SUCCESS") {
                        Debug.Log("Purchase verified!");
                        GameManager.Instance.OnFullTrajectoryPurchased();
                        _purchaseBtn.SetActive(false);
                    } else
                    {
                        Debug.LogError("Failed to validate purchase");
                    }
                }
            }
        }
    }

}
