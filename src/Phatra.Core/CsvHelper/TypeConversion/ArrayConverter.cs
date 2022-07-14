﻿using System;
using System.Collections.Generic;
using System.Linq;
using Phatra.Core.CsvHelper.Configuration;

namespace Phatra.Core.CsvHelper.TypeConversion
{
	/// <summary>
	/// Converts an <see cref="Array"/> to and from a <see cref="string"/>.
	/// </summary>
    public class ArrayConverter : IEnumerableConverter
    {
		/// <summary>
		/// Converts the string to an object.
		/// </summary>
		/// <param name="text">The string to convert to an object.</param>
		/// <param name="row">The <see cref="ICsvReaderRow"/> for the current record.</param>
		/// <param name="propertyMapData">The <see cref="CsvPropertyMapData"/> for the property/field being created.</param>
		/// <returns>The object created from the string.</returns>
		public override object ConvertFromString( string text, ICsvReaderRow row, CsvPropertyMapData propertyMapData )
		{
			Array array;
			var type = propertyMapData.Member.MemberType().GetElementType();

			if( propertyMapData.IsNameSet || row.Configuration.HasHeaderRecord && !propertyMapData.IsIndexSet )
			{
				// Use the name.
				var list = new List<object>();
				var nameIndex = 0;
				while( true )
				{
					object field;
					if( !row.TryGetField( type, propertyMapData.Names.FirstOrDefault(), nameIndex, out field ) )
					{
						break;
					}

					list.Add( field );
					nameIndex++;
				}

				array = (Array)ReflectionHelper.CreateInstance( propertyMapData.Member.MemberType(), list.Count );
				for( var i = 0; i < list.Count; i++ )
				{
					array.SetValue( list[i], i );
				}
			}
			else
			{
				// Use the index.
				var indexEnd = propertyMapData.IndexEnd < propertyMapData.Index
					? row.CurrentRecord.Length - 1
					: propertyMapData.IndexEnd;

				var arraySize = indexEnd - propertyMapData.Index + 1;
				array = (Array)ReflectionHelper.CreateInstance( propertyMapData.Member.MemberType(), arraySize );
				var arrayIndex = 0;
				for( var i = propertyMapData.Index; i <= indexEnd; i++ )
				{
					array.SetValue( row.GetField( type, i ), arrayIndex );
					arrayIndex++;
				}
			}

			return array;
		}
	}
}
