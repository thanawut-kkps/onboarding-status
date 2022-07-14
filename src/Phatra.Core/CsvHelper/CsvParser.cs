using System;
using System.IO;
using Phatra.Core.CsvHelper.Configuration;

namespace Phatra.Core.CsvHelper
{
    /// <summary>
    /// Parses a CSV file.
    /// </summary>
    public class CsvParser : ICsvParser
    {
		private readonly bool _leaveOpen;
		private readonly RecordBuilder _record = new RecordBuilder();
		private FieldReader _reader;
		private bool _disposed;
	    private int _currentRow;
	    private int _currentRawRow;
	    private int _c = -1;
	    private bool _hasExcelSeparatorBeenRead;
		private readonly ICsvParserConfiguration _configuration;

		/// <summary>
		/// Gets the <see cref="ICsvParser.TextReader"/>.
		/// </summary>
		public virtual TextReader TextReader => _reader.Reader;

		/// <summary>
		/// Gets the configuration.
		/// </summary>
		public virtual ICsvParserConfiguration Configuration => _configuration;

		/// <summary>
		/// Gets the character position that the parser is currently on.
		/// </summary>
		public virtual long CharPosition => _reader.CharPosition;

		/// <summary>
		/// Gets the byte position that the parser is currently on.
		/// </summary>
		public virtual long BytePosition => _reader.BytePosition;

	    /// <summary>
	    /// Gets the row of the CSV file that the parser is currently on.
	    /// </summary>
	    public virtual int Row => _currentRow;

	    /// <summary>
	    /// Gets the row of the CSV file that the parser is currently on.
	    /// This is the actual file row.
	    /// </summary>
	    public virtual int RawRow => _currentRawRow;

	    /// <summary>
	    /// Gets the raw row for the current record that was parsed.
	    /// </summary>
	    public virtual string RawRecord => _reader.RawRecord;

        /// <summary>
        /// Creates a new parser using the given <see cref="TextReader" />.
        /// </summary>
        /// <param name="reader">The <see cref="TextReader" /> with the CSV file data.</param>
        public CsvParser(TextReader reader) : this(reader, new CsvConfiguration(), false) { }

		/// <summary>
		/// Creates a new parser using the given <see cref="TextReader" />.
		/// </summary>
		/// <param name="reader">The <see cref="TextReader" /> with the CSV file data.</param>
		/// <param name="leaveOpen">true to leave the reader open after the CsvReader object is disposed, otherwise false.</param>
		public CsvParser( TextReader reader, bool leaveOpen ) : this( reader, new CsvConfiguration(), leaveOpen) { }

        /// <summary>
        /// Creates a new parser using the given <see cref="TextReader"/> and <see cref="CsvConfiguration"/>.
        /// </summary>
        /// <param name="reader">The <see cref="TextReader"/> with the CSV file data.</param>
        /// <param name="configuration">The configuration.</param>
        public CsvParser(TextReader reader, ICsvParserConfiguration configuration) : this(reader, configuration, false) { }

        /// <summary>
        /// Creates a new parser using the given <see cref="TextReader"/> and <see cref="CsvConfiguration"/>.
        /// </summary>
        /// <param name="reader">The <see cref="TextReader"/> with the CSV file data.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="leaveOpen">true to leave the reader open after the CsvReader object is disposed, otherwise false.</param>
        public CsvParser(TextReader reader, ICsvParserConfiguration configuration, bool leaveOpen)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

			_reader = new FieldReader( reader, configuration );
			_configuration = configuration;
			_leaveOpen = leaveOpen;
		}

		/// <summary>
		/// Reads a record from the CSV file.
		/// </summary>
		/// <returns>A <see cref="T:String[]" /> of fields for the record read.</returns>
		public virtual string[] Read()
		{
			try
			{
				if( _configuration.HasExcelSeparator && !_hasExcelSeparatorBeenRead )
				{
					ReadExcelSeparator();
				}

				_reader.ClearRawRecord();

                var row = ReadLine();

				return row;
			}
			catch( Exception ex )
			{
				var csvHelperException = ex as CsvHelperException ?? new CsvParserException( "An unexpected error occurred.", ex );
				ExceptionHelper.AddExceptionData( csvHelperException, Row, null, null, null, _record.ToArray() );

                throw csvHelperException;
            }
        }

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		public virtual void Dispose()
		{
			Dispose( !_leaveOpen );
			GC.SuppressFinalize( this );
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <param name="disposing">True if the instance needs to be disposed of.</param>
		protected virtual void Dispose( bool disposing )
		{
			if( _disposed )
			{
				return;
			}

			if( disposing )
			{
				_reader?.Dispose();
			}

			_disposed = true;
			_reader = null;
		}

		/// <summary>
		/// Reads a line of the CSV file.
		/// </summary>
		/// <returns>The CSV line.</returns>
	    protected virtual string[] ReadLine()
	    {
		    _record.Clear();
		    _currentRow++;
		    _currentRawRow++;

			while( true )
			{
				_c = _reader.GetChar();

			    if( _c == -1 )
			    {
					// We have reached the end of the file.
					if( _record.Length > 0 )
				    {
						// There was no line break at the end of the file.
						// We need to return the last record first.
						_record.Add( _reader.GetField() );
						return _record.ToArray();
					}

                    return null;
                }

				if( _configuration.UseExcelLeadingZerosFormatForNumerics )
				{
					if( ReadExcelLeadingZerosField() )
					{
						break;
					}

                    continue;
                }

			    if( _record.Length == 0 && ( ( _c == _configuration.Comment && _configuration.AllowComments ) || _c == '\r' || _c == '\n' ) )
			    {
				    ReadBlankLine();
				    if( !_configuration.IgnoreBlankLines )
				    {
						break;
				    }

                    continue;
                }

				if( _c == _configuration.Quote && !_configuration.IgnoreQuotes )
			    {
				    if( ReadQuotedField() )
				    {
						break;
				    }
			    }
			    else
			    {
				    if( ReadField() )
				    {
					    break;
				    }
			    }
			}

			return _record.ToArray();
	    }

		/// <summary>
		/// Reads a blank line. This accounts for empty lines
		/// and commented out lines.
		/// </summary>
	    protected virtual void ReadBlankLine()
	    {
			if( _configuration.IgnoreBlankLines )
			{
				_currentRow++;
			}

			while( true )
		    {
			    if( _c == '\r' || _c == '\n' )
			    {
				    ReadLineEnding();
				    _reader.SetFieldStart();
					return;
			    }

			    if( _c == -1 )
			    {
				    return;
			    }

				// If the buffer runs, it appends the current data to the field.
				// We don't want to capture any data on a blank line, so we
				// need to set the field start every char.
			    _reader.SetFieldStart();
				_c = _reader.GetChar();
		    }
	    }

		/// <summary>
		/// Reads until a delimiter or line ending is found.
		/// </summary>
		/// <returns>True if the end of the line was found, otherwise false.</returns>
	    protected virtual bool ReadField()
		{
			if( _c != _configuration.Delimiter[0] && _c != '\r' && _c != '\n' )
			{
				_c = _reader.GetChar();
			}

			while( true )
			{
				if( _c == _configuration.Quote )
				{
					_reader.IsFieldBad = true;
				}

				if( _c == _configuration.Delimiter[0] )
				{
					_reader.SetFieldEnd( -1 );

					// End of field.
					if( ReadDelimiter() )
					{
						// Set the end of the field to the char before the delimiter.
						_record.Add( _reader.GetField() );
						return false;
					}
				}
				else if( _c == '\r' || _c == '\n' )
				{
					// End of line.
					_reader.SetFieldEnd( -1 );
					var offset = ReadLineEnding();
					_reader.SetRawRecordEnd( offset );
					_record.Add( _reader.GetField() );

					_reader.SetFieldStart( offset );

					return true;
				}
				else if( _c == -1 )
				{
					// End of file.
					_reader.SetFieldEnd();
					_record.Add( _reader.GetField() );
					return true;
				}

				_c = _reader.GetChar();
			}
		}

		/// <summary>
		/// Reads until the field is not quoted and a delimeter is found.
		/// </summary>
		/// <returns>True if the end of the line was found, otherwise false.</returns>
		protected virtual bool ReadQuotedField()
		{
			var inQuotes = true;
			// Set the start of the field to after the quote.
			_reader.SetFieldStart();

            while (true)
            {
                // 1,"2" ,3

				var cPrev = _c;
				_c = _reader.GetChar();
				if( _c == _configuration.Quote )
				{
					inQuotes = !inQuotes;

					if( !inQuotes )
					{
						// Add an offset for the quote.
						_reader.SetFieldEnd( -1 );
						_reader.AppendField();
						_reader.SetFieldStart();
					}

                    continue;
                }

				if( inQuotes )
				{
					if( _c == '\r' || _c == '\n' )
					{
						ReadLineEnding();
						_currentRawRow++;
					}

					if( _c == -1 )
					{
						_reader.SetFieldEnd();
						_record.Add( _reader.GetField() );
						return true;
					}
				}

				if( !inQuotes )
				{
					if( _c == _configuration.Delimiter[0] )
					{
						_reader.SetFieldEnd( -1 );

						if( ReadDelimiter() )
						{
							// Add an extra offset because of the end quote.
							_record.Add( _reader.GetField() );
							return false;
						}
					}
					else if( _c == '\r' || _c == '\n' )
					{
						_reader.SetFieldEnd( -1 );
						var offset = ReadLineEnding();
						_reader.SetRawRecordEnd( offset );
						_record.Add( _reader.GetField() );
						_reader.SetFieldStart( offset );
						return true;
					}
					else if( cPrev == _configuration.Quote )
					{
						// We're out of quotes. Read the reset of
						// the field like a normal field.
						return ReadField();
					}
				}
			}
		}

		/// <summary>
		/// Reads the field using Excel leading zero compatibility.
		/// i.e. Fields that start with `=`.
		/// </summary>
		/// <returns></returns>
	    protected virtual bool ReadExcelLeadingZerosField()
	    {
			if( _c == '=' )
			{
				_c = _reader.GetChar();
				if( _c == _configuration.Quote && !_configuration.IgnoreQuotes )
				{
					// This is a valid Excel formula.
					return ReadQuotedField();
				}
			}

            // The format is invalid.
            // Excel isn't consistent, so just read as normal.

			if( _c == _configuration.Quote && !_configuration.IgnoreQuotes )
			{
				return ReadQuotedField();
			}

            return ReadField();
        }

	    /// <summary>
		/// Reads until the delimeter is done.
		/// </summary>
		/// <returns>True if a delimiter was read. False if the sequence of
		/// chars ended up not being the delimiter.</returns>
	    protected virtual bool ReadDelimiter()
	    {
			if( _c != _configuration.Delimiter[0] )
			{
				throw new InvalidOperationException( "Tried reading a delimiter when the first delimiter char didn't match the current char." );
			}

			if( _configuration.Delimiter.Length == 1 )
			{
				return true;
			}

			for( var i = 1; i < _configuration.Delimiter.Length; i++ )
			{
				_c = _reader.GetChar();
				if( _c != _configuration.Delimiter[i] )
				{
					return false;
				}
			}

            return true;
        }

		/// <summary>
		/// Reads until the line ending is done.
		/// </summary>
		/// <returns>True if more chars were read, otherwise false.</returns>
	    protected virtual int ReadLineEnding()
		{
			var fieldStartOffset = 0;
		    if( _c == '\r' )
		    {
				_c = _reader.GetChar();
				if( _c != '\n' )
				{
					// The start needs to be moved back.
					fieldStartOffset--;
			    }
		    }

            return fieldStartOffset;
        }

		/// <summary>
		/// Reads the Excel seperator and sets it to the delimiter.
		/// </summary>
		protected virtual void ReadExcelSeparator()
		{
			// sep=delimiter
			var sepLine = _reader.Reader.ReadLine();
			if( sepLine != null )
			{
				_configuration.Delimiter = sepLine.Substring( 4 );
			}

			_hasExcelSeparatorBeenRead = true;
		}
	}
}
