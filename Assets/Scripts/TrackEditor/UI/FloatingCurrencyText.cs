using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FloatingCurrencyText : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private Image image;

    private float moveSpeed = 100;
    private float vectorMulti = 1000;
    private Vector3 vectorDirection = Vector3.up;

    [SerializeField] private List<AudioClip> moneyClips = new List<AudioClip>();
    [SerializeField] private AudioSource moneyClipsSource;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DeleteAfterDelay", 1);
        moneyClipsSource.PlayOneShot(moneyClips[UnityEngine.Random.Range(0, moneyClips.Count)]);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, vectorDirection * vectorMulti, Time.deltaTime*moveSpeed);

        Color textColor = text.color;
        textColor.a -= Time.deltaTime;
        text.color = textColor;

        Color imageColor = image.color;
        imageColor.a -= Time.deltaTime;
        image.color = imageColor;
    }

    void DeleteAfterDelay()
    {
        Destroy(gameObject);
    }

    internal void SetupVariables(Vector3 position, int modificationAmount)
    {
        transform.position = position;

        if (modificationAmount > 0)
        {
            text.text = "+" + modificationAmount.ToString();
            text.color = Color.green;
        }
        else
        {
            text.text = modificationAmount.ToString();
            text.color = Color.red;
        }
    }
}
