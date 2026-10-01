using UnityEngine;

public class ArmaSway : MonoBehaviour
{
    [SerializeField] private float intensidade = 0.02f;
    [SerializeField] private float suavidade = 4.0f;

    private Vector3 posicaoInicial;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        posicaoInicial = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * intensidade;
        float mouseY = Input.GetAxis("Mouse Y") * intensidade;

        Vector3 posicaoAlvo = new Vector3(-mouseX, -mouseY, 0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, posicaoInicial + posicaoAlvo, Time.deltaTime * suavidade);
    }
}
