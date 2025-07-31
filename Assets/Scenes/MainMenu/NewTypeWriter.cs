using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewTypeWriter : MonoBehaviour
{
    [SerializeField] bool writeText = false;
    [SerializeField] TextMeshProUGUI _textMeshPro;

    public string[] stringArray;

    [SerializeField] float timeBtwnChars;
    [SerializeField] float timeBtwnWords;

    int i = 0;

    private void Start()
    {
        _textMeshPro.enabled = false;
    }

    void Update()
    {
        if (writeText)
        {
            _textMeshPro.enabled = true;
            EndCheck();
            writeText = false;
        }
    }

    public void EndCheck()
    {
        if (i <= stringArray.Length - 1)
        {
            _textMeshPro.text = stringArray[i];
            StartCoroutine(TextVisible());
        }
    }

    private IEnumerator TextVisible()
    {
        _textMeshPro.ForceMeshUpdate();
        int totalVisibleCharacters = _textMeshPro.textInfo.characterCount;
        int counter = 0;

        while (true)
        {
            int visibleCount = counter % (totalVisibleCharacters + 1);
            _textMeshPro.maxVisibleCharacters = visibleCount;

            if (visibleCount >= totalVisibleCharacters)
            {
                i += 1;
                Invoke("EndCheck", timeBtwnWords);
                break;
            }

            counter += 1;
            yield return new WaitForSeconds(timeBtwnChars);


        }
    }
}

