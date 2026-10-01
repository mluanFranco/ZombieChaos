using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeradorDeColetaveis : MonoBehaviour
{
    [SerializeField] private ColetavelVida prefabColetavelVida;
    [SerializeField] private Transform[] pontosDeSpawn;

    [Header("Ritmo")]
    [Tooltip("Tempo antes do primeiro kit aparecer")]
    [SerializeField] private float atrasoInicial = 20f;
    [SerializeField] private float intervaloMinimo = 15f;
    [SerializeField] private float intervaloMaximo = 25f;

    [Tooltip("Quantidade máxima de kits no mapa ao mesmo tempo")]
    [SerializeField] private int maximoAtivos = 3;

    [Header("Posicionamento")]
    [Tooltip("Evita que o kit apareça colado no jogador")]
    [SerializeField] private float distanciaMinimaDoJogador = 10f;

    private readonly HashSet<Transform> pontosOcupados = new HashSet<Transform>();
    private Transform jogador;

    private void Start()
    {
        jogador = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(GerarColetaveis());
    }

    private IEnumerator GerarColetaveis()
    {
        yield return new WaitForSeconds(atrasoInicial);

        while (true)
        {
            if (pontosOcupados.Count < maximoAtivos)
            {
                TentarGerarColetavel();
            }

            yield return new WaitForSeconds(Random.Range(intervaloMinimo, intervaloMaximo));
        }
    }

    private void TentarGerarColetavel()
    {
        List<Transform> pontosLivres = new List<Transform>();

        foreach (Transform ponto in pontosDeSpawn)
        {
            bool livre = !pontosOcupados.Contains(ponto);
            bool longeDoJogador = Vector3.Distance(ponto.position, jogador.position) >= distanciaMinimaDoJogador;

            if (livre && longeDoJogador)
            {
                pontosLivres.Add(ponto);
            }
        }

        // Nenhum ponto disponível agora: tenta de novo no próximo ciclo
        if (pontosLivres.Count == 0) return;

        Transform pontoEscolhido = pontosLivres[Random.Range(0, pontosLivres.Count)];

        ColetavelVida coletavel = Instantiate(prefabColetavelVida, pontoEscolhido.position, Quaternion.identity);
        coletavel.Configurar(this, pontoEscolhido);

        pontosOcupados.Add(pontoEscolhido);
    }

    public void ColetavelRemovido(Transform ponto)
    {
        pontosOcupados.Remove(ponto);
    }
}