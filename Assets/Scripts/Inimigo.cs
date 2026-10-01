using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Inimigo : MonoBehaviour
{
    private Transform jogador;
    private NavMeshAgent navMeshAgent;
    private Animator animator;

    [SerializeField] private float distanciaDeAtaque;

    private float tempoProximoAtaque;
    [SerializeField] private float intervaloEntreAtaques = 1;

    [SerializeField] private HitboxInimigo hitboxInimigo;

    [SerializeField] private int dano;

    [SerializeField] private AudioSource inimigoAudioSource;
    [SerializeField] private AudioClip[] sonsInimigoAudioClips;
    [SerializeField] private AudioSource atacarAudioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        jogador = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        InvokeRepeating(nameof(TocarSomDoInimigo), Random.Range(0f, 4f), Random.Range(4f, 8f));
    }

    // Update is called once per frame
    void Update()
    {
        float distanciaParaJogador = Vector3.Distance(jogador.position, transform.position);

        if (distanciaParaJogador < distanciaDeAtaque)
        {
            navMeshAgent.velocity = Vector3.zero;

            if (Time.time > tempoProximoAtaque)
            {
                PrepararAtaque();    
            }
            
        }
        else
        {
            navMeshAgent.SetDestination(jogador.position);
        }

        animator.SetBool("Mover", navMeshAgent.velocity.magnitude >= 0.1f);
    }

    private void PrepararAtaque()
    {
        Vector3 direcaoParaJogador = (jogador.position - transform.position).normalized;
        Quaternion rotacaoParaJogador = Quaternion.LookRotation(direcaoParaJogador);

        transform.rotation = rotacaoParaJogador;

        animator.SetTrigger("Atacar");
        tempoProximoAtaque = Time.time + intervaloEntreAtaques;
    }

    public void Morre()
    {
        enabled = false;
        animator.SetTrigger("Morrer");
        Destroy(gameObject, 2f);
    }

    public void RealizarAtaque()
    {
        atacarAudioSource.Play();
        
        if (hitboxInimigo.GetJogadorNaHitbox())
        {
            Jogador.Instance.ReduzirVida(dano);
        }
    }

    public void TocarSomDoInimigo()
    {
        inimigoAudioSource.PlayOneShot(sonsInimigoAudioClips[Random.Range(0, sonsInimigoAudioClips.Length)]);
    }
}
