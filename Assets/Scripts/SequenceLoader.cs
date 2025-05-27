using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SequenceLoader : MonoBehaviour
{

    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI extraInfoText;

    public RewardSequence rewardSequence;
    public RecipeSO debugSO;

    private void Start()
    {
        LoadRecipe(debugSO);
    }
    public void LoadRecipe(RecipeSO recipe)
    {
        StartCoroutine(RecipeLoop(recipe));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            ArdunioThink.instance.OnWeightRecieved.Invoke(Random.Range(0f, 10000f)); // Simulate weight input for testing
        }
    }


    public IEnumerator RecipeLoop(RecipeSO recipe)
    {

        foreach (var seq in recipe.sequences)
        {
            if (seq.sequenceType == Sequence.SequenceType.Action)
            {
                yield return ActionSequence(seq);
            }
            else if (seq.sequenceType == Sequence.SequenceType.Weight)
            {
                yield return WeightSequence(seq);
            }
            yield return new WaitForSeconds(3);
        }

        yield return null;

    }

    public IEnumerator ActionSequence(Sequence seq)
    {
        float startingTime = Time.time;
        descriptionText.text = seq.description;
        bool isDone = false;
        ArdunioThink.instance.OnWeightRecieved.AddListener((float weight) =>
        {
            isDone = true;
        });

        while (!isDone)
        {
            extraInfoText.text = "Time Left: " + (seq.duration - (Time.time - startingTime)).ToString("F2") + "s";
            if (Time.time - startingTime >= seq.duration)
            {
                //isDone = true; // Time is up
                ArdunioThink.instance.ChangeLedState(1);
                extraInfoText.text = "Time's up!";
            }
            yield return null;
        }

        yield return new WaitUntil(() => isDone);
        ArdunioThink.instance.ChangeLedState(0);


        float points = Mathf.Clamp(2000f - (Time.time - startingTime) * 100f, 0, 2000f); // Calculate points based on time taken
        //depending on how off the time is give or take points 
        GiveRewards(points); // Placeholder for points calculation


    }

    public IEnumerator WeightSequence(Sequence seq)
    {

        bool isDone = false;
        float currentWeightShowing=0;
        descriptionText.text = seq.description;

        ArdunioThink.instance.OnWeightRecieved.AddListener((float weight) =>
        {
            currentWeightShowing = weight;
            if (Mathf.Abs(currentWeightShowing - seq.weight) < 10000)
            {
                isDone = true;
            }
            else
            {
                //show error
            }
        });

        extraInfoText.text = "Weight Required: " + seq.weight.ToString("F2") + "g";

        yield return new WaitUntil(() => isDone == true);
        float point = 2000f - currentWeightShowing - seq.weight;
        point = Mathf.Clamp(point, 0, 2000f); // Ensure points are within a valid range
        GiveRewards(point); // Placeholder for points calculation


        //depending on the diffrence between required weight vs currentweight give player points 


    }

    public void GiveRewards(float points)
    {
        //2500 max
        //2000 3 star 
        //1500 2 star
        //1000 1 star

        rewardSequence.StartSequence(points);
        //below is 0 star 
    }



}
