using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;
    public GameObject mainMenu;
    public GameObject recipesMenu;
    public GameObject recipeGameMenu;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        OpenMainMenu();
    }
    public void StartRecipeGame(RecipeSO recipeData)
    {

        mainMenu.SetActive(false);
        recipesMenu.SetActive(false);
        recipeGameMenu.SetActive(true);
        SequenceLoader.instance.LoadRecipe(recipeData);
        //  RecipeGameManager.Instance.StartRecipeGame(recipeData);
    }

    public void OpenRecipesMenu()
    {

        mainMenu.SetActive(false);
        recipesMenu.SetActive(true);
        recipeGameMenu.SetActive(false);
    }

    public void OpenMainMenu()
    {

        mainMenu.SetActive(true);
        recipeGameMenu.SetActive(false);
    }
}
