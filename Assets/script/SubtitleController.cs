using UnityEngine;
using UnityEngine.UI; // Text 사용 시 필요
using TMPro; // TextMeshPro 사용 시 필요
using System.Collections;
using System.Collections.Generic;

public class SubtitleManager : MonoBehaviour
{
    public TextMeshProUGUI subtitleText;

    // 자막 데이터 구조체
    [System.Serializable]
    public struct SubtitleData
    {
        public float startTime; // 시작 시간
        public float endTime; // 종료 시간
        [TextArea]
        public string text; // 자막 텍스트
    }

    public List<SubtitleData> subtitles = new List<SubtitleData>(); // 자막 데이터 리스트
    private int currentSubtitleIndex = 0; // 현재 자막 인덱스
    private float timer = 0f; // 타이머

    void Update()
    {
        if (currentSubtitleIndex < subtitles.Count)
        {
            timer += Time.deltaTime;
            if (timer >= subtitles[currentSubtitleIndex].startTime)
            {
                subtitleText.text = subtitles[currentSubtitleIndex].text;

                if (timer >= subtitles[currentSubtitleIndex].endTime)
                {
                    subtitleText.text = "";
                    currentSubtitleIndex++;
                }
            }
        }
    }

    // 자막 재생 시작 함수
     public void PlaySubtitles()
    {
         timer = 0f;
         currentSubtitleIndex = 0;

         StartCoroutine(ShowSubtitles());
    }

    IEnumerator ShowSubtitles()
    {
         while(currentSubtitleIndex < subtitles.Count)
        {
           timer += Time.deltaTime;

            if (timer >= subtitles[currentSubtitleIndex].startTime)
            {
                subtitleText.text = subtitles[currentSubtitleIndex].text;
                
                if (timer >= subtitles[currentSubtitleIndex].endTime)
                 {
                    subtitleText.text = "";
                    currentSubtitleIndex++;
                }
            }

             yield return null;
        }

    }
}