using UnityEngine;
using TMPro;
using System.Collections;

public class TextWriter : MonoBehaviour
{
    public static TextWriter Instance { get; private set; }

    [SerializeField] private bool _isWriting = false;
    [SerializeField] private string _currentText = "";

    [SerializeField] private float _charactersPerSecond = 2f;
    public float CharactersPerSecond
    {
        get { return _charactersPerSecond; }
        set { _charactersPerSecond = value; }
    }

    [SerializeField] private TMP_Text _informationTxt;
    [SerializeField] private TMP_Text _headerTxt;

    private Coroutine _typingCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Starts typing the provided message with the specified header. If a typing coroutine is already running, it will be stopped before starting a new one.
    /// </summary>
    /// <param name="message">The message to be typed out.</param>
    /// <param name="header">The header to be displayed above the message.</param>
    public void WriteText(string message, string header)
    {
        if (_isWriting && _typingCoroutine != null)
            StopCoroutine(_typingCoroutine);

        _isWriting = false;

        _headerTxt.text = header;

        _typingCoroutine = StartCoroutine(TypeText(message));
    }

    /// <summary>
    /// Removes the current text and header, and stops any ongoing typing coroutine.
    /// </summary>
    public void RemoveText()
    {
        _headerTxt.text = "";
        _informationTxt.text = "";

        if (_isWriting && _typingCoroutine != null)
            StopCoroutine(_typingCoroutine);

        _isWriting = false;
    }

    /// <summary>
    /// Types the text character by character based on the specified characters per second.
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>/
    private IEnumerator TypeText(string message)
    {
        _isWriting = true;

        _informationTxt.text = message;
        int totalCharacters = _informationTxt.text.Length;

        if (_charactersPerSecond <= 0)
            yield break;

        _informationTxt.maxVisibleCharacters = 0;

        float timeToWaitForNextChar = 1f / _charactersPerSecond;

        for (int i = 0; i <= totalCharacters; i++)
        {
            _informationTxt.maxVisibleCharacters++;
            yield return new WaitForSeconds(timeToWaitForNextChar);
        }
        _isWriting = false;
    }
}