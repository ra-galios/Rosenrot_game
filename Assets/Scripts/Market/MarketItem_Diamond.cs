public class MarketItem_Diamond : MarketItem {
	override public int GetItemCount()
	{
		return Market.Instance.Dimond;
	}

	override public void SetItemCount(int plusValue)
	{
		if(checkPurchase(Market.Instance.Dimond, plusValue))
		{
			Market.Instance.Dimond += plusValue;
		}
	}

	
	// Update is called once per frame
	void Update () {
		if(previewValue != Market.Instance.Dimond)
		{
			previewValue = GetItemCount();
			m_ItemText.text = previewValue.ToString();
		}
	}
}
