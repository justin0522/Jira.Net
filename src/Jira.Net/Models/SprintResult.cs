﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jira.Net.Models
{
	public class SprintResult
	{
		public List<Sprint> Sprints { get; set; }
		public Sprint Sprint { get; set; }
	}
}
