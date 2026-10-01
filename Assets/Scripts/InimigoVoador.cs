using UnityEngine;
using UnityEngine.AI;

public class InimigoVoador : MonoBehaviour
{
    private Transform jogador;
    private NavMeshAgent navMeshAgent;
    private Animator animator;

    [SerializeField] private float distanciaDeAtaque;
    [SerializeField] private float intervaloEntreAtaques;

    [SerializeField] private GameObject bolaAcida;
    [SerializeField] private Transform pontoDeLancamento;

    private float tempoProximoAtaque;

    [SerializeField] private AudioSource atacarAudioSource;

    private void Start()
    {
        jogador = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        float distanciaParaJogador = Vector3.Distance(jogador.position, transform.position);

        if (distanciaParaJogador <= distanciaDeAtaque)
        {
            navMeshAgent.velocity = Vector3.zero;

            if (Time.time > tempoProximoAtaque)
            {
                Atacar();
            }
        }
        else
        {
            navMeshAgent.SetDestination(jogador.position);
        }
    }

    private void Atacar()
    {
        atacarAudioSource.Play();
        
        tempoProximoAtaque = Time.time + intervaloEntreAtaques;

        pontoDeLancamento.LookAt(jogador);
        Instantiate(bolaAcida, pontoDeLancamento.position, pontoDeLancamento.rotation);
    }

    public void Morrer()
    {
        enabled = false;
        animator.SetTrigger("Morrer");
        Destroy(gameObject, 0.6f);
    }
}
