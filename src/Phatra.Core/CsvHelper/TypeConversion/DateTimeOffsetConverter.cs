﻿// Copyright 2009-2015 Josh Close and Contributors
// This file is a part of CsvHelper and is dual licensed under MS-PL and Apache 2.0.
// See LICENSE.txt for details or visit http://www.opensource.org/licenses/ms-pl.html for MS-PL and http://opensource.org/licenses/Apache-2.0 for Apache 2.0.
// http://csvhelper.com

using System;
using System.Globalization;
using Phatra.Core.CsvHelper.Configuration;

namespace Phatra.Core.CsvHelper.TypeConversion
{
	/// <summary>
	/// Converts a <see cref="DateTimeOffset"/> to and from a <see cref="string"/>.
	/// </summary>
	public class DateTimeOffsetConverter : DefaultTypeConverter
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
			if( text == null )
			{
				return base.ConvertFromString( null, row, propertyMapData );
			}

			var formatProvider = (IFormatProvider)propertyMapData.TypeConverterOptions.CultureInfo.GetFormat( typeof( DateTimeFormatInfo ) ) ?? propertyMapData.TypeConverterOptions.CultureInfo;
			var dateTimeStyle = propertyMapData.TypeConverterOptions.DateTimeStyle ?? DateTimeStyles.None;

			return string.IsNullOrEmpty( propertyMapData.TypeConverterOptions.Format )
				? DateTimeOffset.Parse( text, formatProvider, dateTimeStyle )
				: DateTimeOffset.ParseExact( text, propertyMapData.TypeConverterOptions.Format, formatProvider, dateTimeStyle );
		}
	}
}