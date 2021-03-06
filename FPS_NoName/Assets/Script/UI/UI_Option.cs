using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Dropdown;

public class UI_Option : MonoBehaviour
{

    UI_MainMenu MainMenuSystem;
    [SerializeField] private GameObject MainMenuObject; //メインメニュのスクリプト参照用のオブジェクト
    [SerializeField] private GameObject ApplyMessageObject;
    [SerializeField] private Image CurrentSelect;       //現在選んでる項目を表示する矢印
    [SerializeField] private int CurrentColumn;         //現在選んでる項目
    private int OptionColumns = 5;                      //オプションの項目数 (0: BGM設定 / 1:SE設定 / 2: マウス感度 / 3:パフォーマンス表示設定 / 4: 解像度設定 / 5: OKボタン)
    public bool isEnterMode;                            //オンの時は、選んでいる項目の設定を変更できる
    public bool isApplyShown;
    private RectTransform CurrentSelectPos;             //CurrentSelectのポジション用
    private Vector2 SelectPos;                          //RectTransformに反映させる用
    [SerializeField] private int performance_Enable;    //パフォーマンス表示のON/OFF (0:Disable / 1:Enable)
    public int Performance_Enable { get { return performance_Enable; } }//パフォーマンス表示のON/OFF Getter/Setter
    [SerializeField] private Slider BGMSlider;
    [SerializeField] private Slider SESlider;
    [SerializeField] private Slider Mouse_Sensi;
    [SerializeField] private Text BGMValue;
    [SerializeField] private Text SEValue;
    [SerializeField] private Text MouseSensiValue;

    [SerializeField] private Image StartNoSelect_Button;
    [SerializeField] private Image StartSelect_Button;

    [SerializeField] private Image MasterVolume_SelectImage;
    [SerializeField] private Image SEVolume_SelectImage;
    [SerializeField] private Image MouseSensi_SelectImage;
    [SerializeField] private Image FPS_SelectImage;
    [SerializeField] private Image Resolution_SelectImage;


    [SerializeField] private Dropdown ResolutionList;
    [SerializeField] private GameObject Performance_ON_Object;
    [SerializeField] private GameObject Performance_OFF_Object;

    [SerializeField] private AudioClip MainMenuChangeColumnSE;
    [SerializeField] private AudioClip MainMenuEnterSE;
    private void Awake()
    {
        AudioManager.Instance.Load();
        CurrentSelectPos = CurrentSelect.GetComponent<RectTransform>();
        MainMenuSystem = MainMenuObject.GetComponent<UI_MainMenu>();
    }
    private void Start()
    {
        CurrentColumn = 0;

        //解像度の自動取得
        ResolutionList.ClearOptions();
        var resolutions = Screen.resolutions;
        List<Resolution> checkList = new List<Resolution>();
        foreach (var res in resolutions)
        {
            bool check = true;
            foreach (var listItem in checkList)
            {
                if(res.width == listItem.width)
                {
                    if (res.height == listItem.height)
                    {
                        check = false;
                        break;
                    }
                }
            }
            if (check)
            {
                ResolutionList.options.Add(new OptionData(res.width.ToString() + "*" + res.height.ToString()));
            }
            checkList.Add(res);
        }
    }
    void Update()
    {
        InputDir();
        EnterChangeOptionMode();
        RefreshUIValue();
    }

    void InputDir()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && (!MainMenuSystem.CantSelectMenu && !isEnterMode))
        {
            AudioManager.Instance.SE.PlayOneShot(MainMenuChangeColumnSE);
            if (CurrentColumn == 0)
            {
                CurrentColumn = OptionColumns;
            }
            else
            {
                CurrentColumn--;
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && (!MainMenuSystem.CantSelectMenu && !isEnterMode))
        {
            AudioManager.Instance.SE.PlayOneShot(MainMenuChangeColumnSE);
            if (CurrentColumn == OptionColumns)
            {
                CurrentColumn = 0;
            }
            else
            {
                CurrentColumn++;
            }
        }
    }

    void EnterChangeOptionMode()
    {
        if (isApplyShown)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                AudioManager.Instance.SE.PlayOneShot(MainMenuEnterSE);
                ApplyMessageObject.SetActive(false);
                isApplyShown = false;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !MainMenuSystem.CantSelectMenu && !isApplyShown)
        {
            AudioManager.Instance.SE.PlayOneShot(MainMenuEnterSE);
            isEnterMode = !isEnterMode;
        }

        switch (CurrentColumn)
        {
            case 0:         //BGM設定
                CurrentSelect.enabled = true;
                StartSelect_Button.enabled = false;
                StartNoSelect_Button.enabled = true;
                SelectPos = new Vector2(-243, 238);
                if (isEnterMode)
                {
                    MasterVolume_SelectImage.enabled = true;
                    SEVolume_SelectImage.enabled = false;
                    MouseSensi_SelectImage.enabled = false;
                    FPS_SelectImage.enabled = false;
                    Resolution_SelectImage.enabled = false;
                    if (Input.GetKeyDown(KeyCode.LeftArrow) && BGMSlider.value > 0.0f)
                    {
                        AudioManager.Instance.SE.PlayOneShot(MainMenuChangeColumnSE);
                        BGMSlider.value -= 0.1f;            
                    }
                    else if (Input.GetKeyDown(KeyCode.RightArrow) && BGMSlider.value < 1.0f)
                    {
                        AudioManager.Instance.SE.PlayOneShot(MainMenuChangeColumnSE);
                        BGMSlider.value += 0.1f;
                    }
                }
                else
                {
                    MasterVolume_SelectImage.enabled = false;
                    SEVolume_SelectImage.enabled = false;
                    MouseSensi_SelectImage.enabled = false;
                    FPS_SelectImage.enabled = false;
                    Resolution_SelectImage.enabled = false;
                }
                break;
            case 1:         //SE設定
                CurrentSelect.enabled = true;
                StartSelect_Button.enabled = false;
                StartNoSelect_Button.enabled = true;
                SelectPos = new Vector2(-243, 158);
                if (isEnterMode)
                {
                    MasterVolume_SelectImage.enabled = false;
                    SEVolume_SelectImage.enabled = true;
                    MouseSensi_SelectImage.enabled = false;
                    FPS_SelectImage.enabled = false;
                    Resolution_SelectImage.enabled = false;
                    if (Input.GetKeyDown(KeyCode.LeftArrow) && SESlider.value > 0.0f)
                    {
                        AudioManager.Instance.SE.PlayOneShot(MainMenuChangeColumnSE);
                        SESlider.value -= 0.1f;
                    }
                    else if (Input.GetKeyDown(KeyCode.RightArrow) && SESlider.value < 1.0f)
                    {
                        AudioManager.Instance.SE.PlayOneShot(MainMenuChangeColumnSE);
                        SESlider.value += 0.1f;
                    }
                }
                else
                {
                    MasterVolume_SelectImage.enabled = false;
                    SEVolume_SelectImage.enabled = false;
                    MouseSensi_SelectImage.enabled = false;
                    FPS_SelectImage.enabled = false;
                    Resolution_SelectImage.enabled = false;
                }
                break;
            case 2:         //マウス感度
                CurrentSelect.enabled = true;
                StartSelect_Button.enabled = false;
                StartNoSelect_Button.enabled = true;
                SelectPos = new Vector2(-243, 28);
                /*if (isEnterMode)
                {
                    MasterVolume_SelectImage.enabled = false;
                    SEVolume_SelectImage.enabled = false;
                    MouseSensi_SelectImage.enabled = true;
                    FPS_SelectImage.enabled = false;
                    Resolution_SelectImage.enabled = false;
                }
                else
                {
                    MasterVolume_SelectImage.enabled = false;
                    SEVolume_SelectImage.enabled = false;
                    MouseSensi_SelectImage.enabled = false;
                    FPS_SelectImage.enabled = false;
                    Resolution_SelectImage.enabled = false;
                }*/
                break;
            case 3:         //パフォーマンス表示設定
                CurrentSelect.enabled = true;
                StartSelect_Button.enabled = false;
                StartNoSelect_Button.enabled = true;
                SelectPos = new Vector2(-243, -52);
                /*if (isEnterMode)
                {
                    MasterVolume_SelectImage.enabled = false;
                    SEVolume_SelectImage.enabled = false;
                    MouseSensi_SelectImage.enabled = false;
                    FPS_SelectImage.enabled = true;
                    Resolution_SelectImage.enabled = false;
                    if (Input.GetKeyDown(KeyCode.LeftArrow) && (!Performance_OFF_Object.activeSelf) && Performance_ON_Object.activeSelf)
                    {
                        AudioManager.Instance.SE.PlayOneShot(MainMenuChangeColumnSE);
                        performance_Enable = 0;
                        Performance_ON_Object.SetActive(false);
                        Performance_OFF_Object.SetActive(true);
                    }
                    else if (Input.GetKeyDown(KeyCode.RightArrow) && Performance_OFF_Object.activeSelf && (!Performance_ON_Object.activeSelf))
                    {
                        AudioManager.Instance.SE.PlayOneShot(MainMenuChangeColumnSE);
                        performance_Enable = 1;
                        Performance_OFF_Object.SetActive(false);
                        Performance_ON_Object.SetActive(true);
                    }
                }
                else
                {
                    MasterVolume_SelectImage.enabled = false;
                    SEVolume_SelectImage.enabled = false;
                    MouseSensi_SelectImage.enabled = false;
                    FPS_SelectImage.enabled = false;
                    Resolution_SelectImage.enabled = false;
                }*/
                break;
            case 4:         //解像度設定
                CurrentSelect.enabled = true;
                StartSelect_Button.enabled = false;
                StartNoSelect_Button.enabled = true;
                SelectPos = new Vector2(-243, -138);
                /*if (isEnterMode)
                {
                    MasterVolume_SelectImage.enabled = false;
                    SEVolume_SelectImage.enabled = false;
                    MouseSensi_SelectImage.enabled = false;
                    FPS_SelectImage.enabled = false;
                    Resolution_SelectImage.enabled = true;
                    ResolutionList.Show();
                }
                else
                {
                    MasterVolume_SelectImage.enabled = false;
                    SEVolume_SelectImage.enabled = false;
                    MouseSensi_SelectImage.enabled = false;
                    FPS_SelectImage.enabled = false;
                    Resolution_SelectImage.enabled = false;
                }*/
                break;
            case 5:         //OKボタン
                CurrentSelect.enabled = false;
                StartNoSelect_Button.enabled = false;
                StartSelect_Button.enabled = true;
                if (isEnterMode)
                {
                    ApplyMessageObject.SetActive(true);
                    isApplyShown = true;
                    AudioManager.Instance.Save();
                    isEnterMode = false;
                }
                break;
        }
        CurrentSelectPos.anchoredPosition = SelectPos;//現在の選ばれている項目の場所へ矢印を置く
    }

    void RefreshUIValue()
    {
        float FinalBGMValue = 0.0f;
        float FinalSEValue = 0.0f;
        FinalBGMValue = BGMSlider.value * 100;
        FinalSEValue = SESlider.value * 100;
        BGMValue.text = FinalBGMValue.ToString("0") + "%";
        SEValue.text = FinalSEValue.ToString("0") + "%";
    }
}
