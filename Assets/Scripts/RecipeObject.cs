using UnityEngine;

public class RecipeObject : MonoBehaviour
{
    public RecipeSO recipe;


    public void StartRecipeGame()
    {
        MenuManager.instance.StartRecipeGame(recipe);
    }
}
