﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;

namespace TorannMagic
{
	public class CompMagictrainer : CompUsable
	{
		protected override string FloatMenuOptionLabel
		{
			get
			{
				return string.Format("Magic Trainer");
			}
		}
	}
}
