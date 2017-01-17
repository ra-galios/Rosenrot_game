using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AchievementsController 
{
	public enum Type 
	{
		GoodStart = 0,
		RockyRoad = 1,
		DoingGreyt = 2,
		LetItBee = 3,
		AbyssGazesIntoYou = 4,
		AlwaysRight = 5,
		ExtremeLeft = 6,
		SelfDestructive = 7,
		HeartBreaker = 8,
		SurvivorFinished = 9,
		EverybodysBestFriend = 10,
		RubyRubyRubyRuby = 11,
		JacobAndTheBeanstalk = 12,
		ExplosiveBehavior = 13,
		Catchy = 14,
		BigBang = 15,
		NeverGiveUp = 16,
		GotHigh = 17,
		Adept = 18,
		BeOurGuest = 19,

	}

	public static void AddToAchievement(Type type, int value)
	{

	}

	public static int GetAchievement(Type type)
	{
		return 0;
	}

	public static bool CheckAchievement(Type type)
	{

		return false;
	}
}
