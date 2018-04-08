using Lofle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sample : MonoBehaviour
{
	private void OnGUI()
	{
		GUILayout.Label( string.Format( "Level {0}", ObjectPrefs.Get<int>( "level" ) ) );

		if( GUILayout.Button( "set level 10" ) )
		{
			ObjectPrefs.Set( "level", 10 );
			ObjectPrefs.Save();
		}

		if( GUILayout.Button( "set level 20" ) )
		{
			ObjectPrefs.Set( "level", 20 );
			ObjectPrefs.Save();
		}

		if( GUILayout.Button( "set level 30" ) )
		{
			ObjectPrefs.Set( "level", 30 );
			ObjectPrefs.Save();
		}
	}
}
