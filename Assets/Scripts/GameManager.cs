using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    public int level = 0;   // 스테이지 (레벨이 0인 경우 타이틀 화면)

    private RectTransform m_TitleCursor;
    private enum CursorState {Start, Exit}
    private CursorState m_CursorState = CursorState.Start;
    private GameObject youDied;
    private Text m_LevelText, m_LevelTextShadow;
    private GameObject tutorialTextForCow, tutorialTextForBird;
    private PlayerController player;

    // Start is called before the first frame update
    void Awake()
    {
        if (gm != null)
        {
            Destroy(this.gameObject);
            return;
        }
        gm = this;
        DontDestroyOnLoad(this);
    }
    
    void Update()
    {
        if(level > 0)
        {
            m_LevelText = GameObject.FindGameObjectWithTag("UILevelText").GetComponent<Text>();
            m_LevelTextShadow = GameObject.FindGameObjectWithTag("UILevelTextShadow").GetComponent<Text>();

            m_LevelText.text = level + " Stage";
            m_LevelTextShadow.text = level + " Stage";
        }
        if (level > 0 && youDied == null && GameObject.Find("YouDied") != null) youDied = GameObject.Find("YouDied");
        if (level == 0)
        {
            if (m_TitleCursor == null)
            {
                m_TitleCursor = GameObject.FindGameObjectWithTag("Cursor").GetComponent<RectTransform>();
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                m_CursorState = (CursorState)((int)(m_CursorState + 1) % System.Enum.GetNames(typeof(CursorState)).Length);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                m_CursorState = (CursorState)((int)(m_CursorState + System.Enum.GetNames(typeof(CursorState)).Length - 1) % System.Enum.GetNames(typeof(CursorState)).Length);
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                switch(m_CursorState)
                {
                    case CursorState.Start:
                        SceneManager.LoadScene(1);
                        level = 1;
                        break;

                    case CursorState.Exit:
                        Application.Quit();
                        break;
                }
            }

            switch(m_CursorState)
            {
                case CursorState.Start:
                m_TitleCursor.offsetMax = new Vector2(-721.0f, -463.0f);
                m_TitleCursor.offsetMin = new Vector2(509.0f, 207.0f);
                break;

                case CursorState.Exit:
                m_TitleCursor.offsetMax = new Vector2(-721.0f, -532.0f);
                m_TitleCursor.offsetMin = new Vector2(509.0f, 138.0f);
                break;
            }
        }
        if (level > 0 && youDied != null && youDied.activeInHierarchy)
        {
            if (m_TitleCursor == null)
            {
                m_TitleCursor = GameObject.FindGameObjectWithTag("Cursor").GetComponent<RectTransform>();
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                m_CursorState = (CursorState)((int)(m_CursorState + 1) % System.Enum.GetNames(typeof(CursorState)).Length);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                m_CursorState = (CursorState)((int)(m_CursorState + System.Enum.GetNames(typeof(CursorState)).Length - 1) % System.Enum.GetNames(typeof(CursorState)).Length);
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                switch (m_CursorState)
                {
                    case CursorState.Start:
                        SceneManager.LoadScene(level);
                        break;

                    case CursorState.Exit:
                        SceneManager.LoadScene(0);
                        level = 0;
                        break;
                }
            }

            switch (m_CursorState)
            {
                case CursorState.Start:
                    m_TitleCursor.offsetMax = new Vector2(-761.0f, -463.0f);
                    m_TitleCursor.offsetMin = new Vector2(469.0f, 207.0f);
                    break;

                case CursorState.Exit:
                    m_TitleCursor.offsetMax = new Vector2(-761.0f, -532.0f);
                    m_TitleCursor.offsetMin = new Vector2(469.0f, 138.0f);
                    break;
            }
        }
        /*
        if (SceneManager.GetActiveScene().name.Equals("Level1")) // TutorialStage
        {
            if (player == null)
                player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            else if (tutorialTextForBird == null)
                tutorialTextForBird = GameObject.Find("ForBirdTexts");
            else if (tutorialTextForCow == null)
                tutorialTextForCow = GameObject.Find("ForCowTexts");
            else
            {
                if (player.state == PlayerController.State.HummingBird)
                {
                    tutorialTextForBird.SetActive(true);
                    tutorialTextForCow.SetActive(false);
                }
                else
                {
                    tutorialTextForBird.SetActive(false);
                    tutorialTextForCow.SetActive(true);
                }

            }
        }
        */
    }
    public void NextLevel()
    {
        if (level > 0 && level + 1 < SceneManager.sceneCountInBuildSettings)
        {
            level++;
            SceneManager.LoadScene(level);
        }
        else if (level + 1 >= SceneManager.sceneCountInBuildSettings)
        {
            level = 0;
            SceneManager.LoadScene(0);
        }
    }
}
