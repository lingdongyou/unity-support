using System.IO;
using UnityEngine;

public class AnimatorSequence
{
    public static MeshSequence[] LoadMeshSequences(string name)
    {
        byte[] bytes = LoadFile(name);
        ByteReader br = new ByteReader(bytes);
        int verCount = br.ReadInt();
        int triCount = br.ReadInt();
        int uvCount = br.ReadInt();
        int[] tris = new int[triCount];
        Vector2[] uvs = new Vector2[uvCount];
        for (int i = 0; i < triCount; i++)
        {
            tris[i] = br.ReadShort();
        }
        for (int i = 0; i < uvCount; i++)
        {
            uvs[i] = new Vector2(br.ReadByte() * 0.01f, br.ReadByte() * 0.01f);
        }
        byte clipCount = br.ReadByte();
        MeshSequence[] animationList = new MeshSequence[clipCount];
        for (int i = 0; i < clipCount; i++)
        {
            int clipFrame = br.ReadShort();
            float frameTime = br.ReadShort() * 0.001f;
            MeshSequence meshSeq = new MeshSequence(clipFrame, frameTime);
            for (int j = 0; j < clipFrame; j++)
            {
                Vector3[] verArray = new Vector3[verCount];
                Mesh mesh = new Mesh();
                for (int k = 0; k < verCount; k++)
                {
                    verArray[k] = new Vector3(br.ReadShort() / 100.0f, br.ReadShort() / 100.0f, br.ReadShort() / 100.0f);
                }
                mesh.vertices = verArray;
                mesh.triangles = tris;
                mesh.uv = uvs;
                meshSeq.meshs[j] = mesh;
            }
            animationList[i] = meshSeq;
        }
        return animationList;
    }

    private static byte[] LoadFile(string name)
    {
#if UNITY_EDITOR
        string path = string.Format("Assets/FrameData/{0}", name);
        return File.ReadAllBytes(path);
#else
        return null;
#endif
    }
}

public struct MeshSequence
{
    public int length;
    public float frameTime;
    public Mesh[] meshs;

    public MeshSequence(int frameLength, float time)
    {
        length = frameLength;
        frameTime = time;
        meshs = new Mesh[length];
    }
}