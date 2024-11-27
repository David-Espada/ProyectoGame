using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Farm : MonoBehaviour
{
    [Header("Stats")]
    public float life;
    public float maxLife;
    [Header("References")]
    public Player player;
    public List<Sheep> sheepsInFarm = new List<Sheep>();
    public Slider lifeBar;
    public GameObject gameOverPanel;
    public GameObject ui;
    void Start()
    {
        life = maxLife;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLifeBar();
        if (sheepsInFarm.Count > 0)
        {
            if (sheepsInFarm[0].isDay)
            {
                ActivateSheeps();
            }
        }
    }
    public void ActivateSheeps()
    {
        sheepsInFarm.ForEach(s =>  s.gameObject.SetActive(true) );
        sheepsInFarm.Clear();
    }
    public void TakeDamage(float amount)
    {
        life -= amount;
        if (life <= 0)
        {
            life = 0;
            ui.SetActive(false);
            gameOverPanel.SetActive(true);
            Time.timeScale = 0;
            Debug.Log("Fallaste");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("WorkerSheep"))
        {
            Sheep detectedSheep = other.gameObject.GetComponent<Sheep>();
            if (!detectedSheep.isDay)
            {
                player.AddWhool(detectedSheep.LeaveWhool());
                detectedSheep.gameObject.SetActive(false);
                sheepsInFarm.Add(detectedSheep);
            }
        }
    }
    public void UpdateLifeBar()
    {
        lifeBar.value = life/maxLife;
    }

}
