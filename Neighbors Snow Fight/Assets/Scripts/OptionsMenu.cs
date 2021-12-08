using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    [Header("Crossair")]
    [SerializeField] private Slider RedSlider;
    [SerializeField] private Slider BlueSlider;
    [SerializeField] private Slider GreenSlider;
    [SerializeField] private TextMeshProUGUI crossairPreview;
    [SerializeField] private GameObject applyButton;

    [Header("Volume")]
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider masterVolumeSlider;

    public void Initialize()
    {
        SetVolume(PlayerPrefs.GetFloat("masterVolume", 1));
    }

    private void OnEnable()
    {
        RedSlider.value = PlayerPrefs.GetFloat("crossairColorR", 0);
        GreenSlider.value = PlayerPrefs.GetFloat("crossairColorG", 0);
        BlueSlider.value = PlayerPrefs.GetFloat("crossairColorB", 0);
        PreviewCrossair();
        applyButton.SetActive(false);

        var masterVolume = PlayerPrefs.GetFloat("masterVolume", 1);
        masterVolumeSlider.value = masterVolume;
        SetVolume(masterVolume);
    }

    public void PreviewCrossair()
    {
        crossairPreview.color = new Color(RedSlider.value, GreenSlider.value, BlueSlider.value);
    }

    public void ApplyCrossair()
    {
        applyButton.SetActive(false);
        PlayerPrefs.SetFloat("crossairColorR", RedSlider.value);
        PlayerPrefs.SetFloat("crossairColorG", GreenSlider.value);
        PlayerPrefs.SetFloat("crossairColorB", BlueSlider.value);
    }

    public void SetVolume(float volume)
    {
        Debug.Log("SET VOLUME");
        mixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("masterVolume", volume);
    }
}
