using UnityEngine;

public class BookPickUpManager : MonoBehaviour
{
    [Header("Pickup Reference")]
    [SerializeField] GameObject bookPickupInScene;

    [Header("Book on Player")]
    [SerializeField] GameObject objectOnPlayer;
    [SerializeField] BookController bookScript;
    [SerializeField] GameObject[] pieces;
    public int pagesShown;

    private bool bookPickedUp = false;

    public void SetBookPickedUp(bool pickedUp)
    {
        bookPickedUp = pickedUp;
    }

    public void SetPieces(int pieces)
    {
        pagesShown = pieces;
        Debug.Log(pagesShown);
    }
    public bool IsBookPickedUp()
    {
        return bookPickedUp;
    }

    public int ShownPieces()
    {
        return pagesShown;
    }

    public void ApplyBookState()
    {
        if (bookPickedUp)
        {
            if (bookPickupInScene != null)
                Destroy(bookPickupInScene);

            if (objectOnPlayer != null)
                objectOnPlayer.SetActive(true);

            if (bookScript != null)
                bookScript.enabled = true;
            if(bookScript != null)
                bookScript.gotbook = true;

            if (pagesShown == 1)
            {
                pieces[0].SetActive(true);
            }
            if (pagesShown == 2)
            {
                pieces[0].SetActive(true);
                pieces[1].SetActive(true);
            }
            if (pagesShown == 3)
            {
                pieces[0].SetActive(true);
                pieces[1].SetActive(true);
                pieces[2].SetActive(true);
            }
            if (pagesShown == 4)
            {
                pieces[0].SetActive(true);
                pieces[1].SetActive(true);
                pieces[2].SetActive(true);
                pieces[3].SetActive(true);
            }
            if (pagesShown == 0)
            {
                pieces[0].SetActive(false);
                pieces[1].SetActive(false);
                pieces[2].SetActive(false);
                pieces[3].SetActive(false);
            }

        }
        else
        {
            if (bookPickupInScene != null)
                bookPickupInScene.SetActive(true);

            if (objectOnPlayer != null)
                objectOnPlayer.SetActive(false);

            if (bookScript != null)
                bookScript.enabled = false;
            if (bookScript != null)
                bookScript.gotbook = false;
            if (pagesShown == 0)
            {
                pieces[0].SetActive(false);
                pieces[1].SetActive(false);
                pieces[2].SetActive(false);
                pieces[3].SetActive(false);
            }
        }
    }
}
