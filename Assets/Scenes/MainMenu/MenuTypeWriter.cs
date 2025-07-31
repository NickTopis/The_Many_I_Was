using UnityEngine;
using TMPro;
using System.Collections;

[RequireComponent(typeof(TMP_Text))]
public class MenuTypeWriter : MonoBehaviour
{
    public bool writeText = false;
    private TMP_Text textBox;

    private string textToWrite;

    private int currentvisiblecharacteindex;
    private Coroutine writerCoroutine;

    private WaitForSeconds _simpleDelay;
    private WaitForSeconds _interDelay;

    [Header("WriterSetiings")]
    [SerializeField] private float charsPerSec = 20f;
    [SerializeField] private float interDelay = 0.5f;

    private void Awake()
    {
        textBox = GetComponent<TMP_Text>();
        textBox.enabled = false;
        _simpleDelay = new WaitForSeconds(1 / charsPerSec);
        _interDelay = new WaitForSeconds(interDelay);
        textToWrite = textBox.text;
    }

    private void Update()
    {
        if(writeText == true)
        {
            textBox.enabled = true;
            SetText(textToWrite);
            writeText = false;
        }

    }

    public void SetText(string text)
    {
        if(writerCoroutine != null)
        {
            StopCoroutine(writerCoroutine);
        }

        textBox.text = text;
        textBox.maxVisibleCharacters = 0;
        currentvisiblecharacteindex = 0;

        writerCoroutine = StartCoroutine(routine: Typewriter());
    }

    private IEnumerator Typewriter()
    {
        TMP_TextInfo textInfo = textBox.textInfo;
        while(currentvisiblecharacteindex < textInfo.characterCount + 1)
        {


            char character = textInfo.characterInfo[currentvisiblecharacteindex].character;

            textBox.maxVisibleCharacters++;

            if(character == '?' || character == '.'|| character == ','|| character == ':'||
               character == ';'|| character == '!'|| character == '-')
            {
                yield return _interDelay;
            }
            else
            {
                yield return _simpleDelay;
            }

            currentvisiblecharacteindex++;
        }
    }
}
