using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//хранение ачивок
//если 0 - не бралась ачика
//больше 0 - значение ачивки
//меньше 0 - ачивка получена

public static class AchievementsController 
{
	public enum Type 
	{
		//ачивки без уровня
		GoodStart = 0,
		RockyRoad = 1,
		DoingGreyt = 2,
		LetItBee = 3,
		AbyssGazesIntoYou = 4,
		BeOurGuest = 5,
		//ачивки по уровням - на каждую заводится по 3 номера
		AlwaysRight = 6,
		ExtremeLeft = 9,
		SelfDestructive = 12,
		HeartBreaker = 15,
		SurvivorFinished = 18,
		EverybodysBestFriend = 21,
		RubyRubyRubyRuby = 24,
		JacobAndTheBeanstalk = 27,
		ExplosiveBehavior = 30,
		Catchy = 33,
		BigBang = 36,
		NeverGiveUp = 39,
		GotHigh = 42,
		Adept = 45,
	}

	public static void AddToAchievement(Type type, int addValue)
	{
		int val = GetAchievement(type);
		DataManager.Instance.SetAchievement((int)type, val + addValue);
	}

	public static void CompleteAchievement(Type type)
	{
		DataManager.Instance.SetAchievement((int)type, -1);
	}

	public static int GetAchievement(Type type, int level = 0)
	{
		return DataManager.Instance.GetAchievement((int)type + level);
	}

	public static bool CheckAchievement(Type type, int currentValue)
	{
		//возвращает true только если ачивка была взята в первый раз
		int val = GetAchievement(type);
		if(val > 0 && currentValue >= val)
		{
			CompleteAchievement(type);
			return true;
		}
		return false;
	}

	public static bool CheckAchievementComplete(Type type, int level = 0)
	{
		if(GetAchievement(type, level) < 0)
		{
			return true;
		}
		return false;
	}

	public static void GetRevard(Type type, out int revard)
	{
		revard = 0;		
	}

	public static void GetRevard(Type type, out int[] revards)
	{
		revards = new int[3] {0, 0, 0};		
	}
}
