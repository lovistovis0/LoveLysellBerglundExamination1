using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : SingletonPersistent<GameManager>
{
    [SerializeField] private int gameSceneBuildIndex = 1;
    [SerializeField] private string timeTextTag = "TimeText";
    [SerializeField] private float extraTimeScalePerSecond;
    [SerializeField] private float maxTimeScale = 30;

    private TMP_Text timeText;
    

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == gameSceneBuildIndex)
        {
            Time.timeScale = Mathf.Clamp(1 + Time.timeSinceLevelLoad * extraTimeScalePerSecond, 0, maxTimeScale);

            if (timeText != null)
            {
                timeText.text = ((int)Time.timeSinceLevelLoad).ToString() + "S";
            }
        }
    }

    public void Die()
    {
        SceneManagerExtended.ReloadScene();
        Invoke("GetTimeText", 0.04f);
    }
    
    public void Quit()
    {
        Application.Quit();
    }

    public void LoadGameScene()
    {
        SceneManagerExtended.LoadScene(gameSceneBuildIndex);
        //GetTimeText();
        Invoke("GetTimeText", 0.04f);
    }

    private void GetTimeText()
    {
        GameObject obj = GameObject.FindWithTag(timeTextTag);
        if (obj == null)
        {
            Debug.LogWarning("No timeText obj found");
            return;
        }

        timeText = obj.GetComponent<TMP_Text>();
    }
}
