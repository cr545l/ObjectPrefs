using UnityEngine;
using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

namespace Lofle
{
	/// <summary>
	/// 해당 클래스를 상속 받으면 직렬화 처리 기능을 가진다
	/// <para>상속자는 [Serializable]를 사용하여야 함</para>
	/// </summary>
	[Serializable]
	public class Data
	{
		public static T Parse<T>( string data ) where T : Data
		{
			//T create = Serializer.ToObject<T>( data );
			T create = JsonUtility.FromJson<T>( data );

			if( null == create )
			{
				Debug.Log( "JSON 데이터 파싱실패" );
			}

			return create;
		}

		public override string ToString()
		{
			return JsonUtility.ToJson( this, true );
		}
	}

	/// <summary>
	/// BASE64로 직렬화, 역직렬화 처리
	/// <pase>[Serializable]로 처리된 클래스만 가능</pase>
	/// </summary>
	public class Serializer
	{
		private static BinaryFormatter _binaryFormatter = new BinaryFormatter();

		/// <summary>
		/// 오브젝트를 문자열로 직렬화
		/// </summary>
		/// <param name="target">대상</param>
		/// <returns>직렬화 결과</returns>
		public static string ToString( object target )
		{
			if( null == target )
			{
				return null;
			}

			MemoryStream memoryStream = new MemoryStream();
			_binaryFormatter.Serialize( memoryStream, target );
			return System.Convert.ToBase64String( memoryStream.ToArray() );
		}

		/// <summary>
		/// 문자열을 오브젝트로 역직렬화
		/// </summary>
		/// <param name="target">대상</param>
		/// <returns>역직렬화 결과</returns>
		public static T ToObject<T>( string target )
		{
			try
			{
				if( null == target )
				{
					return default( T );
				}

				MemoryStream dataStream = new MemoryStream( System.Convert.FromBase64String( target ) );
				return (T)_binaryFormatter.Deserialize( dataStream );
			}
			catch( Exception e )
			{
				return default( T );
			}
		}
	}

	public class ObjectPrefs
	{
		public static void Set<T>( string key, T serializableObject )
		{
			if( serializableObject is ValueType )
			{
				SetSerialize<T>( key, serializableObject );
			}
			else
			{
				SetJson<T>( key, serializableObject );
			}
		}

		public static void SetJson<T>( string key, T serializableObject )
		{
			string data = JsonUtility.ToJson( serializableObject );
			PlayerPrefs.SetString( key, data );
		}

		public static void SetSerialize<T>( string key, T serializableObject )
		{
			string data = Serializer.ToString( serializableObject );
			PlayerPrefs.SetString( key, data );
		}

		public static T Get<T>( string key, T init = default( T ) )
		{
			if( init is ValueType )
			{
				return GetSerialize<T>( key, init );
			}
			else
			{
				return GetJson<T>( key, init );
			}
		}

		public static T GetJson<T>( string key, T init = default( T ) )
		{
			T result = default( T );

			if( !PlayerPrefs.HasKey( key ) )
			{
				Debug.LogWarningFormat( "저장된 데이터에서 해당 값을 찾지 못함 {0} {1}", typeof( T ).Name, key );
				result = init;
				SetJson( key, result );
				return result;
			}

			string data = PlayerPrefs.GetString( key, String.Empty );

			if( data == String.Empty )
			{
				Debug.LogWarningFormat( "저장된 데이터가 비어있음 {0} {1}", typeof( T ).Name, key );
				result = init;
				SetJson( key, result );
				return result;
			}

			try
			{
				result = JsonUtility.FromJson<T>( data );
			}
			catch( Exception e )
			{
				Debug.LogWarningFormat( "데이터 파싱 실패 {0} {1}", key, e.Message );
			}

			return result;
		}

		public static T GetSerialize<T>( string key, T init = default( T ) )
		{
			T result = default( T );

			if( !PlayerPrefs.HasKey( key ) )
			{
				Debug.LogWarningFormat( "저장된 데이터에서 해당 값을 찾지 못함 {0} {1}", typeof( T ).Name, key );
				result = init;
				SetSerialize( key, result );
				return result;
			}

			string data = PlayerPrefs.GetString( key, String.Empty );

			if( data == String.Empty )
			{
				Debug.LogWarningFormat( "저장된 데이터가 비어있음 {0} {1}", typeof( T ).Name, key );
				result = init;
				SetSerialize( key, result );
				return result;
			}

			try
			{
				result = Serializer.ToObject<T>( data );
			}
			catch( Exception e )
			{
				Debug.LogWarningFormat( "데이터 파싱 실패 {0} {1}", key, e.Message );
			}

			return result;
		}

		public static void Remove( string key )
		{
			PlayerPrefs.DeleteKey( key );
		}

		public static void Reset()
		{
			PlayerPrefs.DeleteAll();
		}

		public static void Save()
		{
			PlayerPrefs.Save();
		}
	}
}