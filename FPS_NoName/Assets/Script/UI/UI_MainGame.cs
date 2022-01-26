using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MainGameManage {
    public class UI_MainGame : MonoBehaviour
    {
        [SerializeField] private GameObject PlayerIcon1;                //体力が50%以上の時のプレイヤーアイコン
        [SerializeField] private GameObject PlayerIcon2;                //体力が25%以上の時のプレイヤーアイコン
        [SerializeField] private GameObject PlayerIcon3;                //体力が24%以下の時のプレイヤーアイコン
        [SerializeField] private Image Health_Bar;                      //体力が50%以上の時の体力バー
        [SerializeField] private Image Health_Bar_Low1;                 //体力が25%以上の時の体力バー
        [SerializeField] private Image Health_Bar_Low2;                 //体力が24%以下の時の体力バー
        [SerializeField] private Image Armor_Bar;                       //アーマーバー
        [SerializeField] private Text Health_Text;                      //体力値を表示するText
        [SerializeField] private Text Armor_Text;                       //アーマー値を表示するText
        [SerializeField] private Text Weapon_CurrentAmmoText;           //武器の現在のアモを表示するText
        [SerializeField] private Text Weapon_CurrentMagazineText;       //武器の現在のマガジンを表示するText
        [SerializeField] private bool IsPushEsc;
        Character_Info CharacterInfo;
        MainGameManager MainGame_Manager;
        Weapon_State Weapon_Stats;

        // ----------------------------------------------------------------------------------------------------
        //
        //  ゲームオーバー用
        //
        //
        [SerializeField] private GameObject GameOverUI;                 
        [SerializeField] private Image BG;
        [SerializeField] private Image GameOverBG;
        [SerializeField] private Image GameOverLOGO;
        [SerializeField] private Image GameOver_RetrySelect;
        [SerializeField] private Image GameOver_Retry;
        [SerializeField] private Image GameOver_ExitSelect;
        [SerializeField] private Image GameOver_Exit;                   
        [SerializeField] private float FadeSpeed;                       //ゲームオーバー時の表示速度
        [SerializeField] private float MoveSpeed;                       //ゲームオーバーロゴが動く速度
        [SerializeField] private RectTransform GameOverLogo_POS;        //ゲームオーバーロゴのレクトトランスフォーム
        Vector2 GameOverLogo_Vec;                                       //ゲームオーバーロゴのベクトル
        [SerializeField] private bool GameOver_FinishAnim;              //ゲームオーバーUIのアニメーションが終わった時の判定bool
        [SerializeField] private int CurrentSelect;                     //ゲームオーバー時の選んでいる項目 (0:再挑戦 1:諦める)
        //
        //
        // ----------------------------------------------------------------------------------------------------
        void Awake()
        {
            MainGame_Manager = GameObject.Find("MainGameManager").GetComponent<MainGameManager>();

            Health_Bar_Low1.enabled = false;
            Health_Bar_Low2.enabled = false;

            GameOverUI.SetActive(false);

            GameOverLogo_Vec = new Vector2(0,-125);
            GameOverLogo_POS.anchoredPosition = GameOverLogo_Vec;

            BG.enabled = false;
            GameOverBG.enabled = false;
            GameOverLOGO.enabled = false;
            GameOver_RetrySelect.enabled = false;
            GameOver_Retry.enabled = false;
            GameOver_ExitSelect.enabled = false;
            GameOver_Exit.enabled = false;

            BG.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            GameOverBG.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            GameOverLOGO.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            GameOver_RetrySelect.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            GameOver_Retry.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            GameOver_ExitSelect.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            GameOver_Exit.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

            CharacterInfo = GameObject.Find("Player").GetComponent<Character_Info>();
            Weapon_Stats = GameObject.Find("MainWeapon").GetComponent<Weapon_State>();
        }

        void Update()
        {
            HealthValueChangeIcon();
            RefreshUIText();
            InputDir();
            GameOver_System();
        }

        void RefreshUIText()
        {
            Health_Bar.fillAmount = CharacterInfo.Character_CurrentHP / CharacterInfo.Character_MaxHP;
            Health_Bar_Low1.fillAmount = CharacterInfo.Character_CurrentHP / CharacterInfo.Character_MaxHP;
            Health_Bar_Low2.fillAmount = CharacterInfo.Character_CurrentHP / CharacterInfo.Character_MaxHP;
            Armor_Bar.fillAmount = CharacterInfo.Character_CurrentArmor / CharacterInfo.Character_MaxArmor;
            Weapon_CurrentAmmoText.text = Weapon_Stats.Weapon_CurrentAmmo.ToString("00");
            Weapon_CurrentMagazineText.text = Weapon_Stats.Weapon_CurrentMagazine.ToString("000");
            Health_Text.text = CharacterInfo.Character_CurrentHP.ToString();
            Armor_Text.text = CharacterInfo.Character_CurrentArmor.ToString();
        }

        void HealthValueChangeIcon()
        {
            if (Health_Bar.fillAmount >= 0.5f)
            {
                Health_Bar.enabled = true;
                Health_Bar_Low1.enabled = false;
                Health_Bar_Low2.enabled = false;
                PlayerIcon1.SetActive(true);
                PlayerIcon2.SetActive(false);
                PlayerIcon3.SetActive(false);
            }
            else if (Health_Bar.fillAmount >= 0.25f)
            {
                Health_Bar.enabled = false;
                Health_Bar_Low1.enabled = true;
                Health_Bar_Low2.enabled = false;
                PlayerIcon1.SetActive(false);
                PlayerIcon2.SetActive(true);
                PlayerIcon3.SetActive(false);
            }
            else if (Health_Bar.fillAmount >= 0.0f)
            {
                Health_Bar.enabled = false;
                Health_Bar_Low1.enabled = false;
                Health_Bar_Low2.enabled = true;
                PlayerIcon1.SetActive(false);
                PlayerIcon2.SetActive(false);
                PlayerIcon3.SetActive(true);
            }
        }

        void GameOver_System()
        {
            if (CharacterInfo.Character_CurrentHP <= 0)
            {
                GameOverUI.SetActive(true);
                BG.enabled = true;
                GameOverBG.enabled = true;
                GameOverLOGO.enabled = true;
                if (BG.color.a <= 0.8f)
                {
                    BG.color = new Color(0.0f, 0.0f, 0.0f, BG.color.a + FadeSpeed * Time.deltaTime);
                }
                GameOverBG.color = new Color(1.0f, 1.0f, 1.0f, GameOverBG.color.a + FadeSpeed * Time.deltaTime);
                GameOverLOGO.color = new Color(1.0f, 1.0f, 1.0f, GameOverLOGO.color.a + FadeSpeed * Time.deltaTime);

                if (GameOverLOGO.color.a >= 1.0f)
                {
                    GameOver_RetrySelect.enabled = true;
                    GameOver_Exit.enabled = true;

                    if (GameOverLogo_POS.anchoredPosition.y >= 0.0f)
                    {
                        GameOverLogo_POS.anchoredPosition = new Vector2(0.0f, 0.0f);
                        GameOver_FinishAnim = true;
                    }
                    else
                    {
                        GameOverLogo_Vec = new Vector2(0, GameOverLogo_Vec.y + MoveSpeed * Time.deltaTime);
                        GameOver_RetrySelect.color = new Color(1.0f, 1.0f, 1.0f, GameOver_RetrySelect.color.a + 1.0f / (125.0f / MoveSpeed) * Time.deltaTime);
                        GameOver_Exit.color = new Color(1.0f, 1.0f, 1.0f, GameOver_Exit.color.a + 1.0f / (125.0f / MoveSpeed) * Time.deltaTime);
                        GameOverLogo_POS.anchoredPosition = GameOverLogo_Vec;
                    }
                }
            }

            if (GameOver_FinishAnim)            //ゲームオーバーのアニメーションが終わったら
            {
                switch (CurrentSelect)
                {
                    case 0:
                        GameOver_RetrySelect.enabled = true;
                        GameOver_Exit.enabled = true;
                        GameOver_ExitSelect.enabled = false;
                        GameOver_Retry.enabled = false;
                        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
                        {
                            CurrentSelect++;
                        }
                        break;
                    case 1:
                        GameOver_ExitSelect.enabled = true;
                        GameOver_Retry.enabled = true;
                        GameOver_RetrySelect.enabled = false;
                        GameOver_Exit.enabled = false;
                        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
                        {
                            CurrentSelect--;
                        }
                        break;
                }

            }
        }

        void InputDir()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                MainGame_Manager.MainGame_IsPause = !MainGame_Manager.MainGame_IsPause;
            }
        }
    }
}
