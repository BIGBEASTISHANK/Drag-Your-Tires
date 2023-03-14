using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    // Variables
    /////////////
    public AudioMixer mixer;

    public static string SFXMixer = "SFX";
    public static string MusicMixer = "Music";
    public static string MasterMixer = "Master";
    public static string SoundSFXValueSave = "sound_sfx_value";
    public static string SoundMusicValueSave = "sound_music_value";
    public static string SoundMasterValueSave = "sound_master_value";

    public static string CarColorIndexStr = "CarColorIndex";
    public static string MapSelectorIndexStr = "MapSelectorIndex";

    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider masterSlider;

    [SerializeField] private GameObject mainMenu;

    [SerializeField] private AudioSource clickSound;
    
    [SerializeField] private TMP_Dropdown carColorDropdown;
    [SerializeField] private TMP_Dropdown mapSelectorDropdown;

    // References
    //////////////

    private void Start()
    {
        // Set sound slider value
        SoundSliderValue();

        // Setting values for car color and map
        carColorDropdown.value = PlayerPrefs.GetInt(CarColorIndexStr);
        mapSelectorDropdown.value = PlayerPrefs.GetInt(MapSelectorIndexStr);
    }

    // Changing volumes
    public void SFXSound(float SFXValue) { mixer.SetFloat(SFXMixer, SFXValue); PlayerPrefs.SetFloat(SoundSFXValueSave, SFXValue); }
    public void MusicSound(float MusicValue) { mixer.SetFloat(MusicMixer, MusicValue); PlayerPrefs.SetFloat(SoundMusicValueSave, MusicValue); }
    public void MasterSound(float MasterValue) { mixer.SetFloat(MasterMixer, MasterValue); PlayerPrefs.SetFloat(SoundMasterValueSave, MasterValue); }
    
    // Resetting volume
    public void ResetSound()
    {
        clickSound.Play();
        PlayerPrefs.DeleteKey(SoundSFXValueSave); PlayerPrefs.DeleteKey(SoundMusicValueSave); PlayerPrefs.DeleteKey(SoundMasterValueSave);
        sfxSlider.value = PlayerPrefs.GetFloat(SoundSFXValueSave); musicSlider.value = PlayerPrefs.GetFloat(SoundMusicValueSave); masterSlider.value = PlayerPrefs.GetFloat(SoundMasterValueSave);
    }

    // Setting slider value for volume
    private void SoundSliderValue()
    { sfxSlider.value = PlayerPrefs.GetFloat(SoundSFXValueSave); musicSlider.value = PlayerPrefs.GetFloat(SoundMusicValueSave); masterSlider.value = PlayerPrefs.GetFloat(SoundMasterValueSave); }

    // Getting car color index
    public void CarColor(int index) { clickSound.Play(); PlayerPrefs.SetInt(CarColorIndexStr, index); }

    // Getting map index
    public void MapSelector(int index) { clickSound.Play(); PlayerPrefs.SetInt(MapSelectorIndexStr, index); }

    // Back to main menu
    public void BackToMenu()
    { clickSound.Play(); Invoke(nameof(BackToMenuFunc), 0.3f); }
    private void BackToMenuFunc()
    {
        clickSound.Play();
        gameObject.SetActive(false);
        mainMenu.SetActive(true);
    }
}
