using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class creditScript : MonoBehaviour
{
    public float scrollSpeed = 40f;
    private RectTransform credits;

    void Start()
    {
        credits = GetComponent<RectTransform>();
    }

    void Update()
    {
        credits.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);
    }
}
