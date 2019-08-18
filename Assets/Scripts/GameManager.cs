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
    private enum CursorState {Stage};
    private CursorState m_CursorState = CursorState.Stage;
    private GameObject youDied;
    private Text m_LevelText, m_LevelTextShadow;
    public GameObject buttonStart, buttonExit;
    public GameObject selectMenu;
    public GameObject cursor;
    public GameObject prevStageKey, nextStageKey;
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
        prevStageKey.SetActive(false);
        nextStageKey.SetActive(false);
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
            if(level + 1 < SceneManager.sceneCountInBuildSettings) { 
                m_LevelText.text = (level / 4 + 1) + "-" + (level % 4 + 1) + " Stage";
                m_LevelTextShadow.text = (level / 4 + 1) + "-" + (level % 4 + 1) + " Stage";
            }
            else
            {
                m_LevelText.text = m_LevelTextShadow.text = "Credit";
            }
            ++level;
        }
        if (level > 0 && youDied == null && GameObject.Find("YouDied") != null) youDied = GameObject.Find("YouDied");
        if (level == 0)
        {
            if (buttonStart == null && GameObject.Find("ButtonSTART") != null) buttonStart = GameObject.Find("ButtonSTART");
            if (buttonExit == null && GameObject.Find("ButtonEXIT") != null) buttonExit = GameObject.Find("ButtonEXIT");
            if (selectMenu == null && GameObject.FindGameObjectWithTag("SelectMenu") != null) selectMenu = GameObject.FindGameObjectWithTag("SelectMenu");
            if (cursor == null && GameObject.FindGameObjectWithTag("Cursor") != null) cursor = GameObject.FindGameObjectWithTag("Cursor");
            if (prevStageKey == null && GameObject.FindGameObjectWithTag("PrevStageKey") != null) prevStageKey = GameObject.FindGameObjectWithTag("PrevStageKey");
            if (nextStageKey == null && GameObject.FindGameObjectWithTag("NextStageKey") != null) nextStageKey = GameObject.FindGameObjectWithTag("NextStageKey");
            if (!stageSelect)
            {
                prevStageKey.SetActive(false);
                nextStageKey.SetActive(false);
            }

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
                if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return))
                {
                    buttonStart.SetActive(false);
                    buttonExit.SetActive(false);
                    selectMenu.SetActive(true);
                    cursor.SetActive(true);
                    stageSelect = true;
                    prevStageKey.SetActive(false);
                    nextStageKey.SetActive(true);
                    m_CursorState = CursorState.Stage;
                }
                else if (Input.GetKeyDown(KeyCode.Escape)) {
                    Application.Quit();
                }
            }
            else
            {
                m_PrevStageText = GameObject.FindGameObjectWithTag("PrevStageText").GetComponent<Text>();
                m_PrevStageTextShadow = GameObject.FindGameObjectWithTag("PrevStageTextShadow").GetComponent<Text>();
                m_CurrStageText = GameObject.FindGameObjectWithTag("CurrStageText").GetComponent<Text>();
                m_CurrStageTextShadow = GameObject.FindGameObjectWithTag("CurrStageTextShadow").GetComponent<Text>();
                m_NextStageText = GameObject.FindGameObjectWithTag("NextStageText").GetComponent<Text>();
                m_NextStageTextShadow = GameObject.FindGameObjectWithTag("NextStageTextShadow").GetComponent<Text>();
                if (prevStageKey == null && GameObject.FindGameObjectWithTag("PrevStageKey") != null) prevStageKey = GameObject.FindGameObjectWithTag("PrevStageKey");
                if (nextStageKey == null && GameObject.FindGameObjectWithTag("NextStageKey") != null) nextStageKey = GameObject.FindGameObjectWithTag("NextStageKey");
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (m_CursorState == CursorState.Stage)
                    {
                        if (selectedLevel - 1 > 0)
                        {
                            // Decrease stage number by 4 (each stage has 4 substages)
                            selectedLevel -= 4;
                            if (selectedLevel == 1)
                            {
                                // prevStageKey.GetComponent<Image>().color.a = 0;
                                prevStageKey.SetActive(false);
                                nextStageKey.SetActive(true);
                                m_PrevStageText.text = m_PrevStageTextShadow.text = "";
                            }
                            else
                            {
                                prevStageKey.SetActive(true);
                                nextStageKey.SetActive(true);
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
                        if (selectedLevel + 4 < SceneManager.sceneCountInBuildSettings - 1)
                        {
                            selectedLevel += 4;
                            if (selectedLevel + 4 >= SceneManager.sceneCountInBuildSettings - 1) // Currently stage 4 doesn't have 4 substages
                            {
                                prevStageKey.SetActive(true);
                                nextStageKey.SetActive(false);
                                m_NextStageText.text = m_NextStageTextShadow.text = "";
                            }
                            else
                            {
                                prevStageKey.SetActive(true);
                                nextStageKey.SetActive(true);
                                m_NextStageText.text = m_NextStageTextShadow.text = ((selectedLevel + 3) / 4 + 1) + " Stage";
                            }
                            m_CurrStageText.text = m_CurrStageTextShadow.text = ((selectedLevel + 3) / 4) + " Stage";
                            m_PrevStageText.text = m_PrevStageTextShadow.text = ((selectedLevel + 3) / 4 - 1) + " Stage";
                        }
                    }
                }
                else if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return))
                {
                    SceneManager.LoadScene(selectedLevel);
                    level = selectedLevel;
                }
                else if (Input.GetKeyDown(KeyCode.Escape))
                {
                    stageSelect = false;
                    selectedLevel = 1;
                    m_CursorState = CursorState.Stage;
                    m_PrevStageText.text = m_PrevStageTextShadow.text = "";
                    m_CurrStageText.text = m_CurrStageTextShadow.text = "1 Stage";
                    m_NextStageText.text = m_NextStageTextShadow.text = "2 Stage";
                }
            }

            switch (m_CursorState)
            {
                case CursorState.Stage:
                    m_TitleCursor.offsetMax = new Vector2(-1000.0f, -532.0f);
                    m_TitleCursor.offsetMin = new Vector2(230.0f, 138.0f);
                    break;

                default:
                    break;
            }
        }
        if (level > 0 && youDied != null && youDied.activeInHierarchy)
        {
            stageSelect = false;
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
            stageSelect = false;
            SceneManager.LoadScene(0);
        }
    }
}
