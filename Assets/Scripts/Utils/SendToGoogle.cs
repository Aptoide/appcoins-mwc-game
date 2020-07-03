using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct Entry{
    public string name;
    public string fieldId;
}


public class SendToGoogle : MonoBehaviour {

    public Entry[] entries;

    [SerializeField]
    private string BASE_URL = "https://docs.google.com/forms/d/e/1FAIpQLSeAC0CYd1m9hvphv28eSv-ot4lZpszLDUmQa_B76vAEjZ-6Ow/formResponse";

    private string[] _fields;

    private void Awake()
    {
        _fields = new string[entries.Length];

        int fieldIndex = 0;
        foreach(Entry entry in entries) {
            _fields[fieldIndex] = entry.fieldId;
            fieldIndex++;
        }

        //Invoke("Test", 3);
    }

    void Test() {
        Send("Nuno Monteiro", "mail2@mail.com", "NO", "456");
    }

    public void Send(string name, string mail, string wantsMore, string score) {
        string[] values = new string[entries.Length];
        values[0] = name;
        values[1] = mail;
        values[2] = wantsMore;
        values[3] = score;

        Send(values);
    }

    IEnumerator Post(string[] fields, string[] values) {
        WWWForm form = new WWWForm();

        int valueIndex = 0;
        foreach(string field in fields) {
            //Debug.Log("adding field " + field + " adding value " + values[valueIndex]);
            form.AddField(field, values[valueIndex]);
            valueIndex++;
        }

        byte[] rawData = form.data;
        WWW www = new WWW(BASE_URL, rawData);
        yield return www;
    }
    public void Send(string[] values) {
        StartCoroutine(Post(_fields,values));
    }
}
