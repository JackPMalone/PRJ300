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

    [SerializeField] Button resetButton;

    int playerCount = 1, playerScore = 0, dealerCount = 1, dealerScore = 0;

    //Specific Y values for the player and dealer
    float playerY = -3.32f, dealerY = 3.1f;

    //Specific X values for each of the cards
    float[] columns = { -7f, -3.5f, 0f, 3.5f, 7f };

    [SerializeField] List<Cards> cards = new List<Cards>();

    [SerializeField] List<Cards> playerHand = new List<Cards>();
    [SerializeField] List<Cards> dealerHand = new List<Cards>();

    bool doneBefore = false;

    string playerWin = "Congrats!!!\nYou won\n\nDo you want to play again?", dealerWin = "Darn\nSo close\n\nDo you want to try again?";

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

    private void Update()
    {
        if (playerScore >= 21)
        {
            Stand();
            return;
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            PlayerHit();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Stand();
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
        while (dealerScore < 17)
        {
            DealerHit();
        }

        if (dealerScore < playerScore)
            StartCoroutine("PlayerWin");
        else
            StartCoroutine("DealerWin");

        resetButton.gameObject.SetActive(true);
    }

    public void PlayerHit()
    {
        if (playerCount >= 4) return;

        playerCount++;
        InstantiateCards(playerCount, true);
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
        }
        else
        {
            Instantiate(dealerHand[i].face, new Vector2(columns[i], dealerY), Quaternion.identity);
            dealerScore += Score(dealerHand[i]);
            dealerScoreText.text = string.Format($"Dealer: {dealerScore}");
        }
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
                return ((int)card.value + 1);

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

    public IEnumerator PlayerWin()
    {
        foreach (char c in playerWin)
        {
            endSplash.text += c;
            yield return new WaitForSeconds(0.125f);
        }
    }

    private IEnumerator DealerWin()
    {
        endSplash.color = Color.red;

        foreach (char c in dealerWin)
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
