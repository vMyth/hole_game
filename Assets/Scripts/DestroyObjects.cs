using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DestroyObjects : MonoBehaviour
{
    public Text score;
    int s = 0;
    public OnChangePosition holeScript;
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.gameObject);
        TrackProgress();
    }
    private void TrackProgress()
    {

        s++;
        score.text = "SCORE: " + s.ToString();
        if (s % 5 == 0)
        {
            StartCoroutine(holeScript.ScaleHole());
        }
    }
    public void Reload()
    {
        SceneManager.LoadScene(0);
    }
}
