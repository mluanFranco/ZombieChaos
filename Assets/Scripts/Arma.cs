using UnityEngine;

public class Arma : MonoBehaviour
{
    public int tirosPorSegundo;
    public int capacidadeDoPente;
    public int municaoNoInventario;
    public int quantidadeMaximaDeMunicaoNoInventario;
    public int municaoAtual;

    public ParticleSystem efeitoDisparo;

    [SerializeField] private Vector2[] padraoRecoil;
    private int indiceRecoil;

    public int danoBaixo;
    public int danoMedio;
    public int danoAlto;
    public int distanciaParaDanoMaximo;

    [Range(0,1)]
    public float multiplicadorDanoReduzindo;

    public ModeloDaArma modeloDaArma;
    public Animator animator;

    public float tempoDelayRecarregar;

    [SerializeField] private AudioSource disparoAudioSource;

    private void Awake()
    {
        municaoAtual = capacidadeDoPente;
        animator = GetComponent<Animator>();
    }
    
    public Vector2 ObterRecoilAtual()
    {
        return padraoRecoil[indiceRecoil];
    }

    public void ProximoRecoil()
    {
        indiceRecoil++;
        if (indiceRecoil >= padraoRecoil.Length)
        {
            indiceRecoil = 0;
        }
    }

    public void RealizarDisparo()
    {
        municaoAtual--;
        animator.SetTrigger("Atirar");
        efeitoDisparo.Play();
        disparoAudioSource.PlayOneShot(disparoAudioSource.clip);
    }

    public void RecarregarArma(int quantidade)
    {
        municaoAtual += quantidade;
        municaoNoInventario -= quantidade;
    }

    public void AlterarMira()
    {
        bool miraAtiva = animator.GetBool("Mirar");
        animator.SetBool("Mirar", !miraAtiva);

        InterfaceDeUsuario.Instance.ExibirMira(miraAtiva);
    }

    public void CarregarInventario()
    {
        municaoNoInventario = quantidadeMaximaDeMunicaoNoInventario;
    }

    public int GetDano(float distancia, NivelDeDano nivelDeDano)
    {
        int dano = 0;

        switch (nivelDeDano)
        {
            case NivelDeDano.BAIXO:
                dano = danoBaixo;
                break;
            case NivelDeDano.MEDIO:
                dano = danoMedio;
                break;
            case NivelDeDano.ALTO:
                dano = danoAlto;
                break;
        }

        if (distancia > distanciaParaDanoMaximo)
        {
            dano = (int)(dano * multiplicadorDanoReduzindo);
        }

        return dano;
    }
}

public enum ModeloDaArma
{
    PISTOLA,
    SHOTGUN,
    M4A1,
    SMG45,
    AK47,
    LMG,
}