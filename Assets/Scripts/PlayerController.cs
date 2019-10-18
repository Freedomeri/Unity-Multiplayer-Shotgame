using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {
    public Texture texture;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float sensitivityMouse = 2f;
    private Vector3 hitInfo;
    private object m_Camera;
    public float MoveSpeed=15;
    private void Start()
    {
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update () {
        if (isLocalPlayer == false)
        {
            return;
        }


        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
       // float x = Input.GetAxis("Mouse X");

        transform.Rotate(Vector3.up * h * 120 * Time.deltaTime);
       // transform.Rotate(Vector3.up * x * 120 * Time.deltaTime);
        transform.Translate(Vector3.forward * v * 15 * Time.deltaTime);
        //transform.Translate(Vector3.right * h * 15 * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.J))
        {
            CmdFire();//射击
        }
	}


    public override void OnStartLocalPlayer()
    {
        //这个方法只会在本地角色那里调用  当角色被创建的时候
        GetComponent<MeshRenderer>().material.color = Color.blue;
    }


    [Command]// called in client, run in server
    void CmdFire()//这个方法需要在server里面调用
    {
        // 子弹的生成 需要server完成，然后把子弹同步到各个client
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation) as GameObject;
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 100;
        Destroy(bullet, 2);
        NetworkServer.Spawn(bullet);
    }


}
