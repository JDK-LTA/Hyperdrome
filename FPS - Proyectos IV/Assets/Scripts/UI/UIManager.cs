using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] Image dashCdImage;
    [SerializeField] Text roundText, piecesText, ntcText, changerText, selectedWeaponText;

    private void Update()
    {
        if (!WeaponManager.Instance._player.CanDash)
        {
            dashCdImage.fillAmount = WeaponManager.Instance._player.DashT / WeaponManager.Instance._player.DashCooldown;
        }
    }

    public void SetDashImageActive(bool active)
    {
        dashCdImage.gameObject.SetActive(active);
        dashCdImage.fillAmount = 0;
    }
    public void UpdateRoundText(int round, int maxRounds)
    {
        roundText.text = "Round" + "\n" + round + "/" + maxRounds;
    }
    public void UpdatePiecesText(int piecesLeft)
    {
        piecesText.text = "Pieces to" + "\n" + "deliver: " + piecesLeft;
    }
    public void UpdateNoToChangeText(float ntc, float maxNtc, Changer changer)
    {
        switch (changer)
        {
            case Changer.TIME:
                ntcText.text = Mathf.Round(ntc * 10) / 10 + "\n" +"secs";
                break;
            case Changer.AMMO:
            case Changer.HIT:
                ntcText.text = Mathf.Round(ntc * 10) / 10 + "/" + "\n" + maxNtc;
                break;
            default:
                break;
        }
    }
    public void UpdateChangerText(Changer changer)
    {
        switch (changer)
        {
            case Changer.AMMO:
                changerText.text = "Ammo:";
                break;
            case Changer.TIME:
                changerText.text = "Time:";
                break;
            case Changer.HIT:
                changerText.text = "Hits:";
                break;
        }
    }
    public void UpdateSelectedWeaponText(int sw, int max)
    {
        selectedWeaponText.text = "Weapon " + sw + "/" + max;
    }
}
