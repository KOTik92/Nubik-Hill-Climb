using Menu.Car;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(CarItem))]
    public class StreamingAssetsCarIcon : StreamingAssetsItemIcon
    {
        protected override string Path => $"{Application.streamingAssetsPath}/Cars/";
        
        private void Start()
        {
            var carType = GetComponent<CarItem>().Car.typeCar;
            LoadIcon(carType.ToString());
        }
    }
}