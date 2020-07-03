using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMessageSpawner : MonoBehaviour {

    public static GameObject SpawnMessageOnPositionUsingPrefab(Vector2 pos, GameObject prefab, Transform parent) {
        GameObject obj = (GameObject)Instantiate(prefab, parent);
        obj.transform.position = pos;

        return obj;
    }
}
