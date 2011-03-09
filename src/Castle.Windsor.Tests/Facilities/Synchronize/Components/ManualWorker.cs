// Copyright 2004-2011 Castle Project - http://www.castleproject.org/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace CastleTests.Facilities.Synchronize.Components
{
#if !SILVERLIGHT
	using System;
	using System.Threading;

	using Castle.Facilities.Synchronize;

	public class ManualWorker : AsynchronousWorker, IWorkerWithOuts
	{
		private readonly ManualResetEvent ready = new ManualResetEvent(false);
		private Exception exception;

		public void Failed(Exception ex)
		{
			exception = ex;
			Ready();
		}

		public void Ready()
		{
			ready.Set();
		}

		[Synchronize]
		public override int DoWork(int work)
		{
			var remaining = base.DoWork(work);
			if (ready.WaitOne(5000, false))
			{
				if (exception != null)
				{
					throw exception;
				}
				return remaining;
			}
			return work*2;
		}
	}
#endif
}