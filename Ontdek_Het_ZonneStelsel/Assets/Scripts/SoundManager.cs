using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlaySound(AudioClip clip, Vector3 position = new Vector3())
    {
        if (clip == null)
        {
            Debug.LogWarning("Attempted to play a sound, but the AudioClip was null.");
            return;
        }

        AudioSource.PlayClipAtPoint(clip, position);
    }
}
