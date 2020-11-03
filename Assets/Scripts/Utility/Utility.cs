using System;

#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif

public static class Utility {
	public static void AskForPermission(string permission) {
		#if PLATFORM_ANDROID
		Permission.RequestUserPermission(permission);
		#endif
	}

	public static DateTime UnixTimeStampToDateTime(double unixTimeStamp) {
		System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
		dtDateTime = dtDateTime.AddSeconds(unixTimeStamp);
		dtDateTime = dtDateTime.AddHours(1);	// gmt +2 - Winterzeit
		return dtDateTime;
	}
}