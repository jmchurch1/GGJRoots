using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Health Sliders")]
    [SerializeField] Slider playerSlider;
    [SerializeField] Slider plantSlider;

    [Header("Resources")]
    [SerializeField] TextMeshProUGUI worm;
    [SerializeField] TextMeshProUGUI ant;
    [SerializeField] TextMeshProUGUI mole;

    [Header("Wave Text")]
    [SerializeField] TextMeshProUGUI wave;

    public static UIManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void SetPlayerSlider(float healthPercentage)
    {
        playerSlider.value = healthPercentage;
    }

    public void SetPlantSlider(float healthPercentage)
    {
        plantSlider.value = healthPercentage;
    }

    public void SetWormAmount(int amount) { worm.text = amount.ToString(); }

    public void SetAntAmount(int amount) { ant.text = amount.ToString(); }

    public void SetMoleAmount(int amount) { mole.text = amount.ToString(); }

    public void SetWaveAmount(int wave) { this.wave.text = "Wave " + wave.ToString(); }
}
