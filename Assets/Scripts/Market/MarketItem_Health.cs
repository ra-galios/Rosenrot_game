
public class MarketItem_Health : MarketItem {
	override public int GetItemCount()
	{
		return Market.Instance.Health;
	}

	override public void SetItemCount(int plusValue)
	{
		if(checkPurchase(Market.Instance.Health, plusValue))
		{
			Market.Instance.Health += plusValue;
		}
	}

	
	// Update is called once per frame
	void Update () {
		if(previewValue != Market.Instance.Health)
		{
			previewValue = GetItemCount();
			m_ItemText.text = previewValue.ToString();
		}
	}
}
