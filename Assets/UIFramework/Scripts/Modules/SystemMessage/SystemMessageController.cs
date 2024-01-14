using System.Collections;
using System.Collections.Generic;
using UIFramework.Utils;
using UnityEngine;

public class SystemMessageController : MonoBehaviour
{
    [SerializeField] private GameObject SystemMessagePrefab;
    [SerializeField] private float sysMsgInterval = 0.5f;

    private List<GameObject> mListSystemMsg;
    private const int mMaxMsg = 10;
    private Queue<string> buffer = new Queue<string>();
    private Coroutine countdownTimer = null;
    private WaitForSeconds waitForSeconds;

    private void Awake()
    {
        mListSystemMsg = SimpleObjMgr.Instance.InitGameObjectPool(transform, SystemMessagePrefab, SystemMessagePrefab.transform.localPosition, SystemMessagePrefab.transform.localScale, mMaxMsg);
        waitForSeconds = new WaitForSeconds(sysMsgInterval);
    }

    public void AddSystemMessage(string message)
    {
        buffer.Enqueue(message);

        if (countdownTimer == null)
            countdownTimer = StartCoroutine(ShowMessage());
    }

    private IEnumerator ShowMessage()
    {
        GameObject inst = SimpleObjMgr.Instance.GetContainerObject(mListSystemMsg);
        if (inst != null)
            inst.GetComponent<UI_SystemMessage>().ShowMessage(buffer.Dequeue());

        yield return waitForSeconds;

        if (buffer.Count > 0)
            countdownTimer = StartCoroutine(ShowMessage());
        else
            countdownTimer = null;
    }

    private void OnDestroy()
    {
        buffer.Clear();

        if (mListSystemMsg != null)
        {
            SimpleObjMgr.Instance.DestroyContainerObject(mListSystemMsg);
            mListSystemMsg.Clear();
            mListSystemMsg = null;
        }
    }
}