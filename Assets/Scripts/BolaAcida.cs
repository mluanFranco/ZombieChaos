using UnityEngine;

public class BolaAcida : MonoBehaviour
{
    [SerializeField] private int dano;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Jogador.Instance.ReduzirVida(dano);
            Destroy(gameObject, 2f);
            GetComponent<SphereCollider>().enabled = false;
        }
    }
}
