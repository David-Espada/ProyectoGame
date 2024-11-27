using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Stats")]
    public float money;
    public float maxMoney;
    public float whool;
    public float maxWhool;
    public float sellSpeed;
    public float sellMultiplier;
    [Header("References")]
    public Slider moneyBar;
    public Slider whoolBar;
    public TextMeshProUGUI moneyIndicator;
    public TextMeshProUGUI whoolIndicator;
    void Start()
    {
        money = maxMoney;
    }

    // Update is called once per frame
    void Update()
    {
        if (whool > 0)
        {
            SellWhool();
        }
        UpdateBar(moneyBar, money, maxMoney);
        UpdateBar(whoolBar, whool, maxWhool);
        UpdateIndicator(moneyIndicator, money, maxMoney);
        UpdateIndicator(whoolIndicator, whool, maxWhool);
        
    }
    public void UpdateBar(Slider bar , float amount , float maxAmount)
    {
        bar.value = amount/maxAmount;
    }
    public void UpdateIndicator(TextMeshProUGUI indicator , float amount, float maxAmount)
    {
        int integerAmount = (int) amount;
        indicator.text = integerAmount.ToString()+"/"+maxAmount;
    }
    public void SellWhool()
    {
        float amount = Time.deltaTime*sellMultiplier;
        whool -= amount;
        money += amount;
        if (money > maxMoney)
        {
            money = maxMoney ;
        }
    }
    public void SpendMoney(float amount)
    {
        money -= amount;
    }
    public void AddWhool(float amount) 
    {
        whool += amount;
        if (whool > maxWhool)
        {
            whool = maxWhool;
        }
    }
}
