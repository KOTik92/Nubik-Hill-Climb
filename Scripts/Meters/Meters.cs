using DG.Tweening;
using Sdk.Saving;
using TMPro;
using UnityEngine;
using Wallet;

namespace Game
{
    public class Meters : MonoBehaviour
    {
        [SerializeField] private float distanceMoney;
        [SerializeField] private int amountMoney;
        [SerializeField] private WalletGame walletGame;
        [SerializeField] private TextMeshProUGUI metersText;
        [SerializeField] private TextMeshProUGUI maxMetersText;

        [Header("EffectNewRecord")] 
        [SerializeField] private AudioSource audioSourceNewRecord;
        [SerializeField] private ParticleSystem[] particleSystemsNewRecord;
        [SerializeField] private TextMeshProUGUI textNewRecord;

        public int Distance => _distance;
        public int MaxDistance => _maxDistance;
        
        private float _posX;
        private Car.Car _car;
        private int _distance;
        private int _lastDistance;
        private int _maxDistance;
        private int _metersMoney;
        private string _saveName;
        private bool _isNewRecord;

        public void Init(Car.Car car, string saveName)
        {
            _car = car;
            _posX = _car.transform.localPosition.x;
            _saveName = saveName;
            _maxDistance = SavesFacade.GetMeters(_saveName);
            
            if (_maxDistance == 0)
                _isNewRecord = true;
            
            metersText.text = $"{_distance}m";
            maxMetersText.text = $"{_maxDistance}m";
        }
        
        private void Update()
        {
            if (_car.transform.localPosition.x >= _posX)
            {
                _distance = (int)_posX;

                if (_distance >= _maxDistance)
                {
                    _maxDistance = _distance;
                    maxMetersText.text = $"{_maxDistance}m";

                    if (!_isNewRecord)
                    {
                        ActivateEffectNewRecord();
                        _isNewRecord = true;
                    }
                }

                metersText.text = $"{_distance}m";

                if (_distance >= _metersMoney + distanceMoney)
                {
                    _metersMoney = _distance;
                    walletGame.AddMoney(amountMoney);
                }
                
                _posX = _car.transform.localPosition.x;
                
            }
        }

        private void ActivateEffectNewRecord()
        {
            audioSourceNewRecord.Play();
            textNewRecord.gameObject.SetActive(true);
            textNewRecord.transform.localScale = Vector3.zero;
            textNewRecord.transform.DOScale(Vector3.one, 0.3f).OnComplete(() =>
            {
                textNewRecord.transform.DOScale(Vector3.zero, 0.5f).SetDelay(1).OnComplete(() => { textNewRecord.gameObject.SetActive(false); });
            });
            
            foreach (var particle in particleSystemsNewRecord)
            {
                if(particle.isStopped)
                    particle.Play();
            }
        }

        public void Save()
        {
            SavesFacade.SetMeters(_saveName, _maxDistance);
        }
    }
}
