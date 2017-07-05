using System;
using System.IO;
using System.Text;

namespace Business.Utility
{
	/// <summary>
	/// Customized trace class for logging.  See http://www.codeproject.com/KB/dotnet/customnettracelisteners.aspx.
	/// </summary>
	public class LogTrace : FileStream
	{
		#region Fields
		private bool canSplitData = true;
		private string fileBase = String.Empty;
		private int fileDecimals = 0;
		private string fileDirectory = String.Empty;
		private string fileExtension = String.Empty;
		private long maxFileLength = 0;
		private int maxFileCount = 0;
		private int nextFileIndex = 0;
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new instance of LogTrace.
		/// </summary>
		/// <param name="path">The storage path for the log file.</param>
		/// <param name="maxFileLength">The maximum size of the log file.</param>
		/// <param name="maxFileCount">The number of log files to create before overwriting existing log files.</param>
		/// <param name="mode">The file mode.</param>
		public LogTrace(string path, long maxFileLength, int maxFileCount, FileMode mode)
			: base(path, BaseFileMode(mode), FileAccess.Write)
		{
			Initialize(path, maxFileLength, maxFileCount, mode);
		}

		/// <summary>
		/// Creates a new instance of LogTrace.
		/// </summary>
		/// <param name="path">The storage path for the log file.</param>
		/// <param name="maxFileLength">The maximum size of the log file.</param>
		/// <param name="maxFileCount">The number of log files to create before overwriting existing log files.</param>
		/// <param name="mode">The file mode.</param>
		/// <param name="share">The file share.</param>
		public LogTrace(string path, long maxFileLength, int maxFileCount, FileMode mode, FileShare share)
			: base(path, BaseFileMode(mode), FileAccess.Write, share)
		{
			Initialize(path, maxFileLength, maxFileCount, mode);
		}

		/// <summary>
		/// Creates a new instance of LogTrace.
		/// </summary>
		/// <param name="path">The storage path for the log file.</param>
		/// <param name="maxFileLength">The maximum size of the log file.</param>
		/// <param name="maxFileCount">The number of log files to create before overwriting existing log files.</param>
		/// <param name="mode">The file mode.</param>
		/// <param name="share">The file share.</param>
		/// <param name="bufferSize">The internal buffer size.</param>
		public LogTrace(string path, long maxFileLength, int maxFileCount, FileMode mode, FileShare share, int bufferSize)
			: base(path, BaseFileMode(mode), FileAccess.Write, share, bufferSize)
		{
			Initialize(path, maxFileLength, maxFileCount, mode);
		}

		/// <summary>
		/// Creates a new instance of LogTrace.
		/// </summary>
		/// <param name="path">The storage path for the log file.</param>
		/// <param name="maxFileLength">The maximum size of the log file.</param>
		/// <param name="maxFileCount">The number of log files to create before overwriting existing log files.</param>
		/// <param name="mode">The file mode.</param>
		/// <param name="share">The file share.</param>
		/// <param name="bufferSize">The internal buffer size.</param>
		/// <param name="isAsync">Flag indicating whether the underlying file stream is asynchronous.</param>
		public LogTrace(string path, long maxFileLength, int maxFileCount, FileMode mode, FileShare share, int bufferSize, bool isAsync)
			: base(path, BaseFileMode(mode), FileAccess.Write, share, bufferSize, isAsync)
		{
			Initialize(path, maxFileLength, maxFileCount, mode);
		}
		#endregion

		#region Properties
		public override bool CanRead
		{
			get { return false; }
		}

		public bool CanSplitData 
		{ 
			get { return canSplitData; } 
			set { canSplitData = value; } 
		}

		public int MaxFileCount 
		{ 
			get { return maxFileCount; } 
		}

		public long MaxFileLength
		{
			get { return maxFileLength; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Saves the file data and starts a new file.
		/// </summary>
		private void BackUpAndResetStream()
		{
			Flush();
			File.Copy(Name, GetBackupFileName(this.nextFileIndex), true);
			SetLength(0);

			this.nextFileIndex++;
			if (this.nextFileIndex >= this.maxFileCount)
				this.nextFileIndex = 0;
		}

		/// <summary>
		/// Gets the file mode for the log file.
		/// </summary>
		/// <param name="mode">The file mode.</param>
		/// <returns>OpenOrCreate if the input is Append; otherwise returns the input.</returns>
		private static FileMode BaseFileMode(FileMode mode)
		{
			return mode == FileMode.Append ? FileMode.OpenOrCreate : mode;
		}

		/// <summary>
		/// Gets the next file name for the backup log file.
		/// </summary>
		/// <param name="index">The current index of the file.</param>
		/// <returns>The new file name.</returns>
		private string GetBackupFileName(int index)
		{
			StringBuilder format = new StringBuilder();
			format.AppendFormat("D{0}", this.fileDecimals);
			StringBuilder sb = new StringBuilder();
			if (this.fileExtension.Length > 0)
				sb.AppendFormat("{0}{1}{2}", this.fileBase, index.ToString(format.ToString()), this.fileExtension);
			else
				sb.AppendFormat("{0}{1}", this.fileBase, index.ToString(format.ToString()));
			return Path.Combine(this.fileDirectory, sb.ToString());
		}

		/// <summary>
		/// Sets up the log trace.
		/// </summary>
		/// <param name="path">The path to write to.</param>
		/// <param name="length">The length of each file.</param>
		/// <param name="count">The number of files to create.</param>
		/// <param name="mode">The file mode.</param>
		private void Initialize(string path, long length, int count, FileMode mode)
		{
			if (length <= 0)
				throw new ArgumentOutOfRangeException("Invalid maximum file length");
			if (count <= 0)
				throw new ArgumentOutOfRangeException("Invalid maximum file count");

			this.maxFileLength = length;
			this.maxFileCount = count;
			this.canSplitData = true;

			string fullPath = Path.GetFullPath(path);
			this.fileDirectory = Path.GetDirectoryName(fullPath);
			this.fileBase = Path.GetFileNameWithoutExtension(fullPath);
			this.fileExtension = Path.GetExtension(fullPath);

			this.fileDecimals = 1;
			int decimalBase = 10;
			while (decimalBase < this.maxFileCount)
			{
				this.fileDecimals++;
				decimalBase *= 10;
			}

			switch (mode)
			{
				case FileMode.Create:
				case FileMode.CreateNew:
				case FileMode.Truncate:
					// Delete old files
					for (int iFile = 0; iFile < this.maxFileCount; iFile++)
					{
						string file = GetBackupFileName(iFile);

						if (File.Exists(file))
							File.Delete(file);
					}
					break;

				default:
					// Position file pointer to the last backup file
					for (int iFile = 0; iFile < this.maxFileCount; iFile++)
					{
						if (File.Exists(GetBackupFileName(iFile)))
							this.nextFileIndex = iFile + 1;
					}

					if (this.nextFileIndex == this.maxFileCount)
						this.nextFileIndex = 0;

					Seek(0, SeekOrigin.End);
					break;
			}
		}

		/// <summary>
		/// Writes the current buffer to the log file.
		/// </summary>
		/// <param name="array">The data to write.</param>
		/// <param name="offset">The position in the data to start.</param>
		/// <param name="count">The number of bytes to write.</param>
		public override void Write(byte[] array, int offset, int count)
		{
			int actualCount = Math.Min(count, array.GetLength(0));

			if (Position + actualCount <= this.maxFileLength)
			{
				base.Write(array, offset, count);
			}
			else
			{
				if (this.canSplitData)
				{
					int partialCount = (int)(Math.Max(this.maxFileLength, Position) - Position);
					base.Write(array, offset, partialCount);
					offset += partialCount;
					count = actualCount - partialCount;
				}
				else
				{
					if (count > this.maxFileLength)
						throw new ArgumentOutOfRangeException("Buffer size exceeds maximum file length");
				}

				BackUpAndResetStream();
				Write(array, offset, count);
			}
		}
		#endregion
	}
}