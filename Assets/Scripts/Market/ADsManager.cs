using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public class ADsManager : MonoBehaviour 
{
	[SerializeField]
	private string m_gameID;

	[SerializeField]
	private bool m_enableTestMode;

	private Coroutine ads;

	public void ViewAds()
	{
		if(ads == null)
			ads = StartCoroutine(StartAds());
	}

	IEnumerator StartAds ()
    {
        // Wait until Unity Ads is initialized,
        //  and the default ad placement is ready.
        while (!Advertisement.isInitialized || !Advertisement.IsReady()) {
            yield return new WaitForSeconds(0.5f);
        }

        // Show the default ad placement.
        Advertisement.Show();
		ads = null;
		Market.Instance.AddHealthAds(1);
    }
}
