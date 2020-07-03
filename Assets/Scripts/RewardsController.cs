using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Attempt {
    public bool caughtRing1;
    public bool caughtRing2;
    public bool caughtRing3;
}

public class RewardsController : MonoBehaviour {
    public int timeLeft;

    private List<Attempt> _attempts;

    public void Reset()
    {
        _attempts = new List<Attempt>();
    }

    public void AddNewAttempt(Attempt attempt) {
        _attempts.Add(attempt);
    }

    public int TimesCaughtRing1() {
        int timeCaught = 0;
        foreach(Attempt a in _attempts) {
            if (a.caughtRing1) {
                timeCaught++;
            }
        }

        return timeCaught;
    }

    public int TimesCaughtRing2()
    {
        int timeCaught = 0;
        foreach (Attempt a in _attempts)
        {
            if (a.caughtRing2)
            {
                timeCaught++;
            }
        }

        return timeCaught;
    }

    public int TimesCaughtRing3()
    {
        int timeCaught = 0;
        foreach (Attempt a in _attempts)
        {
            if (a.caughtRing3)
            {
                timeCaught++;
            }
        }

        return timeCaught;
    }

    public int ScoreForRing(int ringNumber) {
        int score = 0;

        switch(ringNumber) {
            case 1:
                foreach (Attempt a in _attempts)
                {
                    if (a.caughtRing1)
                    {
                        score += GameManager.Instance.scoreFromRing1;
                        break; //Only score once for ring
                    }
                }
                break;
            case 2:
                foreach (Attempt a in _attempts)
                {
                    if (a.caughtRing2)
                    {
                        score += GameManager.Instance.scoreFromRing2;
                        break; //Only score once for ring
                    }
                }
                break;
            case 3:
                foreach (Attempt a in _attempts)
                {
                    if (a.caughtRing3)
                    {
                        score += GameManager.Instance.scoreFromRing3;
                        break; //Only score once for ring
                    }
                }
                break;
            default:
                Debug.LogError("Got unknown ring number: " + ringNumber);
                break;
        }

        return score;
    }
}
