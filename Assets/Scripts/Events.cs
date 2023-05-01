using System;

public class Events
{
    public static Action<CompType, int> UpdateItemComponent;

    public static Action<string> PlayMusic;
    public static Action<string> PlaySound;
    public static Action StopMusic;

	public static Action CustomerLeft;
	public static Action SpillCleaned;
	public static Action FireExtinguished;

	public static Action EndGame;
	public static Action<int> AddScore;
}
