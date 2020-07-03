using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class  RingUIMessage : UIMessage {

    [SerializeField]
    private TextMeshProUGUI _advantageLabel;

    public void SetupWithMessageAndAdvantage(string msg, string advantage) 
    {
        base.SetupWithMessage(msg);
        _advantageLabel.text = advantage;
    }

}
