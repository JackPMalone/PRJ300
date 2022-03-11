using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    [SerializeField] TMP_Text playerScoreText;
    [SerializeField] TMP_Text dealerScoreText;
    [SerializeField] TMP_Text endSplash;

    [SerializeField] Rigidbody2D rick;

    [SerializeField] Button resetButton;
    [SerializeField] Button hitButton;
    [SerializeField] Button standButton;

    public int playerCount = 1;
    public int something = 0;
    int playerScore = 0, dealerCount = 1, dealerScore = 0, instantiated = 0;

    public float speed = 1;

    //Specific Y values for the player and dealer
    float playerY = -3.32f, dealerY = 3.1f;

    //Specific X values for each of the cards
    float[] columns = { -7f, -3.5f, 0f, 3.5f, 7f };

    [SerializeField] List<Cards> cards = new List<Cards>();

    [SerializeField] List<Cards> playerHand = new List<Cards>();
    [SerializeField] List<Cards> dealerHand = new List<Cards>();

    public bool doneBefore = false;

    string yes = "Never gonna give you up\nNever gonna let you down\nNever gonna run around and desert you\nNever gonna make you cry\nNever gonna say goodbye\nNever gonna tell a lie and hurt you", playerWin = "Congrats!!!\nYou won\n\nDo you want to play again?", dealerWin = "Darn\nSo close\n\nDo you want to try again?";

    bool dumbStuffAhead = false;

    private void Update()
    {
        Logic();

        if (Input.GetKeyDown(KeyCode.R))
            Reset();
        if (Input.GetKeyDown(KeyCode.S))
            StartCoroutine(Ending(Color.green, playerWin));
        if (Input.GetKeyDown(KeyCode.P))
            StartCoroutine(Ending(Color.red, dealerWin));
        if (Input.GetKey(KeyCode.J) && Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.M) && Input.GetKey(KeyCode.I) && Input.GetKey(KeyCode.E) && !dumbStuffAhead)
        {
            dumbStuffAhead = true;
            rick.AddForce(new Vector3(speed * -1, 0f, 0));
        }

        //if (rick.position.x < -88)
        //    rick.AddForce(new Vector2(-rick.velocity.x, 0));
    }

    private void Start()
    {
        doneBefore = false;

        playerScore = 0;

        for (int i = 0; i < 5; i++)
        {
            SetPlayerHand(Mathf.RoundToInt(Random.Range(0, 52)));

            SetDealerHand(Mathf.RoundToInt(Random.Range(0, 52)));
        }

        for (int i = 0; i < 2; i++)
        {
            InstantiateCards(i, true);
            InstantiateCards(i, false);
        }
    }

    private void SetPlayerHand(int i)
    {
        playerHand.Add(cards[i]);
    }

    private void SetDealerHand(int i)
    {
        dealerHand.Add(cards[i]);
    }

    public void Stand()
    {
        hitButton.gameObject.SetActive(false);
        standButton.gameObject.SetActive(false);

        while (dealerScore < 17 && dealerCount <= 4)
        {
            DealerHit();
        }

        Logic();

        resetButton.gameObject.SetActive(true);
    }

    public void PlayerHit()
    {
        if (something >= 4)
        {
            Stand();
            return;
        }

        something++;

        playerCount++;
        InstantiateCards(playerCount, true);

        if (playerScore > 21 && doneBefore)
            playerScore -= 11;
    }

    private void DealerHit()
    {
        if (dealerCount >= 4) return;

        dealerCount++;
        InstantiateCards(dealerCount, false);
    }

    private void InstantiateCards(int i, bool player)
    {
        if (player)
        {
            Instantiate(playerHand[i].face, new Vector2(columns[i], playerY), Quaternion.identity);
            playerScore += Score(playerHand[i]);
            playerScoreText.text = string.Format($"Player: {playerScore}");
            instantiated++;
        }
        else
        {
            Instantiate(dealerHand[i].face, new Vector2(columns[i], dealerY), Quaternion.identity);
            dealerScore += Score(dealerHand[i]);
            dealerScoreText.text = string.Format($"Dealer: {dealerScore}");
        }
    }

    private void Logic()
    {
        if (instantiated == 2 && playerScore == 21)
            Ending(Color.green, "BLACKJACK!!!\nYou Won!!!\n\nDo you want to play again?");
    }

    private int Score(Cards card)
    {
        switch (card.value)
        {
            case Values.Ace:
                return AceStuff();

            case Values.Two:
            case Values.Three:
            case Values.Four:
            case Values.Five:
            case Values.Six:
            case Values.Seven:
            case Values.Eight:
            case Values.Nine:
                return (int)card.value + 1;

            case Values.Ten:
            case Values.Jack:
            case Values.Queen:
            case Values.King:
                return 10;

            default:
                return 0;
        }
    }

    private int AceStuff()
    {
        if ((playerScore + 11) < 21 && doneBefore == false)
        {
            doneBefore = true;
            return 11;
        }
        else
            return 1;
    }

    public IEnumerator Ending(Color color, string text)
    {
        endSplash.color = color;

        foreach (char c in text)
        {
            endSplash.text += c;
            yield return new WaitForSeconds(0.125f);
        }
    }

    public void Reset()
    {
        SceneManager.LoadScene(0);
    }
}
