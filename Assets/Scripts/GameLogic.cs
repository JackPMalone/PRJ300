using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    int playerCount = 1, dealerCount = 1;

    //Specific Y values for the player and dealer
    float playerY = -3.32f, dealerY = 3.1f;

    //Specific X values for each of the cards
    float[] columns = { -7f, -3.5f, 0f, 3.5f, 7f };

    [SerializeField] List<Cards> cards = new List<Cards>();

    [SerializeField] List<Cards> playerHand = new List<Cards>();
    [SerializeField] List<Cards> dealerHand = new List<Cards>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Hit();
        }
    }

    private void Start()
    {
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

    private void SetDealerHand(int i)
    {
        playerHand.Add(cards[i]);
    }

    private void SetPlayerHand(int i)
    {
        dealerHand.Add(cards[i]);
    }

    public void Hit()
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
        if (player) Instantiate(playerHand[i].face, new Vector2(columns[i], playerY), Quaternion.identity);
        
        else Instantiate(dealerHand[i].face, new Vector2(columns[i], dealerY), Quaternion.identity);
    }
}
