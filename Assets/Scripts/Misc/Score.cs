using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public static int scoreCount = 0;
    public Text score;

    private void Start() {
        score = GetComponent<Text>();
    }

    private void Update() {
        score.text = "" + scoreCount;
    }
}
