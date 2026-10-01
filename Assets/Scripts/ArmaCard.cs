using UnityEngine;
using UnityEngine.UI;

public class ArmaCard : MonoBehaviour
{
    [SerializeField] private Button comprarArmaButton;
    [SerializeField] private Button comprarMunicaoButton;

    [SerializeField] private int valorDaArma;
    [SerializeField] private int valorDaMunicao;

    [SerializeField] private ModeloDaArma modeloDaArma;

    [SerializeField] private GerenciadorDeArmas gerenciadorDeArmas;
    [SerializeField] private GerenciadorLoja gerenciadorLoja;

    private void OnEnable()
    {
        comprarArmaButton.interactable = Jogador.Instance.GetPontos() >= valorDaArma;
        comprarMunicaoButton.interactable = Jogador.Instance.GetPontos() >= valorDaMunicao;
    }

    public void ComprarArma()
    {
        if (Jogador.Instance.GetPontos() >= valorDaArma)
        {
            Jogador.Instance.ReduzirPontos(valorDaArma);
            gerenciadorDeArmas.EquiparNovaArma(modeloDaArma);
            gerenciadorLoja.FecharLoja();

            InterfaceDeUsuario.Instance.TocarSomDeClique();
        }
    }

    public void ComprarMunicao()
    {
        if (Jogador.Instance.GetPontos() >= valorDaMunicao)
        {
            Jogador.Instance.ReduzirPontos(valorDaMunicao);
            gerenciadorDeArmas.EquiparMunicao(modeloDaArma);
            gerenciadorLoja.FecharLoja();

            InterfaceDeUsuario.Instance.TocarSomDeClique();
        }
    }
}
