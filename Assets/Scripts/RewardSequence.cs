using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardSequence : MonoBehaviour
{

    public GameObject rewardDisplay;
    public TextMeshProUGUI finalScore;

    public Image star1, star2, star3;

    public void StartSequence(float point)
    {
        rewardDisplay.SetActive(true);

        StartCoroutine(RewardAnimation(point));

    }


    public IEnumerator RewardAnimation(float point)
    {

        finalScore.gameObject.SetActive(true);
        float startingTime = Time.time;
        float duration = 2f; // Duration of the animation
        float endTime = startingTime + duration;
        while (Time.time < endTime)
        {
            float t = (Time.time - startingTime) / duration;
            finalScore.text = Mathf.Lerp(0, point, t).ToString("F0");
            if (Mathf.Lerp(0, point, t) >= 1000 && !star1.gameObject.activeSelf)
            {
                star1.gameObject.SetActive(true);

            }

            if (Mathf.Lerp(0, point, t) >= 1500 && !star2.gameObject.activeSelf)
            {
                star2.gameObject.SetActive(true);

            }

            if (Mathf.Lerp(0, point, t) >= 1900 && !star3.gameObject.activeSelf)
            {
                Debug.Log("start3thing");
                star3.gameObject.SetActive(true);

            }

            yield return null;
        }

        if (point >= 1000 && !star1.gameObject.activeSelf)
        {
            star1.gameObject.SetActive(true);

        }

        if (point >= 1500 && !star2.gameObject.activeSelf)
        {
            star2.gameObject.SetActive(true);

        }

        if (point >= 1900 && !star3.gameObject.activeSelf)
        {
            Debug.Log("start3thing");
            star3.gameObject.SetActive(true);

        }

        finalScore.text = point.ToString("F0");
        yield return new WaitForSeconds(1f);
        ResetAnim();
    }


    public void ResetAnim()
    {
        finalScore.gameObject.SetActive(false);
        star1.gameObject.SetActive(false);
        star2.gameObject.SetActive(false);
        star3.gameObject.SetActive(false);
        rewardDisplay.SetActive(false);

    }
}
