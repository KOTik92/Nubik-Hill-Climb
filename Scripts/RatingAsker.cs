using Sdk.Rating;
using Sdk.Saving;
using UnityEngine;

[RequireComponent(typeof(AppRating))]
public class RatingAsker : MonoBehaviour
{
    private void Start()
    {
        if (SavesFacade.TotalTries == 3 && !SavesFacade.IsAlreadyAskedToRate)
        {
            GetComponent<AppRating>().RateAndReview();
        }
    }
}