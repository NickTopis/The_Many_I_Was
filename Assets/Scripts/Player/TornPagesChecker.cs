using UnityEngine;

public class TornPagesChecker : MonoBehaviour
{
    [SerializeField] GameObject[] pages;
    [SerializeField] GameObject[] pieces;
    [SerializeField] BookPickUpManager bookPickUpManager;
    private int pageid = 0;
    
    void Start()
    {
        pageid = bookPickUpManager.pagesShown;
        if (pageid == 0)
        {
            for (int i = 0; i < pages.Length; i++)
            {
                pages[i].SetActive(false);
                pieces[i].GetComponent<TornPagePiece>().enabled = false;
            }
        }
    }
    void Update()
    {

    }

    public void ShowPage()
    {
        pages[pageid].SetActive(true);
        pageid++;
        bookPickUpManager.pagesShown = pageid;

    }
}
