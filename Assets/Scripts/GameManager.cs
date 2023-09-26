using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : SingletonPersistent<GameManager>
{
    [SerializeField] private GameObject timerText;
    [SerializeField] private float extraTimeScalePerSecond;
    [SerializeField] private float maxTimeScale = 30;

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = Mathf.Clamp(1 + Time.timeSinceLevelLoad * extraTimeScalePerSecond, 0, maxTimeScale);

        if (timerText != null)
        {
            timerText.GetComponent<TMP_Text>().text = ((int)Time.timeSinceLevelLoad).ToString();
        }
    }

    public void Die()
    {
        SceneManagerExtended.ReloadScene();
    }
}
