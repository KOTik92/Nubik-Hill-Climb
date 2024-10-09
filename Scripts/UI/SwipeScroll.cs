using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class SwipeScroll : MonoBehaviour
    {
        [SerializeField] private Scrollbar scrollbar;
        [SerializeField] private Transform content;
        [SerializeField] private float speedScale;
        [SerializeField] private float speedPos;

        private float _scrollPos;
        private float[] _positions;
        private float _distance;
        private bool _isScroll;

        public void Init(int num = 0)
        {
            _positions = new float[content.childCount];
            float amount = _positions.Length - 1;
            _distance = 1 / amount;

            for (int i = 0; i < _positions.Length; i++)
                _positions[i] = _distance * i;

            StartCoroutine(SetScroll(num));
        }

        private IEnumerator SetScroll(int num)
        {
            _scrollPos = _positions[num];
            yield return null;
            scrollbar.value = _positions[num];
            _isScroll = true;
        }

        private void FixedUpdate()
        {
            if(!_isScroll)
                return;

            if (Input.GetMouseButton(0))
                _scrollPos = scrollbar.value;
            else
            {
                for (int i = 0; i < _positions.Length; i++)
                {
                    if (_scrollPos < _positions[i] + (_distance / 2) && _scrollPos > _positions[i] - (_distance / 2))
                        scrollbar.value = Mathf.Lerp(scrollbar.value, _positions[i], speedPos);
                }
            }
            
            for (int i = 0; i < _positions.Length; i++)
            {
                if (_scrollPos < _positions[i] + (_distance / 2) && _scrollPos > _positions[i] - (_distance / 2))
                {
                    content.GetChild(i).localScale = Vector2.Lerp(content.GetChild(i).localScale, new Vector2(1.1f, 1.1f), speedScale);

                    for (int j = 0; j < _positions.Length; j++)
                    {
                        if(j != i)
                            content.GetChild(j).localScale = Vector2.Lerp(content.GetChild(j).localScale, new Vector2(1f, 1f), speedScale);
                    }
                }
            }
        }
    }
}
