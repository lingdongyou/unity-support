using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
public class AnimatorMaker
{
    private Animation m_Animation;
    private Animator m_Animator;
    private SkinnedMeshRenderer m_SkinRender;
    private BinaryWriter m_BinaryWriter;
    private FileStream m_FileStream;
    private GameObject gameObject;

    public AnimatorMaker(GameObject prefab)
    {
        gameObject = Object.Instantiate(prefab);
        m_Animation = gameObject.GetComponent<Animation>();
        m_Animator = gameObject.GetComponent<Animator>();
        m_SkinRender = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        m_FileStream = File.Open(string.Format("Assets/FrameData/{0}", prefab.name), FileMode.Create);
        m_BinaryWriter = new BinaryWriter(m_FileStream);
    }

    public void Bake()
    {
        int vertexCount = m_SkinRender.sharedMesh.vertexCount;
        int[] trangles = m_SkinRender.sharedMesh.triangles;
        Vector2[] uvs = m_SkinRender.sharedMesh.uv;

        ByteWrite byteWrite = new ByteWrite();

        byteWrite.WriteInt(vertexCount);
        byteWrite.WriteInt(trangles.Length);
        byteWrite.WriteInt(uvs.Length);

        for (int i = 0; i < trangles.Length; i++)
        {
            byteWrite.WriteShort((short)(trangles[i]));
        }
        for (int i = 0; i < uvs.Length; i++)
        {
            byteWrite.WriteByte((byte)(uvs[i].x * 100));
            byteWrite.WriteByte((byte)(uvs[i].y * 100));
        }

        if (m_Animation)
        {
            List<AnimationState> animClips = new List<AnimationState>(m_Animation.Cast<AnimationState>());
            byteWrite.WriteByte((byte)(animClips.Count));
            m_BinaryWriter.Write(byteWrite.ToByteArray());
            for (int i = 0; i < animClips.Count; i++)
            {
                var clip = animClips[i].clip;
                if (!clip.legacy)
                {
                    Debug.LogError(string.Format("{0} 动画系统", clip.name));
                    continue;
                }
                BakePerAnimClip(clip);
            }
        }
        else if(m_Animator)
        {
           var clips = m_Animator.runtimeAnimatorController.animationClips;
            byteWrite.WriteByte((byte)(clips.Length));
            m_BinaryWriter.Write(byteWrite.ToByteArray());
            for (int i = 0; i < clips.Length; i++)
            {
                BakePerAnimatorClip(clips[i]);
            }
        }

        if (m_BinaryWriter != null)
        {
            m_BinaryWriter.Close();
            m_BinaryWriter = null;
        }
        if (m_FileStream != null)
        {
            m_FileStream.Close();
            m_FileStream = null;
        }

        if (gameObject != null)
        {
            Object.DestroyImmediate(gameObject);
            gameObject = null;
        }
    }


    public void BakePerAnimClip(AnimationClip clip)
    {
        ByteWrite byteWrite = new ByteWrite();
        int curClipFrame = (int)(clip.frameRate * clip.length);
        float perFrameTime = clip.length / curClipFrame;
        m_Animation.Play(clip.name);
        float sampleTime = 0;

        byteWrite.WriteShort((short)curClipFrame);
        byteWrite.WriteShort((short)(perFrameTime * 1000));

        for (int i = 0; i < curClipFrame; i++)
        {
            clip.SampleAnimation(m_Animation.gameObject, sampleTime);
            //            m_Animation.Sample();
            Mesh bakedMesh = new Mesh();
            m_SkinRender.BakeMesh(bakedMesh);
            for (int j = 0; j < bakedMesh.vertexCount; j++)
            {
                Vector3 vertex = bakedMesh.vertices[j];
                byteWrite.WriteShort((short)(vertex.x * 100));
                byteWrite.WriteShort((short)(vertex.y * 100));
                byteWrite.WriteShort((short)(vertex.z * 100));
            }
            sampleTime += perFrameTime;
        }
        m_BinaryWriter.Write(byteWrite.ToByteArray());
    }

    public void BakePerAnimatorClip(AnimationClip clip)
    {
        ByteWrite byteWrite = new ByteWrite();
        int curClipFrame = (int)(clip.frameRate * clip.length);
        float perFrameTime = clip.length / curClipFrame;
        m_Animator.Play(clip.name);
        float sampleTime = 0;

        byteWrite.WriteShort((short)curClipFrame);
        byteWrite.WriteShort((short)(perFrameTime * 1000));
        for (int i = 0; i < curClipFrame; i++)
        {
            clip.SampleAnimation(m_Animator.gameObject, sampleTime);
            Mesh bakedMesh = new Mesh();
            m_SkinRender.BakeMesh(bakedMesh);
            for (int j = 0; j < bakedMesh.vertexCount; j++)
            {
                Vector3 vertex = bakedMesh.vertices[j];
                byteWrite.WriteShort((short)(vertex.x * 100));
                byteWrite.WriteShort((short)(vertex.y * 100));
                byteWrite.WriteShort((short)(vertex.z * 100));
            }
            sampleTime += perFrameTime;
        }
        m_BinaryWriter.Write(byteWrite.ToByteArray());
    }
}
