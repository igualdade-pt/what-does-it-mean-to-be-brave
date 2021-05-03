﻿using NatSuite.Sharing;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class UIManager_MM : MonoBehaviour
{
    private MainMenuManager mainMenuManager;

    [SerializeField]
    private GameObject panelMoviesMenu;

    [SerializeField]
    private Text text;

    [SerializeField]
    private GameObject panelMainMenu;

    [SerializeField]
    private GameObject panelDeleteMenu;

    [SerializeField]
    private GameObject ReturnButtonFromMovie;

    [Space]
    [SerializeField]
    private Button videoButton;

    [SerializeField]
    private GameObject parentVideoButtonTransform;

    [SerializeField]
    private LeanTweenType easeType;

    [SerializeField]
    private AnimationCurve curve;

    [SerializeField]
    private int timerVideoButton = 1;


    // Buttons VideoPlayer
    [Header("Buttons VideoPlayer")]
    [Space]
    [SerializeField]
    private GameObject playButton;

    [SerializeField]
    private GameObject pauseButton;

    [SerializeField]
    private GameObject resumeButton;

    [SerializeField]
    private GameObject replayButton;

    [SerializeField]
    private GameObject videoPlay;

    [SerializeField]
    private GameObject videoPlayer_BG;


    [Space]
    [SerializeField]
    private Button yesButton;

    [Header("Video Clips Test")]
    [Space]
    [SerializeField]
    private VideoClip[] videos;

    private bool canPlayVideo = false;


    private void Awake()
    {
        UpdateVideosList();

        panelMainMenu.SetActive(true);
        panelMoviesMenu.SetActive(false);
        panelDeleteMenu.SetActive(false);
        videoPlayer_BG.SetActive(false);
        ReturnButtonFromMovie.SetActive(false);
    }


    private void Start()
    {
        mainMenuManager = FindObjectOfType<MainMenuManager>().GetComponent<MainMenuManager>();
    }


    private void UpdateVideosList()
    {
        if (parentVideoButtonTransform.transform.childCount != 0)
        {
            for (int i = 0; i < parentVideoButtonTransform.transform.childCount; i++)
            {
                var transform = parentVideoButtonTransform.transform.GetChild(i);
                GameObject.Destroy(transform.gameObject);
            }
        }


        if (Directory.Exists(Application.persistentDataPath))
        {
            var videosPath = Directory.GetFiles(Application.persistentDataPath);

            if (videosPath.Length == 0)
                return;

            for (int y = 0; y < videosPath.Length; y++)
            {
                if (File.Exists(videosPath[y]))
                {
                    var button = Instantiate(videoButton, Vector3.zero, Quaternion.identity) as Button;
                    var rectTransform = button.GetComponent<RectTransform>();
                    rectTransform.SetParent(parentVideoButtonTransform.transform, false);

                    button.onClick.AddListener(() => _VideoButtonClicked(button));

                    var videoScript = button.GetComponentInChildren<VideoButton>();


                    //Android
                    /*video.source = VideoSource.Url;
                    video.url = videosPath[y];*/
                    videoScript.RenderImageByUrl(videosPath[y]);


                    // TEST
                    //videoScript.RenderImageByVideoClip(videos[y]);

                    var buttons = button.GetComponentsInChildren<Button>(true);
                    for (int i = 0; i < buttons.Length; i++)
                    {
                        if (buttons[i].gameObject.tag == "ButtonDelete")
                        {
                            buttons[i].onClick.AddListener(() => _DeleteVideoButtonClicked(button));
                        }
                        else if (buttons[i].gameObject.tag == "ButtonShare")
                        {
                            buttons[i].onClick.AddListener(() => _ShareVideoButtonClicked(button));
                        }

                    }
                }
            }
        }
    }


    public void _InformationButtonClicked()
    {

    }

    public void _LanguageButtonClicked(int indexScene)
    {
        Debug.Log("Language Clicked, Index Scene: " + indexScene);

        mainMenuManager.LoadScene(indexScene);
    }

    public void _AgeButtonClicked(int indexScene)
    {
        Debug.Log("Age Clicked, Index Scene: " + indexScene);

        mainMenuManager.LoadScene(indexScene);
    }

    public void _BooksButtonClicked()
    {

    }

    public void _SoundButtonClicked()
    {

    }

    public void _SceneButtonClicked(int indexScene)
    {
        mainMenuManager.LoadAsyncGamePlay(indexScene);
    }

    public void _MoviesButtonClicked()
    {
        panelMainMenu.SetActive(false);
        panelMoviesMenu.SetActive(true);
    }

    public void _CloseMoviesButtonClicked()
    {
        if (!videoPlayer_BG.activeSelf)
        {
            panelMainMenu.SetActive(true);
            panelMoviesMenu.SetActive(false);
        }
    }

    public void UpdateLanguage(int indexLanguage)
    {

    }

    public void _ReturnVideoButtonClicked()
    {
        if (videoPlayer_BG.activeSelf)
        {
            videoPlay.GetComponent<VideoPlayer>().Stop();
            videoPlayer_BG.SetActive(false);
            playButton.SetActive(true);
            canPlayVideo = false;
            ReturnButtonFromMovie.SetActive(false);
        }
    }

    private void _DeleteVideoButtonClicked(Button button)
    {
        panelDeleteMenu.SetActive(true);
        yesButton.onClick.AddListener(() => _YesVideoButtonClicked(button));
    }

    private void _YesVideoButtonClicked(Button button)
    {
        var url = button.GetComponentInChildren<VideoPlayer>().url;

        if (File.Exists(url))
        {
            File.Delete(url);
            GameObject.Destroy(button.gameObject);
            //UpdateVideosList();
        }

        //GameObject.Destroy(button.gameObject);
        yesButton.onClick.RemoveAllListeners();
        panelDeleteMenu.SetActive(false);
    }

    public void _NoVideoButtonClicked()
    {
        panelDeleteMenu.SetActive(false);
        yesButton.onClick.RemoveAllListeners();
    }

    private void _ShareVideoButtonClicked(Button button)
    {
        var url = button.GetComponentInChildren<VideoPlayer>().url;

        if (File.Exists(url))
        {
            var payload = new SharePayload();
            payload.AddMedia(url);
            payload.Commit();
        }
    }

    /// <summary>
    /// VideoPlayer Play/Pause/Resume/Stop Video
    /// </summary>
    private void _VideoButtonClicked(Button button)
    {
        if (!File.Exists(button.GetComponentInChildren<VideoPlayer>().url))
            return;

        var rectTransform = button.GetComponent<RectTransform>();

        var videoToPlay = videoPlay.GetComponent<VideoPlayer>();
        videoToPlay.targetTexture.Release();


        // Android
        videoToPlay.source = VideoSource.Url;
        videoToPlay.url = button.GetComponentInChildren<VideoPlayer>().url;

        //TEST
        /*videoToPlay.source = VideoSource.VideoClip;
        videoToPlay.clip = button.GetComponentInChildren<VideoPlayer>().clip;*/


        videoPlayer_BG.SetActive(true);

        var videoRectTransform = videoPlayer_BG.GetComponent<RectTransform>();

        videoRectTransform.position = rectTransform.position;
        videoRectTransform.sizeDelta = new Vector2(300, 200);

        // Position and Scale Animation
        if (easeType == LeanTweenType.animationCurve)
        {
            LeanTween.move(videoRectTransform, Vector3.zero, timerVideoButton).setEase(curve).setOnComplete(PlayButtonReady);
            LeanTween.size(videoRectTransform, new Vector2(1600, 900), timerVideoButton).setEase(curve);
        }
        else
        {
            LeanTween.move(videoRectTransform, Vector3.zero, timerVideoButton).setEase(easeType).setOnComplete(PlayButtonReady);
            LeanTween.size(videoRectTransform, new Vector2(1600, 900), timerVideoButton).setEase(easeType);
        }

        playButton.SetActive(true);
        pauseButton.SetActive(false);
        resumeButton.SetActive(false);
        replayButton.SetActive(false);
    }

    private void PlayButtonReady()
    {
        canPlayVideo = true;
        ReturnButtonFromMovie.SetActive(true);
    }


    // Play Button
    public void _PlayButtonClicked(VideoPlayer videoToPlay)
    {
        if (canPlayVideo)
        {
            playButton.SetActive(false);
            pauseButton.SetActive(true);
            resumeButton.SetActive(false);
            replayButton.SetActive(false);

            videoToPlay.Prepare();

            videoToPlay.prepareCompleted += PlayVideo;

            videoToPlay.loopPointReached += VideoStopped;
        }
    }

    // Video Stopped
    private void VideoStopped(VideoPlayer videoToPlay)
    {
        Debug.Log("Parou");
        if (canPlayVideo)
        {
            replayButton.SetActive(true);
            playButton.SetActive(false);
            pauseButton.SetActive(false);
            resumeButton.SetActive(false);
            videoToPlay.Stop();
        }

    }

    // Start Video after prepared to play
    private void PlayVideo(VideoPlayer videoToPlay)
    {
        if (videoToPlay.isPrepared)
        {
            videoToPlay.Play();

            videoToPlay.errorReceived += Video_errorReceived;
        }
    }

    // Pause Button
    public void _PauseButtonClicked(VideoPlayer videoToPlay)
    {
        Debug.Log("Pausa");
        if (canPlayVideo & videoToPlay.isPlaying)
        {
            pauseButton.SetActive(false);
            resumeButton.SetActive(true);
            videoToPlay.Pause();
        }
    }

    // Resume Button
    public void _ResumeButtonClicked(VideoPlayer videoToPlay)
    {
        Debug.Log("Resume");
        if (canPlayVideo & videoToPlay.isPaused)
        {
            pauseButton.SetActive(true);
            resumeButton.SetActive(false);
            videoToPlay.Play();
        }
    }

    public void _ReplayButtonClicked(VideoPlayer videoToPlay)
    {
        Debug.Log("Replay");
        if (canPlayVideo)
        {
            playButton.SetActive(false);
            pauseButton.SetActive(true);
            resumeButton.SetActive(false);
            replayButton.SetActive(false);
            videoToPlay.Prepare();

            videoToPlay.prepareCompleted += PlayVideo;

            videoToPlay.loopPointReached += VideoStopped;
        }
    }


    private void Video_errorReceived(VideoPlayer source, string message)
    {
        text.text = message;
    }
}