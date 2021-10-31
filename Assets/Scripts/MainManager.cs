using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    public Text HighScoreText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    private int oldScore;

    // Start is called before the first frame update
    void Start()
    {
        oldScore = UIMenuManager.Instance.ReadScore();
        SetHighScoreText(oldScore, UIMenuManager.Instance.ReadHighScoreName());
        AddPoint(0);
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        string playerName = UIMenuManager.Instance.GetPlayerName();
        ScoreText.text = $"{playerName}´s Score : {m_Points}";
        if (m_Points > oldScore)
        {
            SetHighScoreText(m_Points, playerName);
        }
    }

    public void GameOver()
    {
        if (m_Points > oldScore)
        {
            UIMenuManager.Instance.SaveScore(m_Points);
        }
        m_GameOver = true;
        GameOverText.SetActive(true);
    }

    public void SetHighScoreText(int points, string playerName)
    {
        HighScoreText.text = $"Best Score : {playerName} : {points}";
    }
}
