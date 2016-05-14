using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class DialogueDay5 : MonoBehaviour
{
    [SerializeField]
    List<int> m_times;
    [SerializeField]
    List<string> m_lines;
    [SerializeField]
    Text m_text;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(DialogForScene());
    }

    private IEnumerator DialogForScene()
    {
        for (int i = 0; i < m_times.Count; i++)
        {
            m_text.text = m_lines[i];
            yield return new WaitForSeconds(m_times[i]);
        }
    }
}