using UnityEngine;
using System.Collections;
using System;

public class DateManager : MonoBehaviour {
	public enum DateType 
	{year, month, day, hour, minutes, seconds};

	// Use this for initialization
	void Start () {
		//print(HowTimePassed(GetPlayerDate("Test"), DateType.seconds));

		//PlayerPrefs.SetString("Test", GetCurrentDateString());
	}


	public int HowTimePassed(string prevDate, DateType returnType){
		int passed = 0;
		int[] pDate = GetDateArray(prevDate);
		if(pDate != null && pDate.Length == 6){

			DateTime centuryBegin = new DateTime(pDate[0], pDate[1], pDate[2], pDate[3], pDate[4], pDate[5]);
			DateTime currentDate = DateTime.Now;

			long elapsedTicks = currentDate.Ticks - centuryBegin.Ticks;
			TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);

			switch (returnType){
				case DateType.year:
					passed = (int)elapsedSpan.TotalDays / 365;//It is not considered a leap year
					break;
				case DateType.month:
					passed = (int)((float)elapsedSpan.TotalDays / 30.5f);//the mean value of 30.5 days
					break;
				case DateType.day:
					passed = (int)elapsedSpan.TotalDays;
					break;
				case DateType.hour:
					passed = (int)elapsedSpan.TotalHours;
					break;
				case DateType.minutes:
					passed = (int)elapsedSpan.TotalMinutes;
					break;
				case DateType.seconds:
					passed = (int)elapsedSpan.TotalSeconds;
					break;
			}
		}
		return passed;
	}


	public string GetPlayerDate(string pref){
		return PlayerPrefs.GetString(pref, "");
	}

    public void SetDate(string pref, string value)
    {
        PlayerPrefs.SetString(pref, value);
    }
	
	// Update is called once per frame
	public string GetCurrentDateString() 
	{
		string dateString = "0000.00.00.00.00.00";

		int year = DateTime.Now.Year;
		int month = DateTime.Now.Month;
        int day = DateTime.Now.Day;
        int hour = DateTime.Now.Hour;
        int minute = DateTime.Now.Minute;
		int second = DateTime.Now.Second;

		dateString = year + "." + month + "." + day + "." + hour + "." + minute + "." + second;
		return dateString;
	}

	int[] GetDateArray(string date)
	{
		int[] array = null;
		char[] splitchar = { '.' };
		string[] newArray = date.Split(splitchar);
		if(newArray.Length == 6)
		{
			array = new int[6];
			for (int i=0; i<newArray.Length; i++)
			{
				int output = 0;
				if(int.TryParse(newArray[i], out output))
				{
					array[i] = output;
				}					
				else
				{
					array = null;
					break;
				}
			}
		}
		return array;
	}


}
