using UnityEngine;

public class TriggerSubtitle : MonoBehaviour
{
    public SubtitleManager subtitleManager; // 자막 매니저 스크립트 연결
    public SubtitleManager.SubtitleData[] subtitles; // 이 트리거에서 재생할 자막 데이터
    public bool isPlayed = false; // 자막이 재생되었는지 여부

    private void Start()
    {
        if (subtitleManager == null)
        {
             subtitleManager = FindObjectOfType<SubtitleManager>();
           if (subtitleManager == null)
            {
               Debug.LogError("SubtitleManager를 찾을 수 없습니다. Hierarchy 창에서 SubtitleManager 스크립트가 추가된 오브젝트를 할당해주세요.");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // 트리거 영역에 진입한 오브젝트의 태그가 "Player"인지 확인합니다.
        if (other.CompareTag("Player") && !isPlayed)
        {
             // 자막 매니저에 자막 데이터 설정
            subtitleManager.subtitles = new System.Collections.Generic.List<SubtitleManager.SubtitleData>(subtitles);
            subtitleManager.PlaySubtitles(); // 자막 재생
            isPlayed = true;
            Debug.Log("자막 재생 시작!");
        }
    }
}