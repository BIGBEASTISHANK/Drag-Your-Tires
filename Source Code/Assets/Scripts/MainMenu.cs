using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Variables
    /////////////
    private OptionsMenu optionsScript;

    [SerializeField] private string[] sceneName;

    [SerializeField] private Slider loadingSlider;

    [SerializeField] private AudioSource clickSound;

    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject loadingScreen;

    // References
    //////////////
    private void Start() { optionsScript = optionsMenu.GetComponent<OptionsMenu>(); SetSound(); optionsMenu.SetActive(false); loadingScreen.SetActive(false); }

    // SetSound function
    private void SetSound()
    {
        // Setting sound volume
        optionsScript.mixer.SetFloat(OptionsMenu.SFXMixer, PlayerPrefs.GetFloat(OptionsMenu.SoundSFXValueSave));
        optionsScript.mixer.SetFloat(OptionsMenu.MusicMixer, PlayerPrefs.GetFloat(OptionsMenu.SoundMusicValueSave));
        optionsScript.mixer.SetFloat(OptionsMenu.MasterMixer, PlayerPrefs.GetFloat(OptionsMenu.SoundMasterValueSave));
    }

    // Start the game
    public void StartGame()
    { clickSound.Play(); StartCoroutine(StartGameFunc()); }
    IEnumerator StartGameFunc()
    {
        yield return new WaitForSeconds(0.3f);
        AsyncOperation scenLoader = SceneManager.LoadSceneAsync((string)sceneName.GetValue(PlayerPrefs.GetInt(OptionsMenu.MapSelectorIndexStr)));
        loadingScreen.SetActive(true);

        while (!scenLoader.isDone)
        {
            float progressValue = Mathf.Clamp01(scenLoader.progress / 0.9f);
            loadingSlider.value = progressValue;
            yield return null;
        }
    }

    // Open Options menu
    public void OpenOptions()
    { clickSound.Play(); Invoke(nameof(OpenOptionFunc), 0.3f); }
    private void OpenOptionFunc()
    { gameObject.SetActive(false); optionsMenu.SetActive(true); }

    // Quit Game
    public void QuitGame() { clickSound.Play(); Application.Quit(); }
}
