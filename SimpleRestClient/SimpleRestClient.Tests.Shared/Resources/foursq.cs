﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRestClient.Tests
{
	public class VenuesResponse
	{
		public List<Group> Groups { get; set; }
	}

	public class Group
	{
		public string Name { get; set; }
	}
}
