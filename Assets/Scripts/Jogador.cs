using System.Data;
using UnityEngine;

public class Jogador : MonoBehaviour
{
    public static Jogador Instance;

    [SerializeField] private int vidaMaxima = 100;
    private int vidaAtual;
    private bool estaMorto;

    [SerializeField] private int pontos;

    private MovimentoJogador movimentoJogador;
    private GerenciadorDeArmas gerenciadorDeArmas;
    [SerializeField] private GameObject cinemachine;

    private int monstrosDerrotados;

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
        vidaAtual = vidaMaxima;
        AtualizarBarraDeVida();
        InterfaceDeUsuario.Instance.AtualizarPontos(0, pontos);

        movimentoJogador = GetComponent<MovimentoJogador>();
        gerenciadorDeArmas = GetComponent<GerenciadorDeArmas>();
    }

    public void ReduzirVida(int valor)
    {
        if (estaMorto) return;

        InterfaceDeUsuario.Instance.AtivarEfeitoDeDano();
        vidaAtual -= valor;
        AtualizarBarraDeVida();

        if (vidaAtual <= 0)
        {
            Morrer();
        }
    }

    private void Morrer()
    {
        estaMorto = true;
        Time.timeScale = 0;

        InterfaceDeUsuario.Instance.ExibirGameOver();
    }

    private void AtualizarBarraDeVida()
    {
        InterfaceDeUsuario.Instance.AtualizarBarraDeVida(vidaAtual, vidaMaxima);
    }

    public void AdicionarPontos(int valor)
    {
        pontos += valor;
        InterfaceDeUsuario.Instance.AtualizarPontos(valor, pontos);
    }

    public void ReduzirPontos(int valor)
    {
        pontos = Mathf.Max(0, pontos - valor);
        InterfaceDeUsuario.Instance.AtualizarPontos(-valor, pontos);
    }

    public void PausarJogador()
    {
        movimentoJogador.enabled = false;
        gerenciadorDeArmas.enabled = false;
        cinemachine.SetActive(false);
    }

    public void RetormarJogador()
    {
        movimentoJogador.enabled = true;
        gerenciadorDeArmas.enabled = true;
        cinemachine.SetActive(true);
    }

    public int GetPontos()
    {
        return pontos;
    }

    public void RestaurarVida()
    {
        vidaAtual = vidaMaxima;
        AtualizarBarraDeVida();
    }

    public void NovoMonstroDerrotado()
    {
        monstrosDerrotados++;
    }

    public int GetMonstrosDerrotados()
    {
        return monstrosDerrotados;
    }

    public bool RecuperarVida(int valor)
    {
        if (estaMorto || vidaAtual >= vidaMaxima) return false;

        vidaAtual = Mathf.Min(vidaAtual + valor, vidaMaxima);
        AtualizarBarraDeVida();
        return true;
    }
}
