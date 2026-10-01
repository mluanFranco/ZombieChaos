using Unity.Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GerenciadorDeArmas : MonoBehaviour
{
    [SerializeField] private List<Arma> armasDisponiveis;

    [SerializeField] private Arma armaPrimaria;
    [SerializeField] private Arma armaSecundaria;

    [SerializeField] private GameObject efeitoImpactoDeTiro;
    [SerializeField] private GameObject efeitoDeSangue;

    [SerializeField] private LayerMask tiroLayerMask;

    private Transform cameraPrincipal;

    private float tempoProximoTiro;

    private MovimentoJogador movimentoJogador;

    private Coroutine recarregarCoroutine;

    private bool recarregando;

    [SerializeField] private CinemachinePanTilt panTilt;
    private float tempoRecoil;

    [SerializeField] private CinemachineImpulseSource cinemachineImpulseSource;
    [SerializeField] private Animator armaOffsetAnimator;
    private bool mudandoArma;

    void Start()
    {
        cameraPrincipal = Camera.main.transform;
        armaPrimaria.gameObject.SetActive(true);

        // "?." não funciona bem com objetos da Unity, por isso a checagem explícita
        if (armaSecundaria != null)
        {
            armaSecundaria.gameObject.SetActive(false);
        }

        movimentoJogador = GetComponent<MovimentoJogador>();

        AtualizarInterfaceArma(armaPrimaria);
    }

    void Update()
    {
        if (recarregando || mudandoArma) return;

        Arma armaAtual = GetArmaAtual();
        Recarregar(armaAtual);
        Atirar(armaAtual);
        AplicarRecoil(armaAtual);
        Mirar(armaAtual);
        TrocarArma();
    }

    public Arma GetArmaAtual()
    {
        if (armaSecundaria != null && armaSecundaria.gameObject.activeSelf)
        {
            return armaSecundaria;
        }

        return armaPrimaria;
    }

    private void Atirar(Arma armaAtual)
    {
        if (Input.GetButton("Fire1") && Time.time >= tempoProximoTiro && armaAtual.municaoAtual > 0 && !movimentoJogador.EstaCorrendo())
        {
            cinemachineImpulseSource.GenerateImpulse();
            tempoProximoTiro = Time.time + 1f / armaAtual.tirosPorSegundo;

            armaAtual.RealizarDisparo();

            RaycastHit hit;
            if (Physics.Raycast(cameraPrincipal.position, cameraPrincipal.forward, out hit, 1000, tiroLayerMask, QueryTriggerInteraction.Ignore))
            {
                var parteDoCorpoInimigo = hit.transform.GetComponent<ParteDoCorpo>();

                if (parteDoCorpoInimigo)
                {
                    Instantiate(efeitoDeSangue, hit.point, Quaternion.LookRotation(hit.normal));

                    var vidaInimigo = parteDoCorpoInimigo.transform.root.GetComponent<VidaInimigo>();

                    int dano = armaAtual.GetDano(hit.distance, parteDoCorpoInimigo.nivelDeDano);

                    bool inimigoMorto = vidaInimigo.ReduzirVida(dano);

                    if (inimigoMorto)
                    {
                        Jogador.Instance.NovoMonstroDerrotado();

                        if (parteDoCorpoInimigo.nivelDeDano == NivelDeDano.ALTO)
                        {
                            Jogador.Instance.AdicionarPontos(vidaInimigo.GetPontosDerrota() * 2);
                            InterfaceDeUsuario.Instance.ExecutarHeadShot();
                        }
                        else
                        {
                            Jogador.Instance.AdicionarPontos(vidaInimigo.GetPontosDerrota());
                        }
                    }
                }
                else
                {
                    Instantiate(efeitoImpactoDeTiro, hit.point, Quaternion.LookRotation(hit.normal));
                }
            }

            tempoRecoil = 0.2f;
            armaAtual.ProximoRecoil();

            AtualizarInterfaceArma(armaAtual);
        }
    }

    private void Recarregar(Arma armaAtual)
    {
        if ((Input.GetKeyDown(KeyCode.R) || armaAtual.municaoAtual <= 0) && armaAtual.municaoNoInventario > 0)
        {
            recarregarCoroutine = StartCoroutine(ExecutarRecarga(armaAtual));
        }
    }

    private void CancelarRecarga()
    {
        if (recarregarCoroutine != null)
        {
            StopCoroutine(recarregarCoroutine);
        }
        recarregando = false;

        InterfaceDeUsuario.Instance.ExibirMira(true);
    }

    private IEnumerator ExecutarRecarga(Arma armaAtual)
    {
        recarregando = true;
        armaAtual.animator.SetTrigger("Recarregar");
        armaAtual.animator.SetBool("Mirar", false);
        InterfaceDeUsuario.Instance.ExibirMira(true);

        if (armaAtual.modeloDaArma == ModeloDaArma.SHOTGUN)
        {
            int balasParaRecarregar = Mathf.Min(armaAtual.capacidadeDoPente, armaAtual.municaoNoInventario) - armaAtual.municaoAtual;
            yield return new WaitForSeconds(armaAtual.tempoDelayRecarregar);

            for (int i = 0; i < balasParaRecarregar; i++)
            {
                if (i == balasParaRecarregar - 1)
                {
                    armaAtual.animator.SetTrigger("FimRecarregar");
                }

                armaAtual.RecarregarArma(1);
                yield return new WaitForSeconds(armaAtual.tempoDelayRecarregar);

                AtualizarInterfaceArma(armaAtual);
            }
        }
        else
        {
            yield return new WaitForSeconds(armaAtual.tempoDelayRecarregar);
            int balasParaRecarregar = Mathf.Min(armaAtual.capacidadeDoPente, armaAtual.municaoNoInventario) - armaAtual.municaoAtual;
            armaAtual.RecarregarArma(balasParaRecarregar);
        }

        AtualizarInterfaceArma(armaAtual);

        recarregando = false;
    }

    private void AplicarRecoil(Arma armaAtual)
    {
        if (tempoRecoil <= 0f) return;

        tempoRecoil -= Time.deltaTime;
        Vector2 recoilAtual = armaAtual.ObterRecoilAtual();
        panTilt.PanAxis.Value += recoilAtual.x * Time.deltaTime;
        panTilt.TiltAxis.Value += recoilAtual.y * Time.deltaTime;
    }

    private void Mirar(Arma armaAtual)
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            armaAtual.AlterarMira();
        }
    }

    private void TrocarArma()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartCoroutine(AlterarArma(armaPrimaria));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && armaSecundaria != null)
        {
            StartCoroutine(AlterarArma(armaSecundaria));
        }
    }

    private IEnumerator AlterarArma(Arma novaArma)
    {
        if (novaArma == null) yield break;

        Arma armaAtual = GetArmaAtual();

        if (armaAtual == novaArma) yield break;

        mudandoArma = true;
        CancelarRecarga();
        armaOffsetAnimator.SetBool("MostrarArma", false);
        yield return new WaitForSeconds(0.5f);

        // Desativa tudo que estiver equipado (a secundária pode ainda não existir)
        armaPrimaria.gameObject.SetActive(false);

        if (armaSecundaria != null)
        {
            armaSecundaria.gameObject.SetActive(false);
        }

        // Arma comprada que não é a primária vira a nova secundária
        if (novaArma != armaPrimaria)
        {
            armaSecundaria = novaArma;
        }

        novaArma.gameObject.SetActive(true);

        armaOffsetAnimator.SetBool("MostrarArma", true);
        yield return new WaitForSeconds(0.5f);

        AtualizarInterfaceArma(novaArma);

        mudandoArma = false;
    }

    private void AtualizarInterfaceArma(Arma armaAtual)
    {
        if (armaAtual != null)
        {
            InterfaceDeUsuario.Instance.AtualizarMunicao(armaAtual.municaoAtual, armaAtual.municaoNoInventario);
        }
    }

    public void EquiparNovaArma(ModeloDaArma modeloDaArma)
    {
        foreach (var arma in armasDisponiveis)
        {
            if (arma.modeloDaArma == modeloDaArma)
            {
                StartCoroutine(AlterarArma(arma));
                break;
            }
        }
    }

    public void EquiparMunicao(ModeloDaArma modeloDaArma)
    {
        foreach (var arma in armasDisponiveis)
        {
            if (arma.modeloDaArma == modeloDaArma)
            {
                arma.CarregarInventario();
                AtualizarInterfaceArma(GetArmaAtual());
            }
        }
    }
}