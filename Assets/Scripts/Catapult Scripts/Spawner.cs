using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameObject projectilePrefab;
    public Transform spawnPosition;

    public void SpawnProjectile()
    {
        GameObject spawn = Instantiate(projectilePrefab, spawnPosition.position, Quaternion.identity, GameManager.Instance.GetGameInstanceRoot()) as GameObject;
    }
}
