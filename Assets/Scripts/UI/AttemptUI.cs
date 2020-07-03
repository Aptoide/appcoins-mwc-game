using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttemptUI : MonoBehaviour {

    [SerializeField]
    private Animator _animator;

    public void OnLost() {
        _animator.Play("attemptLost");
    }

    public void OnLostAnimDone() {
        this.gameObject.SetActive(false);
    }

	// Use this for initialization
	void Awake () {
        _animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
