using UnityEngine;

public class UIButtonSound : MonoBehaviour
{
    [SerializeField] private AudioClip clip;

    public void PlaySound()
    {
        SoundManager.Instance.PlaySFXAudio(clip);
    }
}
