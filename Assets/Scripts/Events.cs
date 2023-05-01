using System;

public class Events
{
    public static Action<CompType, int> UpdateItemComponent;

    public static Action<string> PlayMusic;
    public static Action<string> PlaySound;
    public static Action StopMusic;

	public static Action CustomerLeft;

}
