using System;
using System.IO;

namespace DJ.Util.IO
{
	public interface IDirectoryVisitor
	{
		/// <summary>
		/// Is called before a directory is walked recursively.
		/// </summary>
		/// <param name="dirInfo">
		/// A <see cref="DirectoryInfo"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/> which indicates if a directory should be walked recursively (<code>true</code>)
		/// or not (<code>false</code>).
		/// </returns>
		bool PreVisit(DirectoryInfo dirInfo);
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="dirInfo">
		/// A <see cref="DirectoryInfo"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// </returns>
		bool PostVisit(DirectoryInfo dirInfo);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="fileInfo">
		/// A <see cref="FileInfo"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// </returns>
		bool Visit(FileInfo fileInfo);
		
	}
}
