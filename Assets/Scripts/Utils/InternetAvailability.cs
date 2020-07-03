using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InternetAvailability : MonoBehaviour {

    public static bool HasInternet() {
        return Application.internetReachability != NetworkReachability.NotReachable;
    }

}
