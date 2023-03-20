using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoUrl : MonoBehaviour
{
    VideoPlayer videoPlayer;

    [SerializeField]
    private string videoFileName;

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);
        Debug.LogError(videoPlayer.url);
        videoPlayer.Play();
    }

}
