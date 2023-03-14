using UnityEngine;

public class CarLogic : MonoBehaviour
{
    // Variables
    /////////////
    private bool isPaused;

    private InputSys inpSys;

    [SerializeField] private GameObject playerUI;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject loadingMenu;

    [SerializeField] private Renderer carRenderer;

    [SerializeField] private Material[] carMaterial;

    // References
    //////////////
    private void Awake() { inpSys = new InputSys(); playerUI.SetActive(true); pauseMenu.SetActive(false); optionsMenu.SetActive(false); loadingMenu.SetActive(false); }

    private void FixedUpdate()
    {
        // Set Car Color
        carRenderer.material = (Material)carMaterial.GetValue(PlayerPrefs.GetInt(OptionsMenu.CarColorIndexStr));

        // Esc pressed
        inpSys.Player.Pause.performed += ctx => PauseGame();
    }

    // Pause Game function
    private void PauseGame()
    {
        if (!isPaused)
        {
            // Setting ui
            playerUI.SetActive(false);
            optionsMenu.SetActive(false);
            pauseMenu.SetActive(true);
            isPaused = true;
        }
        else
        {
            // Setting ui
            playerUI.SetActive(true);
            optionsMenu.SetActive(false);
            pauseMenu.SetActive(false);
            isPaused = false;
        }
    }

    // When GameObject get enabled
    private void OnEnable() { inpSys.Player.Enable(); }
    // When GameObject get disables
    private void onDisable() { inpSys.Player.Disable(); }
}
