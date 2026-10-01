using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class InterfaceDeUsuario : MonoBehaviour
{
    public static InterfaceDeUsuario Instance;

    [SerializeField] private Slider staminaSlider;
    [SerializeField] private TMP_Text municaoText;
    [SerializeField] private Image miraImage;
    [SerializeField] private Slider barraDeVidaSlider;
    [SerializeField] private TMP_Text pontosText;

    [SerializeField] private TMP_Text ondaAtualText;
    [SerializeField] private TMP_Text tempoRestanteProximaOndaText;

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text gameOverOndasText;
    [SerializeField] private TMP_Text gameOverMonstrosDerrotadosText;

    [SerializeField] private Volume danoVolume;
    [SerializeField] private AudioSource danoAudioSource;
    [SerializeField] private AudioSource respiracaoAudioSource;

    private Coroutine danoVolumeCoroutine;

    [SerializeField] private Animator headshotAnimator;
    [SerializeField] private AudioSource headshotAudioSource;

    [SerializeField] private TMP_Text pontosRecebidosText;
    [SerializeField] private Animator pontosRecebidosAnimator;

    [SerializeField] private AudioSource cliqueAudioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void AtualizarStamina(float stamina)
    {
        staminaSlider.value = stamina;
        staminaSlider.gameObject.SetActive(stamina < 0.99f);
    }

    public void AtualizarMunicao(int municaoAtual, int municaoNoInventario)
    {
        municaoText.text = municaoAtual + "/" + municaoNoInventario;
    }

    public void ExibirMira(bool exibirMira)
    {
        miraImage.enabled = exibirMira;
    }

    public void AtualizarBarraDeVida(int vidaAtual, int vidaMaxima)
    {
        barraDeVidaSlider.maxValue = vidaMaxima;
        barraDeVidaSlider.value = vidaAtual;
    }

    public void AtualizarPontos(int variacao, int saldoAtual)
    {
        pontosText.text = "Pontos: " + saldoAtual;

        if (variacao > 0)
        {
            pontosRecebidosText.text = "+" + variacao;
            pontosRecebidosAnimator.SetTrigger("Executar");
        }
        
    }

    public void AtualizarOndaAtual(int ondaAtual)
    {
        ondaAtualText.text = "onda" + ondaAtual;

        gameOverOndasText.text = "Onda " + ondaAtual;
    }

    public void AtualizarTempoRestante(float tempo)
    {
        tempoRestanteProximaOndaText.text = tempo.ToString("00.0");
    }

    public void ExibirGameOver()
    {
        gameOverPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;

        gameOverMonstrosDerrotadosText.text = "Monstros Derrotados: " + Jogador.Instance.GetMonstrosDerrotados();
    }

    private IEnumerator DanoVolumeCoroutine()
    {
        danoAudioSource.Play();
        respiracaoAudioSource.Play();

        while (danoVolume.weight < 1)
        {
            danoVolume.weight += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(4);

        while (danoVolume.weight > 0)
        {
            danoVolume.weight -= Time.deltaTime;
            yield return null;
        }

        danoAudioSource.Stop();
        respiracaoAudioSource.Stop();
    }

    public void AtivarEfeitoDeDano()
    {
        if (danoVolumeCoroutine != null)
        {
            StopCoroutine(danoVolumeCoroutine);
        }

        danoVolumeCoroutine = StartCoroutine(DanoVolumeCoroutine());
    }

    public void ExecutarHeadShot()
    {
        headshotAnimator.SetTrigger("Executar");
        headshotAudioSource.PlayOneShot(headshotAudioSource.clip);
    }

    public void TocarSomDeClique()
    {
        cliqueAudioSource.Play();
    }
}
