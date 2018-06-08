using Neatly.Timer;
using UnityEngine;

public class MeshActor
{
    public Vector3 position;
    public Vector3 rotate;
    public Vector3 scale = Vector3.one;
    private MeshSequence[] meshSeqArray;
    private MeshSequence meshSeq;
    private int m_State = -1;
    private int index = 0;
    public Mesh mesh { get; private set; }

    public void Init(MeshSequence[] seqArray)
    {
        meshSeqArray = seqArray;
        Play(AnimatorSeqDef.Attack1);
        NeatlyTimer.AddClock(this, this.OnClock, meshSeq.frameTime);
    }
    private void OnClock(float dt)
    {
        mesh = meshSeq.meshs[index];
        index++;
        if (index >= meshSeq.meshs.Length)
        {
            if (AnimatorSeqDef.StateLoop[m_State])
            {
                index = 0;
            }
            else
            {
                Play(AnimatorSeqDef.Idle);
            }
        }
    }

    public void Play(int state)
    {
        if (state == m_State) return;
        m_State = state;
        index = 0;
        meshSeq = meshSeqArray[m_State];
        mesh = meshSeq.meshs[index];
    }

    public void LookAt(Vector3 targetV3)
    {
        float deg = Mathf.Atan2(targetV3.x - position.x , targetV3.z - position.z) * Mathf.Rad2Deg;
        rotate = new Vector3(0, deg, 0);
    }
}

