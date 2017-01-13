public class MarketItem_Seeds : MarketItem {
	override public int GetItemCount()
	{
		return Market.Instance.Seeds;
	}

	override public void SetItemCount(int plusValue)
	{
		if(checkPurchase(Market.Instance.Seeds, plusValue))
		{
			Market.Instance.Seeds += plusValue;
		}
	}

	
	// Update is called once per frame
	void Update () {
		if(previewValue != Market.Instance.Seeds)
		{
			previewValue = GetItemCount();
			m_ItemText.text = previewValue.ToString();
		}
	}
}
