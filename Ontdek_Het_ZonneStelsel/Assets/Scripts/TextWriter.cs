using UnityEngine;
using TMPro;
using System.Collections;

public class TextWriter : MonoBehaviour
{
    public static TextWriter Instance { get; private set; }

    [SerializeField] private bool isWriting = false;
    [SerializeField] private string currentText = "";

    [SerializeField] private float _charactersPerSecond = 2f;
    public float CharactersPerSecond
    {
        get { return _charactersPerSecond; }
        set { _charactersPerSecond; }
    }

    [SerializeField] private TMP_Text _text;

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

    void Update()
    {
        if (isWriting)
        {
            StartCoroutine(TypeText(currentText));
            return;
        }
    }

    private IEnumerator TypeText(string message)
    {
        isWriting = false;

        _text.text = message;
        int totalCharacters = _text.text.Length;

        if (_charactersPerSecond <= 0)
            yield break;

        _text.maxVisibleCharacters = 0;

        float timeToWaitForNextChar = 1f / _charactersPerSecond;

        for (int i = 0; i <= totalCharacters; i++)
        {
            _text.maxVisibleCharacters++;
            yield return new WaitForSeconds(timeToWaitForNextChar);
        }
    }
}