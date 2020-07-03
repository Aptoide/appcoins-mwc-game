using System.Collections;
using System.Collections.Generic;

using System.ComponentModel;
using UnityEngine;

public partial class SROptions
{

    [Category("Scores")]
    public void ClearLeaderboardEntries()
    {
        GameManager.Instance.ClearLeaderboardEntries();
    }

    [Category("General")]
    public void Quit() {
        Application.Quit();
    }

    //[Category("Config")]
    //public int Force
    //{
    //    get { return GameManager.Instance.MaxForce; }
    //    set { GameManager.Instance.MaxForce = value; }
    //}

    //public int Distance
    //{
    //    get { return GameManager.Instance.MaxDistance; }
    //    set { GameManager.Instance.MaxDistance = value; }
    //}
}