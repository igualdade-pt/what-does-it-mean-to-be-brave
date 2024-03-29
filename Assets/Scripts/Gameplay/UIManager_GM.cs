﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager_GM : MonoBehaviour
{
    private GameplayManager gameplayManager;
    private RecordManager recordManager;
    private MusicManagerScript musicManager;
    private AudioManager audioManager;

    [Header("Buttons TEXT")]
    [Space]
    [SerializeField]
    private Sprite[] titleSprites;

    [SerializeField]
    private Image title;

    [Header("Characer Select")]
    [Space]
    [SerializeField]
    private GameObject[] charactersPool;

    [SerializeField]
    private RectTransform[] tCharacterPublicBath;

    [SerializeField]
    private RectTransform[] tCharacterSchool;

    private RectTransform[][] tCharacters = new RectTransform[2][];

    [SerializeField]
    private float[] xCharactersPublicBath;

    [SerializeField]
    private float[] xCharactersSchool;

    private float[][] xCharacters = new float[2][];

    [SerializeField]
    private LeanTweenType easeType;

    //private float previousTime = 0;

    [SerializeField]
    private AnimationCurve curve;

    [SerializeField]
    private float timeAnim = 0.8f;

    [Header("Text")]
    [Space]
    [SerializeField]
    private string[] textCharacterNamesForPublicBath_EN;

    [SerializeField]
    private string[] textCharacterNamesForPublicBath_IT;

    [SerializeField]
    private string[] textCharacterNamesForPublicBath_PT;

    [SerializeField]
    private string[] textCharacterNamesForPublicBath_ES;

    [SerializeField]
    private string[] textCharacterNamesForPublicBath_SE;

    [SerializeField]
    private string[] textCharacterNamesForSchool_EN;

    [SerializeField]
    private string[] textCharacterNamesForSchool_IT;

    [SerializeField]
    private string[] textCharacterNamesForSchool_PT;

    [SerializeField]
    private string[] textCharacterNamesForSchool_ES;

    [SerializeField]
    private string[] textCharacterNamesForSchool_SE;

    private string[][][] textCharacterNames = new string[2][][] { new string [5][], new string[5][] };

    [SerializeField]
    private Text textCharacterName;

    [SerializeField]
    private Text textPlay;

    [SerializeField]
    private Text textStopRecord;

    [SerializeField]
    private GameObject[] imagesSelectedCharacter;

    private int currentIndexCharacter;

    private bool canChange;
    private int indexGame;
    private List<int> charactersSelected = new List<int>(12);

    [Header("Properties")]
    [Space]
    [SerializeField]
    private GameObject panelLoading;

    [SerializeField]
    private GameObject panelSelectCharacterMenu;

    [SerializeField]
    private GameObject panelGameplay;

    [SerializeField]
    private GameObject panelStoppedRecordingMenu;

    [SerializeField]
    private GameObject buttonRecording;

    [SerializeField]
    private Animation buttonAnimation;

    /*[SerializeField]
    private Text title;*/

    [SerializeField]
    private Text textError;

    [SerializeField]
    private Text timerRecording;

    [SerializeField]
    private Text timerRecordingShadow;

    [SerializeField]
    private int menuMovieSceneIndex = 2;

    private int indexLanguage;

    private bool activeImage;
    private bool canDrag;
    private bool positiveDrag;
    private Vector2 lastDragPosition;
    private int sec;
    private bool isRecording;
    private int counterSec;
    private int counterMin;

    private void Start()
    {
        panelLoading.SetActive(false);
        panelSelectCharacterMenu.SetActive(true);
        panelGameplay.SetActive(false);
        panelStoppedRecordingMenu.SetActive(false);
        gameplayManager = FindObjectOfType<GameplayManager>().GetComponent<GameplayManager>();

        canChange = true;
        charactersSelected.Clear();
        textError.text = "";

        recordManager = FindObjectOfType<RecordManager>().GetComponent<RecordManager>();

        musicManager = FindObjectOfType<MusicManagerScript>().GetComponent<MusicManagerScript>();

        audioManager = FindObjectOfType<AudioManager>().GetComponent<AudioManager>();
    }

    public void UpdateSceneCharacter(int value)
    {
        indexGame = value;

        textCharacterNames[0][0] = textCharacterNamesForPublicBath_EN;
        textCharacterNames[0][1] = textCharacterNamesForPublicBath_IT;
        textCharacterNames[0][2] = textCharacterNamesForPublicBath_PT;
        textCharacterNames[0][3] = textCharacterNamesForPublicBath_ES;
        textCharacterNames[0][4] = textCharacterNamesForPublicBath_SE;

        textCharacterNames[1][0] = textCharacterNamesForSchool_EN;
        textCharacterNames[1][1] = textCharacterNamesForSchool_IT;
        textCharacterNames[1][2] = textCharacterNamesForSchool_PT;
        textCharacterNames[1][3] = textCharacterNamesForSchool_ES;
        textCharacterNames[1][4] = textCharacterNamesForSchool_SE;

        tCharacters[0] = tCharacterPublicBath;
        tCharacters[1] = tCharacterSchool;

        xCharacters[0] = xCharactersPublicBath;
        xCharacters[1] = xCharactersSchool;

        charactersPool[0].SetActive(false);
        charactersPool[1].SetActive(false);
        charactersPool[value].SetActive(true);

        InitUpdateCharacter(0);
    }


    /// <summary>
    /// Character Change
    /// </summary>
    /// <param name="index"> Index Of Current Character </param>
    public void InitUpdateCharacter(int index)
    {
        // Change Flag
        int t = index - Mathf.FloorToInt(xCharacters[indexGame].Length / 2);
        if (t < 0)
        {
            t += xCharacters[indexGame].Length;
        }

        int x = Mathf.CeilToInt(tCharacters[indexGame].Length / 2);

        if (indexGame == 1)
        {
            for (int i = 0; i < tCharacters[indexGame].Length; i++)
            {
                if (t > 0)
                {
                    t--;
                }
                else
                {
                    t = xCharacters[indexGame].Length - 1;
                }

                tCharacters[indexGame][i].anchoredPosition = new Vector2(xCharacters[indexGame][t], tCharacters[indexGame][i].anchoredPosition.y);

                if (t == x - 1)
                {
                    tCharacters[indexGame][i].sizeDelta = new Vector2(400, 600);
                }
                else
                {
                    tCharacters[indexGame][i].sizeDelta = new Vector2(250, 450);
                }
            }
        }
        else
        {
            currentIndexCharacter = 1;
            t = -1;
            index = 0;
            for (int i = 0; i < xCharacters[indexGame].Length; i++)
            {
                if (i != 0)
                {
                    t++;
                    tCharacters[indexGame][t].anchoredPosition = new Vector2(xCharacters[indexGame][i], tCharacters[indexGame][t].anchoredPosition.y);

                    if (t == 0)
                    {
                        tCharacters[indexGame][t].sizeDelta = new Vector2(400, 600);
                    }
                    else
                    {
                        tCharacters[indexGame][t].sizeDelta = new Vector2(250, 450);
                    }
                }
            }
        }


        // Change Name
        textCharacterName.text = textCharacterNames[indexGame][indexLanguage][index];
    }

    public void UpdateCharacter(int index, bool right)
    {
        // Change Character
        int t = index - Mathf.FloorToInt(xCharacters[indexGame].Length / 2);
        if (t < 0)
        {
            t += xCharacters[indexGame].Length;
        }

        int x = Mathf.CeilToInt(tCharacters[indexGame].Length / 2);

        if (indexGame == 1)
        {
            // Play Sound
            audioManager.PlayClip(1, 0.6f);
            // ****
            for (int i = 0; i < tCharacters[indexGame].Length; i++)
            {
                if (t > 0)
                {
                    t--;
                }
                else
                {
                    t = xCharacters[indexGame].Length - 1;
                }

                switch (right)
                {
                    case true:
                        if (t == x - 2 || t == x - 1 || t == x || t == x + 1)
                        {
                            if (easeType == LeanTweenType.animationCurve)
                            {
                                LeanTween.moveX(tCharacters[indexGame][i], xCharacters[indexGame][t], timeAnim).setEase(curve).setOnComplete(CanChangeCharacter); ;
                            }
                            else
                            {
                                LeanTween.moveX(tCharacters[indexGame][i], xCharacters[indexGame][t], timeAnim).setEase(easeType).setOnComplete(CanChangeCharacter); ;
                            }

                            if (t == x - 1)
                            {
                                if (easeType == LeanTweenType.animationCurve)
                                {
                                    LeanTween.size(tCharacters[indexGame][i], new Vector2(400, 600), timeAnim).setEase(curve);
                                }
                                else
                                {
                                    LeanTween.size(tCharacters[indexGame][i], new Vector2(400, 600), timeAnim).setEase(easeType);
                                }

                            }
                            else
                            {
                                if (easeType == LeanTweenType.animationCurve)
                                {
                                    LeanTween.size(tCharacters[indexGame][i], new Vector2(250, 450), timeAnim).setEase(curve);
                                }
                                else
                                {
                                    LeanTween.size(tCharacters[indexGame][i], new Vector2(250, 450), timeAnim).setEase(easeType);
                                }
                            }
                        }
                        else
                        {
                            LeanTween.cancel(tCharacters[indexGame][i]);
                            tCharacters[indexGame][i].anchoredPosition = new Vector2(xCharacters[indexGame][t], tCharacters[indexGame][i].anchoredPosition.y);
                        }
                        break;

                    case false:
                        if (t == x - 2 || t == x - 1 || t == x || t == x - 3)
                        {
                            if (easeType == LeanTweenType.animationCurve)
                            {
                                LeanTween.moveX(tCharacters[indexGame][i], xCharacters[indexGame][t], timeAnim).setEase(curve).setOnComplete(CanChangeCharacter);
                            }
                            else
                            {
                                LeanTween.moveX(tCharacters[indexGame][i], xCharacters[indexGame][t], timeAnim).setEase(easeType).setOnComplete(CanChangeCharacter);
                            }

                            if (t == x - 1)
                            {
                                if (easeType == LeanTweenType.animationCurve)
                                {
                                    LeanTween.size(tCharacters[indexGame][i], new Vector2(400, 600), timeAnim).setEase(curve);
                                }
                                else
                                {
                                    LeanTween.size(tCharacters[indexGame][i], new Vector2(400, 600), timeAnim).setEase(easeType);
                                }

                            }
                            else
                            {
                                if (easeType == LeanTweenType.animationCurve)
                                {
                                    LeanTween.size(tCharacters[indexGame][i], new Vector2(250, 450), timeAnim).setEase(curve);
                                }
                                else
                                {
                                    LeanTween.size(tCharacters[indexGame][i], new Vector2(250, 450), timeAnim).setEase(easeType);
                                }
                            }
                        }
                        else
                        {
                            LeanTween.cancel(tCharacters[indexGame][i]);
                            tCharacters[indexGame][i].anchoredPosition = new Vector2(xCharacters[indexGame][t], tCharacters[indexGame][i].anchoredPosition.y);
                        }
                        break;
                }

            }
        }
        else
        {
            t = -1;
            if (right)
            {
                index = 0;
            }
            else
            {
                index = 1;
            }

            for (int i = 0; i < xCharacters[indexGame].Length; i++)
            {
                switch (right)
                {
                    case true:
                        if (i != 0)
                        {
                            t++;
                            if (easeType == LeanTweenType.animationCurve)
                            {
                                LeanTween.moveX(tCharacters[indexGame][t], xCharacters[indexGame][i], timeAnim).setEase(curve).setOnComplete(CanChangeCharacter); ;
                            }
                            else
                            {
                                LeanTween.moveX(tCharacters[indexGame][t], xCharacters[indexGame][i], timeAnim).setEase(easeType).setOnComplete(CanChangeCharacter); ;
                            }

                            if (t == 0)
                            {
                                if (easeType == LeanTweenType.animationCurve)
                                {
                                    LeanTween.size(tCharacters[indexGame][t], new Vector2(400, 600), timeAnim).setEase(curve);
                                }
                                else
                                {
                                    LeanTween.size(tCharacters[indexGame][t], new Vector2(400, 600), timeAnim).setEase(easeType);
                                }

                            }
                            else
                            {
                                if (easeType == LeanTweenType.animationCurve)
                                {
                                    LeanTween.size(tCharacters[indexGame][t], new Vector2(250, 450), timeAnim).setEase(curve);
                                }
                                else
                                {
                                    LeanTween.size(tCharacters[indexGame][t], new Vector2(250, 450), timeAnim).setEase(easeType);
                                }
                            }
                        }
                        break;

                    case false:
                        if (i != 2)
                        {
                            t++;
                            if (easeType == LeanTweenType.animationCurve)
                            {
                                LeanTween.moveX(tCharacters[indexGame][t], xCharacters[indexGame][i], timeAnim).setEase(curve).setOnComplete(CanChangeCharacter);
                            }
                            else
                            {
                                LeanTween.moveX(tCharacters[indexGame][t], xCharacters[indexGame][i], timeAnim).setEase(easeType).setOnComplete(CanChangeCharacter);
                            }

                            if (t == 1)
                            {
                                if (easeType == LeanTweenType.animationCurve)
                                {
                                    LeanTween.size(tCharacters[indexGame][t], new Vector2(400, 600), timeAnim).setEase(curve);
                                }
                                else
                                {
                                    LeanTween.size(tCharacters[indexGame][t], new Vector2(400, 600), timeAnim).setEase(easeType);
                                }

                            }
                            else
                            {
                                if (easeType == LeanTweenType.animationCurve)
                                {
                                    LeanTween.size(tCharacters[indexGame][t], new Vector2(250, 450), timeAnim).setEase(curve);
                                }
                                else
                                {
                                    LeanTween.size(tCharacters[indexGame][t], new Vector2(250, 450), timeAnim).setEase(easeType);
                                }
                            }
                        }
                        break;
                }

            }
        }


        // Change Name
        textCharacterName.text = textCharacterNames[indexGame][indexLanguage][index];
    }

    private void CanChangeCharacter()
    {
        canChange = true;
    }

    public void _RightButtonClick()
    {
        if (canChange)
        {
            if (indexGame == 1)
            {
                canChange = false;
                if (currentIndexCharacter < tCharacters[indexGame].Length - 1)
                {
                    currentIndexCharacter++;
                }
                else
                {
                    currentIndexCharacter = 0;
                }

                Debug.Log("Current Index Character: " + currentIndexCharacter);
                UpdateCharacter(currentIndexCharacter, true);

                //languageMenuManager.ChangeLanguageIndex = currentIndexCharacter;
            }
            else
            {
                canChange = false;
                if (currentIndexCharacter < tCharacters[indexGame].Length - 1)
                {
                    currentIndexCharacter++;
                    // Play Sound
                    audioManager.PlayClip(1, 0.6f);
                    // ****
                }
                else
                {
                    currentIndexCharacter = tCharacters[indexGame].Length - 1;
                }

                if (currentIndexCharacter <= tCharacters[indexGame].Length - 1)
                {
                    UpdateCharacter(currentIndexCharacter, true);
                }


                Debug.Log("Current 'Public Bath' Index Character: " + currentIndexCharacter);
            }


        }
    }

    public void _LeftButtonClick()
    {
        if (canChange)
        {
            if (indexGame == 1)
            {
                canChange = false;
                if (currentIndexCharacter > 0)
                {
                    currentIndexCharacter--;
                }
                else
                {
                    currentIndexCharacter = tCharacters[indexGame].Length - 1;
                }

                Debug.Log("Current Index Character: " + currentIndexCharacter);
                UpdateCharacter(currentIndexCharacter, false);

                //languageMenuManager.ChangeLanguageIndex = currentIndexCharacter;
            }
            else
            {
                canChange = false;
                if (currentIndexCharacter > 0)
                {
                    currentIndexCharacter--;
                    // Play Sound
                    audioManager.PlayClip(1, 0.6f);
                    // ****
                }
                else
                {
                    currentIndexCharacter = 0;
                }

                if (currentIndexCharacter >= 0)
                {
                    UpdateCharacter(currentIndexCharacter, false);
                }

                Debug.Log("Current 'Public Bath' Index Character: " + currentIndexCharacter);
                

                //languageMenuManager.ChangeLanguageIndex = currentIndexCharacter;
            }
        }
    }


    public void _BeginDrag()
    {
        lastDragPosition = Input.mousePosition;
        //lastDragPosition = Input.GetTouch(0).position;
    }

    public void _Drag()
    {
        canDrag = false;

        if (Input.mousePosition.x != lastDragPosition.x)
        {
            canDrag = true;
            positiveDrag = Input.mousePosition.x > lastDragPosition.x;
        }


        if (canDrag)
        {
            if (indexGame == 1)
            {
                if (positiveDrag)
                {
                    if (canChange)
                    {
                        canChange = false;
                        if (currentIndexCharacter < tCharacters[indexGame].Length - 1)
                        {
                            currentIndexCharacter++;

                        }
                        else
                        {
                            currentIndexCharacter = 0;
                        }

                        Debug.Log("Current Index Character: " + currentIndexCharacter);
                        UpdateCharacter(currentIndexCharacter, true);

                        //languageMenuManager.ChangeLanguageIndex = currentIndexCharacter;
                    }
                }
                else
                {
                    if (canChange)
                    {
                        canChange = false;
                        if (currentIndexCharacter > 0)
                        {
                            currentIndexCharacter--;
                        }
                        else
                        {
                            currentIndexCharacter = tCharacters[indexGame].Length - 1;
                        }

                        Debug.Log("Current Index Character: " + currentIndexCharacter);
                        UpdateCharacter(currentIndexCharacter, false);

                        //languageMenuManager.ChangeLanguageIndex = currentIndexCharacter;
                    }

                }

            }
            else
            {
                if (positiveDrag)
                {
                    if (canChange)
                    {
                        canChange = false;
                        if (currentIndexCharacter < tCharacters[indexGame].Length - 1)
                        {
                            currentIndexCharacter++;
                            // Play Sound
                            audioManager.PlayClip(1, 0.6f);
                            // ****
                        }
                        else
                        {
                            currentIndexCharacter = tCharacters[indexGame].Length - 1;
                        }

                        if (currentIndexCharacter <= tCharacters[indexGame].Length - 1)
                        {
                            UpdateCharacter(currentIndexCharacter, true);
                        }

                        Debug.Log("Current Index Character: " + currentIndexCharacter);
                        

                        //languageMenuManager.ChangeLanguageIndex = currentIndexCharacter;
                    }
                }
                else
                {
                    if (canChange)
                    {
                        canChange = false;
                        if (currentIndexCharacter > 0)
                        {
                            currentIndexCharacter--;
                            // Play Sound
                            audioManager.PlayClip(1, 0.6f);
                            // ****
                        }
                        else
                        {
                            currentIndexCharacter = 0;
                        }

                        if (currentIndexCharacter >= 0)
                        {
                            UpdateCharacter(currentIndexCharacter, false);
                        }

                        Debug.Log("Current Index Character: " + currentIndexCharacter);
                        

                        //languageMenuManager.ChangeLanguageIndex = currentIndexCharacter;
                    }

                }

            }
        }


        lastDragPosition = Input.mousePosition;
        //lastDragPosition = Input.GetTouch(0).position;
    }

    public void _EndDrag()
    {
        canDrag = true;
    }


    public void _SelectedCharacterClick(int index)
    {
        Debug.Log(" Index: " + index);
        textError.text = "";
        if (charactersSelected.Count <= 2)
        {
            activeImage = true;
            switch (charactersSelected.Contains(index))
            {
                case true:
                    // Play Sound
                    audioManager.PlayClip(0, 0.6f);
                    // ****
                    charactersSelected.Remove(index);
                    break;

                case false:
                    if (charactersSelected.Count < 2)
                    {
                        // Play Sound
                        audioManager.PlayClip(0, 0.6f);
                        // ****
                        charactersSelected.Add(index);
                    }
                    else
                    {
                        // Play Sound
                        audioManager.PlayClip(5, 0.6f);
                        // ****
                        activeImage = false;
                        switch (indexLanguage)
                        {
                            case 0:
                                // English
                                textError.text = "---- Reached The Maximum Number Of Characters! ----";
                                break;

                            case 1:
                                // Italian
                                textError.text = "---- ¡Raggiunto Il Numero Massimo Di Personaggios! ----";
                                break;

                            case 2:
                                // Portuguese
                                textError.text = "---- Atingiste O Número Máximo De Personagens! ----";
                                break;

                            case 3:
                                // Spanish
                                textError.text = "---- Alcanzó El Número Máximo De Personajes! ----";
                                break;

                            case 4:
                                // Swedish
                                textError.text = "---- Nått Maximalt Antal Tecken! ----";
                                break;

                            default:
                                // English
                                textError.text = "---- Reached The Maximum Number Of Characters! ----";
                                break;
                        }
                    }
                    break;
            }
        }
    }

    public void _SelectedCharacterClick(GameObject image)
    {
        if (activeImage)
        {
            switch (image.activeSelf)
            {
                case true:
                    image.SetActive(false);
                    break;

                case false:
                    image.SetActive(true); 
                    break;
            }
        }
    }

    public void _PlayButtonClick()
    {
        if (charactersSelected.Count != 0)
        {
            // Play Sound
            audioManager.PlayClip(0, 0.6f);
            // ****
            musicManager.StopMusic();
            panelSelectCharacterMenu.SetActive(false);
            panelGameplay.SetActive(true);
            charactersSelected.TrimExcess();

            gameplayManager.StartGameplay(charactersSelected);

            charactersSelected.Clear();
            for (int i = 0; i < imagesSelectedCharacter.Length; i++)
            {
                imagesSelectedCharacter[i].SetActive(false);
            }
        }
        else
        {
            // Play Sound
            audioManager.PlayClip(5, 0.6f);
            // ****
            switch (indexLanguage)
            {
                case 0:
                    // English
                    textError.text = "---- Select a Character! ----";
                    break;

                case 1:
                    // Italian
                    textError.text = "---- Seleziona Un Personaggio! ----";
                    break;

                case 2:
                    // Portuguese
                    textError.text = "---- Seleciona Uma Personagem! ----";
                    break;

                case 3:
                    // Spanish
                    textError.text = "---- Seleccionar Un Personaje! ----";
                    break;

                case 4:
                    // Swedish
                    textError.text = "---- Välj En Karaktär! ----";
                    break;

                default:
                    // English
                    textError.text = "---- Select a Character! ----";
                    break;
            }
        }
    }

    public void UpdateLanguage(int index)
    {
        indexLanguage = index;

        title.sprite = titleSprites[index];

       

        switch (index)
        {
            case 0:
                // English
                textPlay.text = "Play";
                textStopRecord.text = "STOP RECORDING?";

                for (int i = 0; i < imagesSelectedCharacter.Length; i++)
                {
                    imagesSelectedCharacter[i].GetComponent<Text>().text = "Player Selected";
                }
                break;

            case 1:
                // Italian
                textPlay.text = "Giocare";
                textStopRecord.text = "INTERROMPI REGISTRAZIONE?";

                for (int i = 0; i < imagesSelectedCharacter.Length; i++)
                {
                    imagesSelectedCharacter[i].GetComponent<Text>().text = "Giocatore Selezionato";
                }
                break;

            case 2:
                // Portuguese
                textPlay.text = "Jogar";
                textStopRecord.text = "PARAR DE GRAVAR?";

                for (int i = 0; i < imagesSelectedCharacter.Length; i++)
                {
                    imagesSelectedCharacter[i].GetComponent<Text>().text = "Personagem Selecionada";
                    Debug.Log("HELLO:    " + imagesSelectedCharacter[i].GetComponent<Text>().text);
                }                
                break;

            case 3:
                // Spanish
                textPlay.text = "Jugar";
                textStopRecord.text = "PARA DE GRABAR?";

                for (int i = 0; i < imagesSelectedCharacter.Length; i++)
                {
                    imagesSelectedCharacter[i].GetComponent<Text>().text = "Jugador Seleccionado";
                }
                break;

            case 4:
                // Swedish
                textPlay.text = "Spela";
                textStopRecord.text = "STOPPA INSPELNINGEN?";

                for (int i = 0; i < imagesSelectedCharacter.Length; i++)
                {
                    imagesSelectedCharacter[i].GetComponent<Text>().text = "Spelare Vald";
                }
                break;

            default:
                // English
                textPlay.text = "Play";
                textStopRecord.text = "STOP RECORDING?";

                for (int i = 0; i < imagesSelectedCharacter.Length; i++)
                {
                    imagesSelectedCharacter[i].GetComponent<Text>().text = "Player Selected";
                }
                break;
        }

    }


    public void _RecordButtonClicked()
    {
        // Play Sound
        audioManager.PlayClip(0, 0.6f);
        // ****
        panelStoppedRecordingMenu.SetActive(recordManager.IsRecording);


        if (recordManager.IsRecording)
        {
            buttonAnimation[buttonAnimation.clip.name].time = 0;
            buttonAnimation[buttonAnimation.clip.name].speed = 0;
            isRecording = false;
            StopAllCoroutines();
        }
        else
        {            
            buttonAnimation[buttonAnimation.clip.name].speed = 1;
            if (!buttonAnimation.isPlaying)
            {
                buttonAnimation.Play();
            }
            isRecording = true;
            counterSec = 0;
            counterMin = 0;
            StartCoroutine(RecordTimer());
        }

        recordManager.StartRecording();

    }

    private IEnumerator RecordTimer()
    {

        while (isRecording)
        {
            yield return new WaitForSeconds(1);
            counterSec++;

            if (counterSec > 59)
            {
                counterSec = 0;
                counterMin++;
            }

            timerRecording.text = counterMin.ToString("0") + ":" + counterSec.ToString("00");


            timerRecordingShadow.text = timerRecording.text;
        }

    }

    public void _UnPauseButtonClicked()
    {
        // Play Sound
        audioManager.PlayClip(0, 0.6f);
        // ****
        buttonAnimation[buttonAnimation.clip.name].speed = 1;
        panelStoppedRecordingMenu.SetActive(recordManager.IsRecording);
        recordManager.UnPauseRecording();
        isRecording = true;
        StartCoroutine(RecordTimer());
    }

    public void _SaveRecordButtonClicked()
    {
        panelLoading.SetActive(true);
        // Play Sound
        audioManager.PlayClip(0, 0.6f);
        // ****
        recordManager.StopRecording();
    }

    public void _ReturnButtonClicked(int indexScene)
    {
        // Play Sound
        audioManager.PlayClip(0, 0.6f);
        // ****
        panelLoading.SetActive(true);
        musicManager.PlayMusicMenu();
        gameplayManager.LoadAsyncGamePlay(indexScene, false);
    }

    public void _ReturnToCharactersButtonClicked()
    {
        if (!recordManager.IsRecording)
        {
            // Play Sound
            audioManager.PlayClip(0, 0.6f);
            // ****
            musicManager.ResumeMusic();
            panelStoppedRecordingMenu.SetActive(false);
            panelGameplay.SetActive(false);
            panelSelectCharacterMenu.SetActive(true);
        }
    }

    public void _RecordAgainButtonClicked()
    {
        // Play Sound
        audioManager.PlayClip(0, 0.6f);
        // ****
        panelStoppedRecordingMenu.SetActive(false);
    }


    public void MovieSaved()
    {
        // Play Sound
        audioManager.PlayClip(2, 0.6f);
        // ****
        musicManager.PlayMusicMenu();
        musicManager.ResumeMusic();
        gameplayManager.LoadAsyncGamePlay(menuMovieSceneIndex, true);
        //gameplayManager.LoadScene(menuMovieSceneIndex, true);
    }
}
