
using UnityEngine;
public class OtherSoldier : MonoBehaviour
{
    public MeshActor actor1;
    public MeshActor actor2;
    public MeshActor actor3;
    public MeshActor actor4;
    public MeshActor actor5;

    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                Manager.Instance.GetActor().position = new Vector3(i * 2, 0, j * 2);
            }
        }
        //        actor1 = Manager.Instance.GetActor();
        //        actor2 = Manager.Instance.GetActor();
        //        actor3 = Manager.Instance.GetActor();
        //        actor4 = Manager.Instance.GetActor();
        //        actor5 = Manager.Instance.GetActor();
        //        actor1.position = new Vector3(0,0,1);
        //        actor2.position = new Vector3(0, 0, 2);
        //        actor3.position = new Vector3(0, 0, 3);
        //        actor4.position = new Vector3(0, 0, 4);
        //        actor5.position = new Vector3(0, 0, 5);
    }
}
