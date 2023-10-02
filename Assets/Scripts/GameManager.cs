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

    private float actualTime;
    

    // Update is called once per frame
    void Update()
    {
        actualTime += Time.deltaTime / Time.timeScale;
        
        if (SceneManager.GetActiveScene().buildIndex == gameSceneBuildIndex)
        {
            Time.timeScale = Mathf.Clamp(1 + Time.timeSinceLevelLoad * extraTimeScalePerSecond, 0, maxTimeScale);

            if (timeText != null)
            {
                timeText.text = ((int)actualTime).ToString() + "s";
            }
        }
    }

    public void Die()
    {
        SceneManagerExtended.ReloadScene();
        actualTime = 0;
        Invoke("GetTimeText", 0.1f);
    }
    
    public void Quit()
    {
        Application.Quit();
    }

    public void LoadGameScene()
    {
        SceneManagerExtended.LoadScene(gameSceneBuildIndex);
        //GetTimeText();
        Invoke("GetTimeText", 0.1f);
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
