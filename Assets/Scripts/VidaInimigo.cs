using UnityEngine;
using UnityEngine.Events;

public class VidaInimigo : MonoBehaviour
{
    [SerializeField] private int vida = 100;
    [SerializeField] private UnityEvent OnMorrer;
    [SerializeField] private int pontosDerrota;
    
    public bool ReduzirVida(int valor)
    {
        if (vida <= 0) return false;

        vida -= valor;

        if (vida <= 0)
        {
            OnMorrer.Invoke();
            return true;
        }

        return false;
    }

    public int GetPontosDerrota()
    {
        return pontosDerrota;
    }
}
