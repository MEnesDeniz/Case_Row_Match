using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SuccessScreen : MonoBehaviour
{
    public GameObject cloud;
    public GameObject maxScore;

    private void Awake()
    {
        this.gameObject.SetActive(false);

        var _transform = this.transform;
        maxScore = _transform.GetChild(5).gameObject;
        cloud = _transform.GetChild(3).gameObject;
    }

    public void setScore(int _maxScore)
    {
        this.maxScore.GetComponent<TextMeshPro>().text = _maxScore.ToString();
    }
}
