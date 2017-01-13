using UnityEngine;
using UnityEngine.UI;

public class MarketItem : MonoBehaviour 
{
	[SerializeField]
	protected Text m_ItemText;

	[SerializeField]
	private MarketItem m_MarketItemPrice;

	[SerializeField]
	private int m_Cost;

	protected int previewValue;

	protected void Start()
	{
		previewValue = GetItemCount();
		if(m_ItemText)
			m_ItemText.text = previewValue.ToString();
	}

	public void ByItem(int value)
	{	
		if(value > 0)
		{
			if(m_MarketItemPrice)
			{
				if(checkPurchase(m_MarketItemPrice.GetItemCount(), -m_Cost * value))
				{
					SetItemCount(value);
					m_MarketItemPrice.SpendItem(m_Cost * value);
				} 
			}
			else
				SetItemCount(value);
		}
			
	}

	public void SpendItem(int value)
	{	
		if(value > 0)
			SetItemCount(-value);
	}


	virtual public int GetItemCount()
	{
		//все поведение определяется в классаъ наследниках
		return 0;
	}	

	virtual public void SetItemCount(int plusValue)
	{	
		//все поведение определяется в классаъ наследниках
	}

	protected bool checkPurchase(int marketValue, int value)	
	{
		if(marketValue + value >= 0)
			return true;
		else
			return false;
	}
}


