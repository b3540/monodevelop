﻿//
// DotNetCoreRuntimeVersions.cs
//
// Author:
//       Matt Ward <matt.ward@xamarin.com>
//
// Copyright (c) 2017 Xamarin Inc. (http://xamarin.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using MonoDevelop.Core;

namespace MonoDevelop.DotNetCore
{
	class DotNetCoreRuntimeVersions
	{
		public static IEnumerable<DotNetCoreVersion> GetInstalledVersions (DotNetCorePath dotNetCorePath)
		{
			if (dotNetCorePath.IsMissing)
				return Enumerable.Empty<DotNetCoreVersion> ();

			string runtimePath = GetDotNetCoreRuntimePath (dotNetCorePath);
			if (!Directory.Exists (runtimePath))
				return Enumerable.Empty<DotNetCoreVersion> ();

			return Directory.EnumerateDirectories (runtimePath)
				.Select (directory => GetDotNetCoreVersionFromDirectory (directory))
				.Where (version => version != null);
		}

		static string GetDotNetCoreRuntimePath (DotNetCorePath dotNetCorePath)
		{
			string rootDirectory = Path.GetDirectoryName (dotNetCorePath.FileName);
			return Path.Combine (rootDirectory, "shared", "Microsoft.NETCore.App");
		}

		static DotNetCoreVersion GetDotNetCoreVersionFromDirectory (string directory)
		{
			string directoryName = Path.GetFileName (directory);
			DotNetCoreVersion version = null;
			if (DotNetCoreVersion.TryParse (directoryName, out version))
				return version;

			LoggingService.LogInfo ("Unable to parse runtime version from directory. '{0}'", directory);
			return null;
		}
	}
}