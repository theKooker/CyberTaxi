using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialAnimController : MonoBehaviour
{
    [SerializeField] private TextMesh Header;
    [SerializeField] private string[] HeaderTexts;
    [SerializeField] private Animator TutorialTextTap;
    [SerializeField] private Animator TutorialTextHold;
    [SerializeField] private GameObject Lanes;

    private int _headerIndex = 0;
    private AudioSource letterSound;

    private void Awake()
    {
        letterSound = GetComponent<AudioSource>();
    }


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ShowTutorialText());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator ShowTutorialText()
    {
        foreach (var letter in HeaderTexts[_headerIndex])
        {
            Header.text += letter;
            letterSound.Play();
            yield return new WaitForSeconds(.05f);
        }
        yield return new WaitForSeconds(4f);
        TutorialTextTap.SetTrigger("Tap");
        yield return new WaitForSeconds(5f);
        _headerIndex++;
        Header.text = "";
        foreach (var letter in HeaderTexts[_headerIndex])
        {
            Header.text += letter;
            letterSound.Play();
            yield return new WaitForSeconds(.05f);
        }
        yield return new WaitForSeconds(6f);
        TutorialTextTap.SetTrigger("Vanish");
        yield return new WaitForSeconds(3f);
        _headerIndex++;
        Header.text = "";
        foreach (var letter in HeaderTexts[_headerIndex])
        {
            Header.text += letter;
            letterSound.Play();
            yield return new WaitForSeconds(.05f);
        }
        _headerIndex++;
        Header.text += "\n";
        foreach (var letter in HeaderTexts[_headerIndex])
        {
            Header.text += letter;
            letterSound.Play();
            yield return new WaitForSeconds(.05f);
        }
        TutorialTextHold.SetTrigger("Hold");
        yield return new WaitForSeconds(2f);
        TutorialTextTap.SetTrigger("HoldDescription");
        yield return new WaitForSeconds(5f);
        _headerIndex++;
        Header.text = "";
        foreach (var letter in HeaderTexts[_headerIndex])
        {
            Header.text += letter;
            letterSound.Play();
            yield return new WaitForSeconds(.05f);
        }
        TutorialTextTap.SetTrigger("VanishCTap");
        TutorialTextHold.SetTrigger("VanishCHold");
        yield return new WaitForSeconds(4f);
        _headerIndex++;
        Header.text = "";
        foreach (var letter in HeaderTexts[_headerIndex])
        {
            Header.text += letter;
            letterSound.Play();
            yield return new WaitForSeconds(.05f);
        }
        Lanes.SetActive(true);
        yield return new WaitForSeconds(2f);
        TutorialTextTap.SetTrigger("TapOnLane");
        _headerIndex++;
        Header.text = "";
        foreach (var letter in HeaderTexts[_headerIndex])
        {
            Header.text += letter;
            letterSound.Play();
            yield return new WaitForSeconds(.05f);
        }
        yield return new WaitForSeconds(2f);
        TutorialTextTap.SetTrigger("TapPressed");
        yield return new WaitForSeconds(2f);
        _headerIndex++;
        Header.text = "";
        foreach (var letter in HeaderTexts[_headerIndex])
        {
            Header.text += letter;
            letterSound.Play();
            yield return new WaitForSeconds(.05f);
        }
        yield return new WaitForSeconds(4f);
        _headerIndex++;
        Header.text = "";
        foreach (var letter in HeaderTexts[_headerIndex])
        {
            Header.text += letter;
            letterSound.Play();
            yield return new WaitForSeconds(.05f);
        }
        _headerIndex++;
        foreach (var letter in HeaderTexts[_headerIndex])
        {
            Header.text += letter;
            letterSound.Play();
            yield return new WaitForSeconds(1f);
        }

        SceneManager.LoadScene("MainMenu");
    }
}
