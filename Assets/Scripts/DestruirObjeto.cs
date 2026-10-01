using UnityEngine;

public class DestruirObjeto : MonoBehaviour
{
    [SerializeField] private float tempoDeVida;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, tempoDeVida);
    }
}
