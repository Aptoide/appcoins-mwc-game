using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIMessage : MonoBehaviour {

    [SerializeField]
    private TextMeshProUGUI _messageLabel;

    public void OnAnimationDone( ) {
        Destroy(this.gameObject);
    }

    public void SetupWithMessage(string msg) {
        _messageLabel.text = msg;
    }
}
