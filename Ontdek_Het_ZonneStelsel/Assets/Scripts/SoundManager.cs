using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public enum AudioType
    {
        Master,
        SFX,
        Music
    }

    #region AudioPlayer Class

    public class AudioPlayer
    {
        private readonly AudioSource _source;
        private readonly MonoBehaviour _runner;

        private Coroutine _playCoroutine;
        private Coroutine _fadeCoroutine;

        private float _targetVolume = 1f;

        public AudioPlayer(AudioSource source, MonoBehaviour runner)
        {
            _source = source;
            _runner = runner;
        }

        public void Play(AudioClip clip, Vector3 position, float targetVolume = 1f, float fade = 0f)
        {
            if (clip == null) return;

            StopImmediate();

            _playCoroutine = _runner.StartCoroutine(PlayCoroutine(clip, position, targetVolume, fade));
        }

        private IEnumerator PlayCoroutine(AudioClip clip, Vector3 position, float targetVolume, float fade)
        {
            _source.transform.position = position;
            _targetVolume = targetVolume;

            float fadeTime = Mathf.Min(fade, clip.length);

            if (fade > 0f)
                FadeIn(fade);
            else
                _source.volume = _targetVolume;

            _source.PlayOneShot(clip);

            yield return new WaitForSeconds(clip.length - fadeTime);

            if (fade > 0f)
                FadeOut(fade);
        }


        public void StopImmediate()
        {
            if (_playCoroutine != null) _runner.StopCoroutine(_playCoroutine);
            if (_fadeCoroutine != null) _runner.StopCoroutine(_fadeCoroutine);
            _source.Stop();
        }


        #region Fade

        public void FadeIn(float duration)
        {
            _source.volume = 0f;
            FadeTo(_targetVolume, duration);
        }

        public void FadeOut(float duration)
        {
            FadeTo(0f, duration, StopImmediate);
        }

        private void FadeTo(float target, float duration, Action onComplete = null)
        {
            if (_fadeCoroutine != null) _runner.StopCoroutine(_fadeCoroutine);
            _fadeCoroutine = _runner.StartCoroutine(FadeRoutine(target, duration, onComplete));
        }

        private IEnumerator FadeRoutine(float target, float duration, Action onComplete)
        {
            float start = _source.volume;
            float time = 0f;

            while (time < duration)
            {
                time += Time.deltaTime;
                _source.volume = Mathf.Lerp(start, target, time / duration);
                yield return null;
            }

            _source.volume = target;
            onComplete?.Invoke();
        }

        #endregion


        private void Shuffle(AudioClip[] array)
        {
            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = UnityEngine.Random.Range(0, i + 1);
                (array[i], array[j]) = (array[j], array[i]);
            }
        }


        public void PlayQueue(AudioClip[] clips, Vector3 position, bool loop = false, bool random = false, float fade = 0f, float targetVolume = 1f)
        {
            StopImmediate();
            _playCoroutine = _runner.StartCoroutine(QueueCoroutine(clips, position, loop, random, fade, targetVolume));
        }

        private IEnumerator QueueCoroutine(AudioClip[] clips, Vector3 position, bool loop, bool random, float fade, float targetVolume)
        {
            do
            {
                AudioClip[] queue = (AudioClip[])clips.Clone();
                if (random) Shuffle(queue);

                foreach (AudioClip clip in queue)
                {
                    if (clip == null) continue;

                    _source.transform.position = position;
                    _targetVolume = targetVolume;

                    float fadeTime = Mathf.Min(fade, clip.length);
                    if (fadeTime > 0f)
                        FadeIn(fadeTime);

                    _source.PlayOneShot(clip);

                    yield return new WaitForSeconds(clip.length - fadeTime);

                    if (fadeTime > 0f)
                        FadeOut(fadeTime);
                }
            }
            while (loop);
        }
    }

    #endregion

    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioSource _masterSource;
    private AudioPlayer _masterPlayer;

    [SerializeField] private AudioSource[] _sfxSources;
    private AudioPlayer[] _sfxPlayers;
    private int _nextSFXIndex = 0;

    [SerializeField] private AudioSource _musicSource;
    private AudioPlayer _musicPlayer;
    [SerializeField] private AudioSource _jingleSource;
    private AudioPlayer _jinglePlayer;

    [SerializeField] private AudioClip[] _musicClips;

    private const string MASTER_VOLUME_KEY = "MASTER_VOLUME";
    [SerializeField] private float _masterVolume = 1f;

    private const string SFX_VOLUME_KEY = "SFX_VOLUME";
    [SerializeField] private float _sfxVolume = 1f;

    private const string MUSIC_VOLUME_KEY = "MUSIC_VOLUME";
    [SerializeField] private float _musicVolume = 1f;

    [SerializeField] private Slider _masterSlider;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private Slider _musicSlider;

    [SerializeField] private InputField _masterInputField;
    [SerializeField] private InputField _sfxInputField;
    [SerializeField] private InputField _musicInputField;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeAudioPlayers();

        _masterSlider.onValueChanged.AddListener(SetMasterVolume);
        _sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        _musicSlider.onValueChanged.AddListener(SetMusicVolume);

        _masterInputField.onEndEdit.AddListener(SetMasterVolume);
        _sfxInputField.onEndEdit.AddListener(SetSFXVolume);
        _musicInputField.onEndEdit.AddListener(SetMusicVolume);
    }

    public void SetMasterVolume(float volume)
    {
        _masterVolume = Mathf.Clamp01(volume);
        _masterInputField.text = _masterVolume.ToString("0.00");
        PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, _masterVolume);
        ApplyVolumes();
    }

    public void SetMasterVolume(string volume)
    {
        if (float.TryParse(volume, out float vol))
        {
            SetMasterVolume(vol);
            _masterSlider.value = _masterVolume;
        }
    }

    public void SetSFXVolume(float volume)
    {
        _sfxVolume = Mathf.Clamp01(volume);
        _sfxInputField.text = _sfxVolume.ToString("0.00");
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, _sfxVolume);
        ApplyVolumes();
    }

    public void SetSFXVolume(string volume)
    {
        if (float.TryParse(volume, out float vol))
        {
            SetSFXVolume(vol);
            _sfxSlider.value = _sfxVolume;
        }

    }

    public void SetMusicVolume(float volume)
    {
        _musicVolume = Mathf.Clamp01(volume);
        _musicInputField.text = _musicVolume.ToString("0.00");
        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, _musicVolume);
        ApplyVolumes();
    }

    public void SetMusicVolume(string volume)
    {
        if (float.TryParse(volume, out float vol))
        {
            SetMusicVolume(vol);
            _musicSlider.value = _musicVolume;
        }
    }

    /// <summary>
    /// Initializes the AudioPlayer instances for each audio source. 
    /// </summary>
    private void InitializeAudioPlayers()
    {
        _masterPlayer = new AudioPlayer(_masterSource, this);

        _sfxPlayers = new AudioPlayer[_sfxSources.Length];
        for (int i = 0; i < _sfxSources.Length; i++)
            _sfxPlayers[i] = new AudioPlayer(_sfxSources[i], this);

        _musicPlayer = new AudioPlayer(_musicSource, this);
        _musicPlayer.PlayQueue(_musicClips, Vector3.zero, loop: true, random: true, fade: 2f, targetVolume: _musicVolume);

        _jinglePlayer = new AudioPlayer(_jingleSource, this);
    }

    private void Start()
    {
        _masterVolume = PlayerPrefs.GetFloat(MASTER_VOLUME_KEY, _masterVolume);
        _sfxVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, _sfxVolume);
        _musicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, _musicVolume);

        ApplyVolumes();
    }

    /// <summary>
    /// Applies the current volume settings to all audio sources.
    /// </summary>
    private void ApplyVolumes()
    {
        _masterSource.volume = _masterVolume;

        foreach (var source in _sfxSources)
            source.volume = _sfxVolume;

        _musicSource.volume = _musicVolume;
        _jingleSource.volume = _musicVolume;
    }


    /// <summary>
    /// Plays a sound on the master source,
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="position"></param>
    /// <param name="fade"></param>
    public void PlayMasterAudio(AudioClip clip, Vector3 position = new Vector3(), float fade = 0f)
    {
        if (clip == null) return;
        _masterPlayer.Play(clip, position, targetVolume: _masterVolume, fade: fade);
    }

    /// <summary>
    /// Plays a sound effect on the next available SFX source, allowing for multiple overlapping sound effects.
    /// If all sources are currently playing, it will stop the next source in line and play the new sound effect on it.
    /// This ensures that sound effects can always play without being cut off, while also preventing too many overlapping sounds.
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="position"></param>
    public void PlaySFXAudio(AudioClip clip, Vector3 position = new Vector3(), float fade = 0f)
    {
        if (clip == null || _sfxPlayers.Length == 0) return;

        AudioPlayer sfxPlayer = _sfxPlayers[_nextSFXIndex];

        sfxPlayer.Play(clip, position, targetVolume: _sfxVolume, fade: fade);

        _nextSFXIndex = (_nextSFXIndex + 1) % _sfxPlayers.Length;
    }

    /// <summary>
    /// Plays a music clip on the jingle source, allowing it to play over the background music.
    /// This is ideal for short music cues. 
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="fade"></param>
    public void PlayMusicAudio(AudioClip clip, float fade = 0f)
    {
        if (clip == null) return;
        _jinglePlayer.Play(clip, Vector3.zero, targetVolume: _musicVolume, fade: fade);
    }
}