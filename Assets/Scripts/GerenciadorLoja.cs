using UnityEngine;

public class GerenciadorLoja : MonoBehaviour
{
    private bool estaNaAreaDeCompra;
    private bool lojaEstaAberta;
    [SerializeField] private GameObject toolTipAbrirLoja;
    [SerializeField] private GameObject lojaUI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Loja"))
        {
            estaNaAreaDeCompra = true;
            toolTipAbrirLoja.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Loja"))
        {
            estaNaAreaDeCompra = false;
            toolTipAbrirLoja.SetActive(false);
            FecharLoja();
        }
    }

    private void Update()
    {
        if (estaNaAreaDeCompra && Input.GetKeyDown(KeyCode.F))
        {
            lojaEstaAberta = !lojaEstaAberta;

            if (lojaEstaAberta)
            {
                AbrirLoja();
            }
            else
            {
                FecharLoja();
            }
        }
    }

    private void AbrirLoja()
    {
        Cursor.lockState = CursorLockMode.None;
        lojaUI.SetActive(true);
        toolTipAbrirLoja.SetActive(false);

        Jogador.Instance.PausarJogador();
    }

    public void FecharLoja()
    {
        Cursor.lockState = CursorLockMode.Locked;
        lojaUI.SetActive(false);

        Jogador.Instance.RetormarJogador();
    }
}
