using UnityEngine;

public class HitboxInimigo : MonoBehaviour
{
    private bool jogadorNaHitbox;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorNaHitbox = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorNaHitbox = false;
        }
    }

    public bool GetJogadorNaHitbox()
    {
        return jogadorNaHitbox;
    }
}
