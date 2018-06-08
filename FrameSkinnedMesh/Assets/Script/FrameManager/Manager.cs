using System.Collections.Generic;
using Neatly.Timer;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public Material material;
    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;

    public static Manager Instance { get; private set; }

    private const int MaxCount = 100;
    private Material[] shareMaterial;
    private MeshSequence[] meshSeqList;
    private List<MeshActor> actorList = new List<MeshActor>();
    private Dictionary<int, CombineInstance[]> combineMap;
    private Matrix4x4 matrix;
    private Mesh CurrentMesh;

    public void Awake()
    {
        Instance = this;
        NeatlyTimer.Init();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshSeqList = AnimatorSequence.LoadMeshSequences("SK_Blue_CasterMinion_000");
        //        meshSeqList = AnimatorSequence.LoadMeshSequences("Footman_Blue");

        shareMaterial = new[] { material };
        meshRenderer.sharedMaterials = shareMaterial;
        combineMap = new Dictionary<int, CombineInstance[]>();
    }

    public MeshActor GetActor()
    {
        var actor = new MeshActor();
        actor.Init(meshSeqList);
        actorList.Add(actor);
        return actor;
    }

    void LateUpdate()
    {
        matrix = transform.worldToLocalMatrix;
        var combine = GetCombineInstanceArray(actorList.Count);
        for (int i = 0; i < actorList.Count; i++)
        {
            combine[i].mesh = actorList[i].mesh;
            Matrix4x4 translateMatrix = Matrix4x4.Translate(actorList[i].position);
            Matrix4x4 rotateMatrix = Matrix4x4.Rotate(Quaternion.Euler(actorList[i].rotate))*Matrix4x4.Rotate(Quaternion.Euler(new Vector3(-90, 0, 0))); //
            Matrix4x4 scaleMatrix = Matrix4x4.Scale(actorList[i].scale);
            combine[i].transform = matrix * (translateMatrix * rotateMatrix * scaleMatrix);
        }

        if (CurrentMesh == null)
        {
            CurrentMesh = new Mesh();
        }
        else
        {
            CurrentMesh.Clear();
        }
        CurrentMesh.CombineMeshes(combine, true);
        meshFilter.mesh = CurrentMesh;
    }

    public CombineInstance[] GetCombineInstanceArray(int count)
    {
        CombineInstance[] combineArray;
        if (combineMap.TryGetValue(count, out combineArray))
        {
            return combineArray;
        }
        combineArray = new CombineInstance[count];
        combineMap.Add(count, combineArray);
        return combineArray;
    }
}