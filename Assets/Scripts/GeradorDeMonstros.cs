using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeradorDeMonstros : MonoBehaviour
{
    [Tooltip("Array de prefabs de monstros, ordenados por dificuldade")]
    [SerializeField] private GameObject[] monstros;

    [SerializeField] private Transform[] pontosDeSpawn;

    [Header("Dificuldade")]
    [Tooltip("Quantidade de monstros na onda 1")]
    [SerializeField] private int monstrosIniciaisPorOnda = 6;

    [Tooltip("Quantos monstros a mais surgem a cada nova onda")]
    [SerializeField] private float incrementoPorOnda = 2f;

    [Tooltip("A cada quantas ondas um novo tipo de monstro é liberado")]
    [SerializeField] private int ondasParaNovoTipo = 2;

    [Header("Spawn")]
    [SerializeField] private float distanciaMinimaDoJogador = 15f;

    [Tooltip("Pequeno atraso entre monstros da mesma sub-onda, para não nascerem empilhados")]
    [SerializeField] private float intervaloEntreSpawns = 0.4f;

    private int ondaAtual = 1;
    private float tempoRestanteParaProximaOnda;

    private Transform jogador;

    [SerializeField] private AudioSource novaOndaAudioSource;

    private void Start()
    {
        jogador = GameObject.FindGameObjectWithTag("Player").transform;

        StartCoroutine(GerarOndas());

        InterfaceDeUsuario.Instance.AtualizarTempoRestante(tempoRestanteParaProximaOnda);
        InterfaceDeUsuario.Instance.AtualizarOndaAtual(ondaAtual);
    }

    private void Update()
    {
        tempoRestanteParaProximaOnda -= Time.deltaTime;
        InterfaceDeUsuario.Instance.AtualizarTempoRestante(tempoRestanteParaProximaOnda);
    }

    private int CalcularMonstrosDaOnda(int onda)
    {
        // Crescimento linear: fácil de balancear pelo Inspector
        return Mathf.CeilToInt(monstrosIniciaisPorOnda + incrementoPorOnda * (onda - 1));
    }

    private IEnumerator GerarOndas()
    {
        while (true)
        {
            Jogador.Instance.RestaurarVida();
            novaOndaAudioSource.Play();

            tempoRestanteParaProximaOnda = 30 + 5 * ondaAtual;
            int totalMonstros = CalcularMonstrosDaOnda(ondaAtual);

            int subOndas = Mathf.CeilToInt(tempoRestanteParaProximaOnda / 20f);
            float intervaloSubOnda = tempoRestanteParaProximaOnda / subOndas;

            // Distribui o total sem perder monstros na divisão inteira
            int monstrosPorSubOnda = totalMonstros / subOndas;
            int resto = totalMonstros % subOndas;

            for (int i = 0; i < subOndas; i++)
            {
                int quantidade = monstrosPorSubOnda + (i < resto ? 1 : 0);

                // Roda em paralelo para não atrasar o cronômetro da onda
                StartCoroutine(GerarMonstros(quantidade));

                yield return new WaitForSeconds(intervaloSubOnda);
            }

            ondaAtual++;
            InterfaceDeUsuario.Instance.AtualizarOndaAtual(ondaAtual);
        }
    }

    private IEnumerator GerarMonstros(int quantidadeDeMonstros)
    {
        int tiposLiberados = Mathf.Clamp(ondaAtual / ondasParaNovoTipo, 1, monstros.Length);

        for (int i = 0; i < quantidadeDeMonstros; i++)
        {
            int indiceTipoDeMonstro = Random.Range(0, tiposLiberados);
            Transform pontoDeSpawn = EscolherPontoDeSpawn();

            Instantiate(monstros[indiceTipoDeMonstro], pontoDeSpawn.position, pontoDeSpawn.rotation);

            yield return new WaitForSeconds(intervaloEntreSpawns);
        }
    }

    private Transform EscolherPontoDeSpawn()
    {
        List<Transform> pontosValidos = new List<Transform>();
        Transform pontoMaisDistante = pontosDeSpawn[0];
        float maiorDistancia = 0f;

        foreach (Transform ponto in pontosDeSpawn)
        {
            float distancia = Vector3.Distance(ponto.position, jogador.position);

            if (distancia >= distanciaMinimaDoJogador)
            {
                pontosValidos.Add(ponto);
            }

            if (distancia > maiorDistancia)
            {
                maiorDistancia = distancia;
                pontoMaisDistante = ponto;
            }
        }

        // Se nenhum ponto estiver longe o bastante, usa o mais distante (evita loop infinito)
        if (pontosValidos.Count == 0)
        {
            return pontoMaisDistante;
        }

        return pontosValidos[Random.Range(0, pontosValidos.Count)];
    }
}