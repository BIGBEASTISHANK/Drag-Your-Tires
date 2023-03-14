using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Variables
    /////////////
    public CarLogic carLogicScript;

    private OptionsMenu optionsScript;

    [SerializeField] private GameObject playerUI;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject loadingScreen;

    [SerializeField] private AudioSource clickSound;

    [SerializeField] private Slider loadingSlider;

    // References
    //////////////
    private void Start() { optionsScript = optionsMenu.GetComponent<OptionsMenu>(); SetSound(); optionsMenu.SetActive(false); }
    // SetSound function
    private void SetSound()
    {
        // Setting sound volume
        optionsScript.mixer.SetFloat(OptionsMenu.SFXMixer, PlayerPrefs.GetFloat(OptionsMenu.SoundSFXValueSave));
        optionsScript.mixer.SetFloat(OptionsMenu.MusicMixer, PlayerPrefs.GetFloat(OptionsMenu.SoundMusicValueSave));
        optionsScript.mixer.SetFloat(OptionsMenu.MasterMixer, PlayerPrefs.GetFloat(OptionsMenu.SoundMasterValueSave));

        playerUI.SetActive(false);
        optionsMenu.SetActive(false);
        loadingScreen.SetActive(false);
    }

    // Resume the game
    public void ResumeGame()
    { clickSound.Play(); Invoke(nameof(ResumeGameFunc), 0.3f); }
    private void ResumeGameFunc()
    { playerUI.SetActive(true); gameObject.SetActive(false); optionsMenu.SetActive(false); }

    // Opening Options menu
    public void OpenOptions()
    { clickSound.Play(); Invoke(nameof(OpenOptionsFunc), 0.3f); }
    private void OpenOptionsFunc() { gameObject.SetActive(false); optionsMenu.SetActive(true); }

    // Quitting game
    public void BackToMenu()
    { StartCoroutine(BackToMenuLoading()); }

    IEnumerator BackToMenuLoading()
    {
        clickSound.Play();
        yield return new WaitForSeconds(0.3f);
        AsyncOperation scenLoader = SceneManager.LoadSceneAsync(0);
        loadingScreen.SetActive(true);

        while (!scenLoader.isDone)
        {
            float progressValue = Mathf.Clamp01(scenLoader.progress / 0.9f);
            loadingSlider.value = progressValue;
            yield return null;
        }
    }
}
