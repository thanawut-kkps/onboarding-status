﻿using System.Reflection;
using Phatra.Core.CsvHelper.TypeConversion;

namespace Phatra.Core.CsvHelper.Configuration
{
	/// <summary>
	/// Options used when auto mapping.
	/// </summary>
    public class AutoMapOptions
    {
		/// <summary>
		/// Gets or sets a value indicating whether references
		/// should be ignored when auto mapping. True to ignore
		/// references, otherwise false. Default is false.
		/// </summary>
		public bool IgnoreReferences { get; set; }

		/// <summary>
		/// Gets or sets a value indicating if headers of reference
		/// properties/fields should get prefixed by the parent property/field
		/// name when automapping.
		/// True to prefix, otherwise false. Default is false.
		/// </summary>
		public bool PrefixReferenceHeaders { get; set; }

		/// <summary>
		/// Gets or sets a value indicating if private
		/// properties/fields should be read from and written to.
		/// True to include private properties/fields, otherwise false. Default is false.
		/// </summary>
		public bool IncludePrivateProperties { get; set; }

		/// <summary>
		/// Gets or sets the member types that are used when auto mapping.
		/// MemberTypes are flags, so you can choose more than one.
		/// Default is Properties.
		/// </summary>
		public MemberTypes MemberTypes { get; set; } = MemberTypes.Properties;

		/// <summary>
		/// Gets or sets the <see cref="TypeConverterOptionsFactory"/>.
		/// </summary>
		public TypeConverterOptionsFactory TypeConverterOptionsFactory { get; set; } = new TypeConverterOptionsFactory();

		/// <summary>
		/// Create options using the defaults.
		/// </summary>
		public AutoMapOptions() { }

		/// <summary>
		/// Creates options using the given <see cref="CsvConfiguration"/>.
		/// </summary>
		/// <param name="configuration"></param>
	    public AutoMapOptions( CsvConfiguration configuration )
	    {
		    IgnoreReferences = configuration.IgnoreReferences;
		    PrefixReferenceHeaders = configuration.PrefixReferenceHeaders;
		    IncludePrivateProperties = configuration.IncludePrivateMembers;
		    MemberTypes = configuration.MemberTypes;
			TypeConverterOptionsFactory = configuration.TypeConverterOptionsFactory;
	    }

		/// <summary>
		/// Creates a copy of the auto map options.
		/// </summary>
		/// <returns>A copy of the auto map options.</returns>
		public AutoMapOptions Copy()
		{
			var copy = new AutoMapOptions();
			var properties = GetType().GetProperties( BindingFlags.Public | BindingFlags.Instance );
			foreach( var property in properties )
			{
				property.SetValue( copy, property.GetValue( this, null ), null );
			}

			return copy;
		}
    }
}
