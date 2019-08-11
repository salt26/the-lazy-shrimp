using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    public int level;   // 스테이지 (레벨이 0인 경우 타이틀 화면)

    private RectTransform m_TitleCursor;
    private bool stageSelect;
    private int selectedLevel = 1;
    private enum CursorState {Start, Exit, Stage, Return};
    private CursorState m_CursorState = CursorState.Stage;
    private GameObject youDied;
    private Text m_LevelText, m_LevelTextShadow;
    public GameObject buttonStart, buttonExit;
    public GameObject selectMenu;
    public GameObject cursor;
    private Text m_PrevStageText, m_PrevStageTextShadow;
    private Text m_CurrStageText, m_CurrStageTextShadow;
    private Text m_NextStageText, m_NextStageTextShadow;
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
        selectMenu.SetActive(false);
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        selectMenu.SetActive(false);
    }

    void Update()
    {
        if(level > 0)
        {
            m_LevelText = GameObject.FindGameObjectWithTag("UILevelText").GetComponent<Text>();
            m_LevelTextShadow = GameObject.FindGameObjectWithTag("UILevelTextShadow").GetComponent<Text>();

            --level;
            m_LevelText.text = (level / 4 + 1) + "-" + (level % 4 + 1) + " Stage";
            m_LevelTextShadow.text = (level / 4 + 1) + "-" + (level % 4 + 1) + " Stage";
            ++level;
        }
        if (level > 0 && youDied == null && GameObject.Find("YouDied") != null) youDied = GameObject.Find("YouDied");
        if (level == 0)
        {
            if (buttonStart == null && GameObject.Find("ButtonSTART") != null) buttonStart = GameObject.Find("ButtonSTART");
            if (buttonExit == null && GameObject.Find("ButtonEXIT") != null) buttonExit = GameObject.Find("ButtonEXIT");
            if (selectMenu == null && GameObject.FindGameObjectWithTag("SelectMenu") != null) selectMenu = GameObject.FindGameObjectWithTag("SelectMenu");
            if (cursor == null && GameObject.FindGameObjectWithTag("Cursor") != null) cursor = GameObject.FindGameObjectWithTag("Cursor");
            if (m_TitleCursor == null)
            {
                m_TitleCursor = cursor.GetComponent<RectTransform>();
            }

            if (!stageSelect)
            {
                if (selectMenu.activeSelf) selectMenu.SetActive(false);
                if (!buttonStart.activeSelf) buttonStart.SetActive(true);
                if (!buttonExit.activeSelf) buttonExit.SetActive(true);
                if (cursor.activeSelf) cursor.SetActive(false);
                /*
                if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow))
                {
                    m_CursorState = m_CursorState == CursorState.Start ? CursorState.Exit : CursorState.Start;
                }
                */
                if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return))
                {
                    buttonStart.SetActive(false);
                    buttonExit.SetActive(false);
                    selectMenu.SetActive(true);
                    cursor.SetActive(true);
                    stageSelect = true;
                    m_CursorState = CursorState.Stage;
                }
                else if (Input.GetKeyDown(KeyCode.Escape)) {
                    Application.Quit();
                }
                /*
                else if (Input.GetKeyDown(KeyCode.Return))
                {
                    switch (m_CursorState)
                    {
                        case CursorState.Start:
                            buttonStart.SetActive(false);
                            buttonExit.SetActive(false);
                            selectMenu.SetActive(true);
                            stageSelect = true;
                            m_CursorState = CursorState.Stage;
                            break;

                        case CursorState.Exit:
                            Application.Quit();
                            break;

                        default:
                            break;
                    }
                }
                */
            }
            else
            {
                m_PrevStageText = GameObject.FindGameObjectWithTag("PrevStageText").GetComponent<Text>();
                m_PrevStageTextShadow = GameObject.FindGameObjectWithTag("PrevStageTextShadow").GetComponent<Text>();
                m_CurrStageText = GameObject.FindGameObjectWithTag("CurrStageText").GetComponent<Text>();
                m_CurrStageTextShadow = GameObject.FindGameObjectWithTag("CurrStageTextShadow").GetComponent<Text>();
                m_NextStageText = GameObject.FindGameObjectWithTag("NextStageText").GetComponent<Text>();
                m_NextStageTextShadow = GameObject.FindGameObjectWithTag("NextStageTextShadow").GetComponent<Text>();
                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    m_CursorState = (m_CursorState == CursorState.Stage ? CursorState.Return : CursorState.Stage);
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (m_CursorState == CursorState.Stage)
                    {
                        if (selectedLevel - 1 > 0)
                        {
                            // Decrease stage number by 4 (each stage has 4 substages)
                            selectedLevel -= 4;
                            if (selectedLevel == 1)
                            {
                                m_PrevStageText.text = m_PrevStageTextShadow.text = "";
                            }
                            else
                            {
                                m_PrevStageText.text = m_PrevStageTextShadow.text = ((selectedLevel + 3) / 4 - 1) + " Stage";
                            }
                            m_CurrStageText.text = m_CurrStageTextShadow.text = ((selectedLevel + 3) / 4) + " Stage";
                            m_NextStageText.text = m_NextStageTextShadow.text = ((selectedLevel + 3) / 4 + 1) + " Stage";
                        }

                    }
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (m_CursorState == CursorState.Stage)
                    {
                        // Increase stage number by 4 (each stage has 4 substages)
                        if (selectedLevel + 4 < SceneManager.sceneCountInBuildSettings)
                        {
                            selectedLevel += 4;
                            if (selectedLevel + 4 >= SceneManager.sceneCountInBuildSettings) // Currently stage 4 doesn't have 4 substages
                            {
                                m_NextStageText.text = m_NextStageTextShadow.text = "";
                            }
                            else
                            {
                                m_NextStageText.text = m_NextStageTextShadow.text = ((selectedLevel + 3) / 4 + 1) + " Stage";
                            }
                            m_CurrStageText.text = m_CurrStageTextShadow.text = ((selectedLevel + 3) / 4) + " Stage";
                            m_PrevStageText.text = m_PrevStageTextShadow.text = ((selectedLevel + 3) / 4 - 1) + " Stage";
                        }
                    }
                }
                else if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return))
                {
                    // buttonStart.SetActive(true);
                    // buttonExit.SetActive(true);
                    // selectMenu.SetActive(false);
                    switch (m_CursorState)
                    {
                        case CursorState.Stage:
                            SceneManager.LoadScene(selectedLevel);
                            level = selectedLevel;
                            break;

                        case CursorState.Return:
                            stageSelect = false;
                            m_CursorState = CursorState.Stage;
                            break;

                        default:
                            break;
                    }
                }
            }

            switch (m_CursorState)
            {
                case CursorState.Stage:
                    m_TitleCursor.offsetMax = new Vector2(-1000.0f, -532.0f);
                    m_TitleCursor.offsetMin = new Vector2(230.0f, 138.0f);
                    break;

                case CursorState.Return:
                    m_TitleCursor.offsetMax = new Vector2(-530.0f, -532.0f);
                    m_TitleCursor.offsetMin = new Vector2(700.0f, 138.0f);
                    break;

                default:
                    break;
            }
        }
        if (level > 0 && youDied != null && youDied.activeInHierarchy)
        {
            stageSelect = false;
            /*
            if (m_TitleCursor == null)
            {
                m_TitleCursor = GameObject.FindGameObjectWithTag("Cursor").GetComponent<RectTransform>();
                m_CursorState = CursorState.Start;
                stageSelect = false;
            }
            */
            /*
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                m_CursorState = (m_CursorState == CursorState.Start ? CursorState.Exit : CursorState.Start);
            }
            */
            if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return))
            {
                SceneManager.LoadScene(level);
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(0);
                m_CursorState = CursorState.Stage;
                level = 0;
                selectedLevel = 1;
            }
            /*
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                switch (m_CursorState)
                {
                    case CursorState.Start:
                        SceneManager.LoadScene(level);
                        break;

                    case CursorState.Exit:
                        SceneManager.LoadScene(0);
                        m_CursorState = CursorState.Start;
                        level = 0;
                        selectedLevel = 1;
                        break;
                    default:
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
            */
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
            selectedLevel = 1;
            SceneManager.LoadScene(0);
        }
    }
}
