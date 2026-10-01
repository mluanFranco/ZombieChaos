using UnityEngine;

public class ColetavelVida : MonoBehaviour
{
    [Header("Cura")]
    [SerializeField] private int quantidadeDeVida = 30;

    [Tooltip("Segundos até sumir sozinho. 0 = nunca some")]
    [SerializeField] private float tempoDeVida = 45f;

    [Header("Visual")]
    [SerializeField] private float velocidadeRotacao = 90f;
    [SerializeField] private float alturaFlutuacao = 0.15f;
    [SerializeField] private float velocidadeFlutuacao = 2f;

    [Header("Som")]
    [SerializeField] private AudioClip somColeta;

    private Vector3 posicaoInicial;
    private GeradorDeColetaveis gerador;
    private Transform pontoDeOrigem;

    public void Configurar(GeradorDeColetaveis gerador, Transform pontoDeOrigem)
    {
        this.gerador = gerador;
        this.pontoDeOrigem = pontoDeOrigem;
    }

    private void Start()
    {
        posicaoInicial = transform.position;

        if (tempoDeVida > 0f)
        {
            Destroy(gameObject, tempoDeVida);
        }
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, velocidadeRotacao * Time.deltaTime, Space.World);

        float deslocamento = Mathf.Sin(Time.time * velocidadeFlutuacao) * alturaFlutuacao;
        transform.position = posicaoInicial + Vector3.up * deslocamento;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Só coleta se o jogador realmente precisar de vida
        if (Jogador.Instance.RecuperarVida(quantidadeDeVida))
        {
            if (somColeta)
            {
                // PlayClipAtPoint continua tocando mesmo após o objeto ser destruído
                AudioSource.PlayClipAtPoint(somColeta, transform.position);
            }

            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        // Chamado tanto na coleta quanto quando o tempo de vida acaba
        if (gerador)
        {
            gerador.ColetavelRemovido(pontoDeOrigem);
        }
    }
}