using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOFlyAction : SSAction {
    public float gravity = -5;
    //设置物体重力
    private Vector3 startVector;//物体随机出现
    private Vector3 gravityVector = Vector3.zero;//物体做平抛运动时改变的位置
    private float time;//记录物体运动时间
    private Vector3 currentAngle = Vector3.zero;//记录抛出角度

    private UFOFlyAction()
    {

    }

    public static UFOFlyAction GetSSAction(Vector3 direction, float angle, float power)
    {
        UFOFlyAction action = CreateInstance<UFOFlyAction>();
        if (direction.x == -1)
        {
            action.startVector = Quaternion.Euler(new Vector3(0, 0, -angle)) * Vector3.left * power;
        }
        else
        {
            action.startVector = Quaternion.Euler(new Vector3(0, 0, angle)) * Vector3.right * power;
        }
        return action;
    }

    public override void Update()
    {
        time += Time.fixedDeltaTime;
        gravityVector.y = gravity * time;

        transform.position += (startVector + gravityVector) * Time.fixedDeltaTime;
        currentAngle.z = Mathf.Atan((startVector.y + gravityVector.y) / startVector.x);
        transform.eulerAngles = currentAngle;

        if(this.transform.position.y < -10)
        {
            this.destroy = true;
            this.callback.SSActionEvent(this);
        }
    }

    public override void Start()
    {

    }
}
