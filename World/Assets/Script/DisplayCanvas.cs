using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayCanvas : MonoBehaviour
{
    [SerializeField]
    private Player Player;
    
    [SerializeField]
    private Coin Coin;

    private TMPro.TextMeshProUGUI coinDistanceText;
    private Image arrowImage;
    private Image staminaIndicator;
    private Image staminaBack;
    private Image coinTimer;
    private Image coinTimerBg;
    private TMPro.TextMeshProUGUI coinTimerTxt;
    private TMPro.TextMeshProUGUI LeftHint;
    private TMPro.TextMeshProUGUI RightHint;
    private Renderer CoinRenderer;

    // Start is called before the first frame update
    void Start()
    {
        coinDistanceText = GameObject.Find("CoinDistanceText").GetComponent<TMPro.TextMeshProUGUI>();
        arrowImage = GameObject.Find("Arrow").GetComponent<Image>();
        staminaIndicator = GameObject.Find("StaminaIndicatorFront").GetComponent<Image>();
        staminaBack = GameObject.Find("StaminaIndicatorBg").GetComponent<Image>();
        coinTimer = GameObject.Find("CoinTimerFrontTimer").GetComponent<Image>();
        coinTimerBg = GameObject.Find("CoinTimerBg").GetComponent<Image>();
        coinTimerTxt = GameObject.Find("CoinTimerCoinTimerText").GetComponent <TMPro.TextMeshProUGUI>();
        LeftHint = GameObject.Find("LeftHint").GetComponent<TMPro.TextMeshProUGUI>();
        RightHint = GameObject.Find("RightHint").GetComponent<TMPro.TextMeshProUGUI>();

        CoinRenderer = Coin.GetComponentInChildren<Renderer>();
        
    }

    // Update is called once per frame
    void Update()
    {

        float coinDistance = (Coin.transform.position - Player.transform.position).magnitude;
        coinDistanceText.SetText(coinDistance.ToString("0.0"));

        coinDistanceText.color = new Color(1 / (1 + coinDistance / 10), 0.2f, 1 - 1 / (1 + coinDistance / 10));


        Vector3 c = Coin.transform.position - Player.transform.position;
        c.y = 0;
        float angle = Vector3.SignedAngle(c, Player.transform.forward, Vector3.up);
        arrowImage.transform.eulerAngles = new Vector3(0, 0, angle);

        if (CoinRenderer.isVisible)
        {
            LeftHint.enabled = false;
            RightHint.enabled = false;
        }
        else
        {
            if (angle < 0)
            {
                LeftHint.enabled = false;
                RightHint.enabled = true;

            }
            else
            {
                RightHint.enabled = false;
                LeftHint.enabled = true;
            }
        }

        coinTimer.fillAmount = Coin.restTime;
        staminaIndicator.fillAmount = Player.Stamina;
    }

    private void LateUpdate()
    {
        coinDistanceText.enabled = GameSettings.CoinDistanceEnabled;
        staminaIndicator.enabled= GameSettings.StaminaEnabled;
        staminaBack.enabled= GameSettings.StaminaEnabled;
        arrowImage.enabled = GameSettings.DirectionHintsEnabled;
        if (!GameSettings.DirectionHintsEnabled)
        {
            LeftHint.enabled = RightHint.enabled = false;
        }
        coinTimer.enabled = GameSettings.CoinTimeoutEnabled;
        coinTimerBg.enabled = GameSettings.CoinTimeoutEnabled;
        coinTimerTxt.enabled = GameSettings.CoinTimeoutEnabled;
    }
}
