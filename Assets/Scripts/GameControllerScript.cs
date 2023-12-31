using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControllerScript : MonoBehaviour
{
    public int columns;
    public int rows;
    public float minPadding = 0.1f; // Minimum padding between sprites.
    public float maxPadding = 5f; // Maximum padding between sprites.

    [SerializeField] private MainImageScript startObject;
    [SerializeField] private Sprite[] images;
    [SerializeField] GameObject mainScreen;
    [SerializeField] GameObject gameOverScreen;

    private int[] Randomiser(int[] locations)
    {
        int[] array = locations.Clone() as int[];
        for(int i=0; i < array.Length; i++)
        {
            int newArray = array[i];
            int j = Random.Range(i, array.Length);
            array[i] = array[j];
            array[j] = newArray;
        }
        return array;
    }

    public void SelectMode(int modeno)
    {
        switch(modeno)
        {
            case 1:
                columns = 2;
                rows = 2;
                break;
            case 2:
                columns = 3;
                rows = 4;
                break;
            case 3:
                columns = 4;
                rows = 4;
                break;
        }
        AtStart();
        mainScreen.SetActive(false);
    }

    private void AtStart()
    {
        float screenHeight = (Camera.main.orthographicSize * 1.5f);
        float screenWidth = screenHeight * Camera.main.aspect;
        float padding = Mathf.Lerp(maxPadding, minPadding, Mathf.Min(rows, columns) / (float)Mathf.Max(rows, columns));
        float spriteWidth, spriteHeight;
        if (screenWidth < screenHeight)
        {
            spriteHeight = (screenHeight - (rows - 1) * padding) / rows;
            spriteWidth = (screenWidth - (columns - 1) * padding) / columns;
        }
        else
        {
            // Landscape mode
            spriteWidth = (screenWidth - (columns - 1) * padding) / columns;
            spriteHeight = (screenHeight - (rows - 1) * padding) / rows;
        }

        
        int totalElements = rows * columns;
        int[] locations = new int[totalElements];
        for (int i = 0; i < totalElements; i++)
        {
            locations[i] = i / 2; 
        }
        locations = Randomiser(locations);

        Vector3 startPosition = startObject.transform.position;
        
        for (int i = 0; i < columns; i++)
        {
            for(int j = 0; j < rows; j++)
            {
                MainImageScript gameImage;
                if(i == 0 && j == 0)
                {
                    gameImage = startObject;
                    
                }
                else
                {
                    gameImage = Instantiate(startObject) as MainImageScript;
                }

                int index = j * columns + i;
                int id = locations[index];
                gameImage.ChangeSprite(id, images[id], spriteWidth, spriteHeight);
                float xPos = i * (spriteWidth + padding) - screenWidth / 2 + spriteWidth / 2;
                float yPos = screenHeight / 2 - j * (spriteHeight + padding) - spriteHeight / 2;

                gameImage.transform.position = new Vector3(xPos, yPos, startPosition.z);
            }
        }
    }

    private MainImageScript firstOpen;
    private MainImageScript secondOpen;

    private int score = 0;
    private int attempts = 0;

    [SerializeField] private TextMesh scoreText;
    [SerializeField] private TextMesh attemptsText;

    public bool canOpen
    {
        get { return secondOpen == null; }
    }

    public void imageOpened(MainImageScript startObject)
    {
        if(firstOpen == null)
        {
            firstOpen = startObject;
        }
        else
        {
            secondOpen = startObject;
            StartCoroutine(CheckGuessed());
        }
    }

    private IEnumerator CheckGuessed()
    {
        if (firstOpen.spriteId == secondOpen.spriteId) // Compares the two objects
        {
            firstOpen.Successfull();
            score++; // Add score
            scoreText.text = "Score: " + score;
        }
        else
        {
            yield return new WaitForSeconds(0.5f); // Start timer

            firstOpen.Close();
            secondOpen.Close();
        }

        attempts++;
        attemptsText.text = "Attempts: " + attempts;

        
        if(score == rows*columns/2)
        {
            firstOpen.GameOver();
            gameOverScreen.SetActive(true);
            Debug.Log("Game Over");
        }
        firstOpen = null;
        secondOpen = null;
    }

    public void Restart()
    {
        SceneManager.LoadScene("MainScene");
    }

}
