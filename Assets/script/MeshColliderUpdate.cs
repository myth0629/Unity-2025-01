using UnityEngine;

public class SkinnedMeshColliderUpdater : MonoBehaviour
{
    [System.Serializable]
    public struct MeshData
    {
       public SkinnedMeshRenderer skinnedMeshRenderer;
        public MeshCollider meshCollider;
        [HideInInspector]public Mesh mesh;
    }
    public MeshData[] meshDatas = new MeshData[2];
   
    void Start()
    {
        for (int i = 0; i < meshDatas.Length; i++)
        {
            if(meshDatas[i].meshCollider == null)
            {
               meshDatas[i].meshCollider = gameObject.AddComponent<MeshCollider>();
            }
            meshDatas[i].mesh = new Mesh();
         }
      
    }

    void Update()
    {
        for (int i = 0; i < meshDatas.Length; i++)
        {
              if (meshDatas[i].skinnedMeshRenderer != null && meshDatas[i].meshCollider != null)
             {
                 // 스키닝된 메쉬의 현재 모양을 메쉬에 구움
                 meshDatas[i].skinnedMeshRenderer.BakeMesh(meshDatas[i].mesh);
               // 메쉬 컬라이더의 메쉬 업데이트
                 meshDatas[i].meshCollider.sharedMesh = meshDatas[i].mesh;
            }
        }
     }
}