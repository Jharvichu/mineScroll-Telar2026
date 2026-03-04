using UnityEngine;
using UnityEngine.UI;
using System;

public class CircularQTE : MonoBehaviour
{
    [Header("UI References")]
    public Transform needle;          
    public Image targetZoneImage;    

    private float currentAngle = 0f;
    private float speed = 150f;
    private float minAngle;
    private float maxAngle;

    private Action onSuccess;
    private Action onFail;
    private bool isPlaying = false;

    
    public void StartQTE(float needleSpeed, float zoneStart, float zoneSize, Vector2 screenPosition, Action successCallback, Action failCallback)
    {
        speed = needleSpeed;
        minAngle = zoneStart;
        maxAngle = zoneStart + zoneSize;

        
        RectTransform rect = GetComponent<RectTransform>();
        rect.anchoredPosition = screenPosition;

        
        targetZoneImage.fillAmount = zoneSize / 360f; 
        targetZoneImage.rectTransform.localRotation = Quaternion.Euler(0, 0, -zoneStart);

        onSuccess = successCallback;
        onFail = failCallback;
        currentAngle = 0f;
        isPlaying = true;
        gameObject.SetActive(true); 
    }

    void Update()
    {
        if (!isPlaying) return;

        
        currentAngle += speed * Time.deltaTime;
        if (currentAngle >= 360f) currentAngle -= 360f; 

        needle.localRotation = Quaternion.Euler(0, 0, -currentAngle);

        if (Input.GetKeyDown(KeyCode.F))
        {
            CheckResult();
        }
    }

    void CheckResult()
    {
        isPlaying = false;
        
        
        gameObject.SetActive(false); 

        
        if (currentAngle >= minAngle && currentAngle <= maxAngle)
        {
            onSuccess?.Invoke();
        }
        else
        {
            onFail?.Invoke();
        }
    }
}