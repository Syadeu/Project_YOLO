using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Syadeu.Mono
{
    public sealed class TextUIComponent : MonoBehaviour
    {
        public CanvasGroup m_CanvasGroup;
        public Text m_Text;

        private string TargetText = string.Empty;
        private Coroutine Coroutine = null;

        public bool IsTexting => Coroutine != null;

        public void StartText(string text, float speed)
        {
            if (Coroutine != null)
            {
                StopCoroutine(Coroutine);
            }

            TargetText = text;
            Coroutine = StartCoroutine(TextIter(text, speed));
        }
        IEnumerator TextIter(string text, float speed)
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(speed);

            int currentIdx = 0;
            while (currentIdx < text.Length)
            {
                m_Text.text = text.Substring(0, currentIdx);
                currentIdx++;
                yield return waitForSeconds;
            }

            m_Text.text = text;
            Coroutine = null;
        }
        public void Skip()
        {
            if (Coroutine != null)
            {
                StopCoroutine(Coroutine);
                Coroutine = null;
            }
            m_Text.text = TargetText;
        }

        #region Recycler

        public void OnCreated()
        {
            m_CanvasGroup.alpha = 0;
        }
        public void OnInitialize()
        {
            m_CanvasGroup.Lerp(1, .1f);
        }
        public void OnTerminate()
        {
            m_CanvasGroup.Lerp(0, 1.5f);
        }

        #endregion
    }
}
