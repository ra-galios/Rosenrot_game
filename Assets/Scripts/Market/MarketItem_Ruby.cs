public class MarketItem_Ruby : MarketItem {
	override public int GetItemCount()
	{
		return Market.Instance.Ruby;
	}

	override public void SetItemCount(int plusValue)
	{
		if(checkPurchase(Market.Instance.Ruby, plusValue))
		{
			Market.Instance.Ruby += plusValue;
		}
	}

	
	// Update is called once per frame
	void Update () {
		if(m_ItemText && previewValue != Market.Instance.Ruby)
		{
			previewValue = GetItemCount();
			m_ItemText.text = previewValue.ToString();
		}
	}
}
