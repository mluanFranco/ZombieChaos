using UnityEngine;
using UnityEngine.AI;

public class MovimentoJogador : MonoBehaviour
{
    [SerializeField] private float velocidadeMovimento = 2.6f;
    private Transform cameraPrincipal;
    private CharacterController characterController;

    [SerializeField] private Transform pontoDeVerificacao;
    [SerializeField] private LayerMask camadaDeColisao;

    private bool estaNoChao;
    private float velocidadeVertical;
    private float velocidadeHorizontal;

    private bool estaCorrendo;
    private float nivelStamina;

    private GerenciadorDeArmas gerenciadorDeArmas;

    [SerializeField] private AudioSource passosAudioSource;
    [SerializeField] private AudioClip[] passosAudioClips;
    [SerializeField] private AudioClip pularAudioClip;

    [SerializeField] private float intervaloPassosAndando;
    [SerializeField] private float intervaloPassosCorrendo;

    private float temporizadorPassos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cameraPrincipal = Camera.main.transform;
        characterController = GetComponent<CharacterController>();
        gerenciadorDeArmas = GetComponent<GerenciadorDeArmas>();

        temporizadorPassos = intervaloPassosAndando;
    }

    // Update is called once per frame
    void Update()
    {
        AplicarGravidade();
        ProcessarMovimento();
        AtualizarStamina();
        SonsDePassos();
    }

    private void AplicarGravidade()
    {
        estaNoChao = Physics.CheckSphere(pontoDeVerificacao.position, 0.3f, camadaDeColisao);

        if (Input.GetKeyDown(KeyCode.Space) && estaNoChao)
        {
            velocidadeVertical = 4.5f;
            passosAudioSource.PlayOneShot(pularAudioClip);
        }

        if (!estaNoChao || velocidadeVertical > Physics.gravity.y)
        {
            velocidadeVertical += Physics.gravity.y * Time.deltaTime;
        }

        characterController.Move(new Vector3(0, velocidadeVertical, 0) * Time.deltaTime);
    }

    private void ProcessarMovimento()
    {
        float movimentoHorizontal = Input.GetAxis("Horizontal");
        float movimentoVertical = Input.GetAxis("Vertical");

        estaCorrendo = Input.GetKey(KeyCode.LeftShift);

        Vector3 direcaoMovimento = new Vector3(movimentoHorizontal, 0, movimentoVertical);
        direcaoMovimento = cameraPrincipal.TransformDirection(direcaoMovimento).normalized;
        direcaoMovimento.y = 0;

        float velocidadeAtual = estaCorrendo && nivelStamina > 0f ? velocidadeMovimento * 2f : velocidadeMovimento;

        if (estaCorrendo && nivelStamina > 0f)
        {
            nivelStamina -= Time.deltaTime;
            nivelStamina = Mathf.Max(0f, nivelStamina);
        }

        gerenciadorDeArmas.GetArmaAtual().animator.SetBool("Mover", direcaoMovimento != Vector3.zero);
        gerenciadorDeArmas.GetArmaAtual().animator.SetBool("Correr", estaCorrendo && nivelStamina > 0f);

        characterController.Move(direcaoMovimento * Time.deltaTime * velocidadeAtual);
    }

    private void AtualizarStamina()
    {
        if (!estaCorrendo && nivelStamina < 2f)
        {
            nivelStamina += Time.deltaTime;
        }

        InterfaceDeUsuario.Instance.AtualizarStamina(nivelStamina / 2f);
    }

    public bool EstaCorrendo()
    {
        return estaCorrendo && nivelStamina > 0f;
    }

    private void SonsDePassos()
    {
        if (characterController.velocity == Vector3.zero || !estaNoChao)
        {
            temporizadorPassos = intervaloPassosAndando;
            return;
        }

        temporizadorPassos -= Time.deltaTime;

        if (temporizadorPassos <= 0f)
        {
            int indice = Random.Range(0, passosAudioClips.Length);
            passosAudioSource.PlayOneShot(passosAudioClips[indice]);

            temporizadorPassos = estaCorrendo && nivelStamina >= 0f ? intervaloPassosCorrendo : intervaloPassosAndando;
        }
    }
}
