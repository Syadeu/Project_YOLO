using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Syadeu.Mono
{
    public sealed class TextUIComponent : MonoBehaviour
    {
        public Text m_Text;

        private string TargetText = string.Empty;
        private Coroutine Coroutine = null;

        public void StartText(string text)
        {
            if (Coroutine != null)
            {
                StopCoroutine(Coroutine);
            }

            TargetText = text;
            Coroutine = StartCoroutine(TextIter(text));
        }
        IEnumerator TextIter(string text)
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(.2f);

            int currentIdx = 0;
            while (currentIdx < text.Length)
            {
                m_Text.text = text.Substring(0, currentIdx);
                currentIdx++;
                yield return waitForSeconds;
            }

            m_Text.text = text;
        }
        public void Skip()
        {
            if (Coroutine != null)
            {
                StopCoroutine(Coroutine);
            }
            m_Text.text = TargetText;
        }
    }
}
