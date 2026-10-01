using UnityEngine;

public class Recarregar : MonoBehaviour
{
    [SerializeField] private AudioSource recarregarAudioSource;

    [SerializeField] private AudioClip recarregar1AudioClip;
    [SerializeField] private AudioClip recarregar2AudioClip;
    [SerializeField] private AudioClip recarregar3AudioClip;

    public void Recarregar1()
    {
        recarregarAudioSource.PlayOneShot(recarregar1AudioClip);
    }

    public void Recarregar2()
    {
        recarregarAudioSource.PlayOneShot(recarregar2AudioClip);
    }

    public void Recarregar3()
    {
        recarregarAudioSource.PlayOneShot(recarregar3AudioClip);
    }
}
