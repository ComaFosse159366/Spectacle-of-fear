using TMPro;
using UnityEngine;

public class textshaker : MonoBehaviour
{
    public int shake = 10;
    public float shakeAmount = 0.1f;
    public TMP_Text textComponent;
    private Vector3 originalPosition;

    private void Awake()
    {
        originalPosition = textComponent.transform.localPosition;
    }

    public void Update()
    {
        shakeText();
    }

    public void shakeText()
    {
        float offsetX = Random.Range(-shakeAmount, shakeAmount);
        float offsetY = Random.Range(-shakeAmount, shakeAmount);
        textComponent.transform.localPosition = originalPosition + new Vector3(offsetX, offsetY, 0f);
    }
}
