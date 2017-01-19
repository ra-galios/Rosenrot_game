public class MarketItem_Bomb : MarketItem {
	override public int GetItemCount()
	{
		return Market.Instance.Bomb;
	}

	override public void SetItemCount(int plusValue)
	{
		if(checkPurchase(Market.Instance.Bomb, plusValue))
		{
			Market.Instance.Bomb += plusValue;
		}
	}

	
	// Update is called once per frame
	void Update () {
		if(m_ItemText && previewValue != Market.Instance.Bomb)
		{
			previewValue = GetItemCount();
			m_ItemText.text = previewValue.ToString();
		}
	}	
}
