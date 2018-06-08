
using UnityEngine;
using UnityEngine.UI;

public class MainSoldier : MonoBehaviour
{
    public NJoystick joystick;
    public Button btnAttack;
    public Button btnDeath;
    private MeshActor actor;

    private float speed = 4;

    void Start()
    {
        actor = Manager.Instance.GetActor();
        actor.position = Vector3.zero;
        joystick.Register(null, OnMove, OnMoveEnd);
        btnAttack.onClick.AddListener(() => { actor.Play(AnimatorSeqDef.Attack1); });
        btnDeath.onClick.AddListener(() => { actor.Play(AnimatorSeqDef.Death); });
    }

    public void OnMove(int x, int y)
    {
        var targetPosition = actor.position + new Vector3(x / 10000.0f, 0, y / 10000.0f) * speed * Time.deltaTime;
        actor.LookAt(targetPosition);
        actor.position = targetPosition;
        actor.Play(AnimatorSeqDef.Run);
    }
    public void OnMoveEnd()
    {
        actor.Play(AnimatorSeqDef.Idle);
    }
}
