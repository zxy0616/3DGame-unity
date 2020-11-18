using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine;
//追踪动作在动作管理器CCActionManager类中实现了Tracert函数，传入了追踪者和被追踪的对象也就是玩家对象。创建了追踪事件，在追上玩家或者追踪标志follow_player被置为false前一直追着玩家（当碰撞事件发生时追踪者的追踪标志会被场记设置为false）。
public class CCTracertAction : SSAction
{
    public GameObject target;
    public float speed;

    private CCTracertAction() { }
    public static CCTracertAction getAction(GameObject target, float speed)
    {
        CCTracertAction action = ScriptableObject.CreateInstance<CCTracertAction>();
        action.target = target;
        action.speed = speed;
        return action;
    }

    public override void Update()
    {
        this.transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        Quaternion rotation = Quaternion.LookRotation(target.transform.position - gameObject.transform.position, Vector3.up);
        gameObject.transform.rotation = rotation;
        if (gameObject.GetComponent<Prop>().follow_player == false||transform.position == target.transform.position)
        {
            destroy = true;
            CallBack.SSActionCallback(this);
        }
    }

    public override void Start()
    {

    }
}

