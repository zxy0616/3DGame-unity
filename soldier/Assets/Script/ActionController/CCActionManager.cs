using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;
//场记通过动作管理器CCActionManager管理对象的移动，CCActionManager实现了追踪Tracert，巡逻GoAround方法，并通过回调函数来循环执行巡逻动作或者在追踪结束时继续巡逻动作。
public class CCActionManager : SSActionManager, SSActionCallback
{
    public SSActionEventType Complete = SSActionEventType.Completed;
    Dictionary<int,CCMoveToAction> actionList = new Dictionary<int, CCMoveToAction>();

    public void Tracert(GameObject p,GameObject player)
    {
        if (actionList.ContainsKey(p.GetComponent<Prop>().block)) actionList[p.GetComponent<Prop>().block].destroy = true;
        CCTracertAction action = CCTracertAction.getAction(player, 0.8f);
        addAction(p.gameObject, action, this);
    }
    //巡逻动作则是选取一个在合理范围的位置，朝该位置移动，到达后调用回调函数继续巡逻动作。
    public void GoAround(GameObject p)
    {
        CCMoveToAction action = CCMoveToAction.getAction(p.GetComponent<Prop>().block,0.6f,GetNewTarget(p));
        actionList.Add(p.GetComponent<Prop>().block, action);
        addAction(p.gameObject, action, this);
    }

    private Vector3 GetNewTarget(GameObject p)
    {
        Vector3 pos = p.transform.position;
        int block = p.GetComponent<Prop>().block;
        float ZUp = 13.2f - (block / 3) * 9.65f;
        float ZDown = 5.5f - (block / 3) * 9.44f;
        float XUp = -4.7f + (block % 3) * 8.8f;
        float XDown = -13.3f + (block % 3) * 10.1f;
        Vector3 Move = new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));
        Vector3 Next = pos + Move;
        while (!(Next.x<XUp && Next.x>XDown && Next.z<ZUp && Next.z > ZDown))
        {
            Move = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
            Next = pos + Move;
        }
        return Next;
    }

    public void StopAll()
    {
        foreach(CCMoveToAction x in actionList.Values)
        {
            x.destroy = true;
        }
        actionList.Clear();
    }

    public void SSActionCallback(SSAction source)
    {
        if(actionList.ContainsKey(source.gameObject.GetComponent<Prop>().block)) actionList.Remove(source.gameObject.GetComponent<Prop>().block);
        GoAround(source.gameObject);
    }
}