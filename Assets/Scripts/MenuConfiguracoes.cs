using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuConfiguracoes : MonoBehaviour
{
    [SerializeField] private GameObject menuPause;

    [SerializeField] private CinemachineInputAxisController cinemachineInputAxisController;

    [SerializeField] private Slider sensibilidadeSlider;
    [SerializeField] private Slider audioSlider;
    [SerializeField] private TMP_Dropdown qualidadeDropdown;

    void Start()
    {
        CarregarConfiguracoes();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && menuPause)
        {
            menuPause.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Jogador.Instance.PausarJogador();
        }
    }

    public void RetomarPartida()
    {
        menuPause.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Jogador.Instance.RetormarJogador();
    }

    public void SalvarSensibilidade()
    {
        float sensibilidade = sensibilidadeSlider.value;

        PlayerPrefs.SetFloat("Sensibilidade", sensibilidade);
        PlayerPrefs.Save();

        if (cinemachineInputAxisController)
        {
            cinemachineInputAxisController.Controllers[0].Input.LegacyGain = 120 * sensibilidade;
            cinemachineInputAxisController.Controllers[1].Input.LegacyGain = -120 * sensibilidade;
        }
    }

    public void SalvarAudio()
    {
        float audio = audioSlider.value;
        PlayerPrefs.SetFloat("Audio", audio);
        PlayerPrefs.Save();

        AudioListener.volume = audio;
    }

    public void SalvarQualidade()
    {
        int qualidadeIndex = qualidadeDropdown.value;

        PlayerPrefs.SetInt("Qualidade", qualidadeIndex);
        PlayerPrefs.Save();

        QualitySettings.SetQualityLevel(qualidadeIndex);
    }

    public void SalvarConfiguracoes()
    {
        SalvarSensibilidade();
        SalvarAudio();
        SalvarQualidade();
    }

    public void CarregarConfiguracoes()
    {
        float sensibilidade = PlayerPrefs.GetFloat("Sensibilidade", 1.0f);
        float audio = PlayerPrefs.GetFloat("Audio", 1.0f);
        int qualidade = PlayerPrefs.GetInt("Qualidade", 3);

        audioSlider.value = audio;
        AudioListener.volume = audio;

        if (cinemachineInputAxisController)
        {
            cinemachineInputAxisController.Controllers[0].Input.LegacyGain = 120 * sensibilidade;
            cinemachineInputAxisController.Controllers[1].Input.LegacyGain = -120 * sensibilidade;
        }

        sensibilidadeSlider.value = sensibilidade;

        qualidadeDropdown.value = qualidade;

        QualitySettings.SetQualityLevel(qualidade);
    }

    public void CarregarNovaCena(int indexCena)
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(indexCena);
    }

    public void SairDoJogo()
    {
        Application.Quit();
    }
}
