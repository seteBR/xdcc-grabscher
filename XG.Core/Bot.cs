//  
//  Copyright (C) 2009 Lars Formella <ich@larsformella.de>
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace XG.Core
{
	[Serializable()]
	public class XGBot : XGObject
	{
		#region VARIABLES

		public new XGChannel Parent
		{
			get { return base.Parent as XGChannel; }
			set { base.Parent = value; }
		}

		[field: NonSerialized()]
		private XGPacket currentQueuedPacket = null;

		private BotState botState;
		public BotState BotState
		{
			get { return botState; }
			set
			{
				if (botState != value)
				{
					botState = value;
					this.Modified = true;
				}

				if(value == BotState.Waiting)
				{
					this.currentQueuedPacket = this.GetOldestActivePacket();
				}
				else
				{
					// this.currentQueuedPacket = null;
				}
			}
		}

		[field: NonSerialized()]
		private IPAddress ip = IPAddress.None;
		public IPAddress IP
		{
			get { return ip; }
			set
			{
				if (ip != value)
				{
					ip = value;
					this.Modified = true;
				}
			}
		}

		private string lastMessage = "";
		public string LastMessage
		{
			get { return lastMessage; }
			set
			{
				if (lastMessage != value)
				{
					lastMessage = value;
					this.lastContact = DateTime.Now;
					this.Modified = true;
				}
			}
		}

		private DateTime lastContact = new DateTime(1, 1, 1, 0, 0, 0, 0);
		public DateTime LastContact
		{
			get { return lastContact; }
			set
			{
				if (lastContact != value)
				{
					lastContact = value;
					this.Modified = true;
				}
			}
		}

		private int queuePosition = 0;
		public int QueuePosition
		{
			get { return queuePosition; }
			set
			{
				if (queuePosition != value)
				{
					queuePosition = value;
					this.Modified = true;
				}
			}
		}

		private int queueTime = 0;
		public int QueueTime
		{
			get { return queueTime; }
			set
			{
				if (queueTime != value)
				{
					queueTime = value;
					this.Modified = true;
				}
			}
		}

		private double infoSpeedMax = 0;
		public double InfoSpeedMax
		{
			get { return infoSpeedMax; }
			set
			{
				if (infoSpeedMax != value)
				{
					infoSpeedMax = value;
					this.Modified = true;
				}
			}
		}
		private double infoSpeedCurrent = 0;
		public double InfoSpeedCurrent
		{
			get { return infoSpeedCurrent; }
			set
			{
				if (infoSpeedCurrent != value)
				{
					infoSpeedCurrent = value;
					this.Modified = true;
				}
			}
		}

		private int infoSlotTotal = 0;
		public int InfoSlotTotal
		{
			get { return infoSlotTotal; }
			set
			{
				if (infoSlotTotal != value)
				{
					infoSlotTotal = value;
					this.Modified = true;
				}
			}
		}
		private int infoSlotCurrent = 0;
		public int InfoSlotCurrent
		{
			get { return infoSlotCurrent; }
			set
			{
				if (infoSlotCurrent != value)
				{
					infoSlotCurrent = value;
					this.Modified = true;
				}
			}
		}

		private int infoQueueTotal = 0;
		public int InfoQueueTotal
		{
			get { return infoQueueTotal; }
			set
			{
				if (infoQueueTotal != value)
				{
					infoQueueTotal = value;
					this.Modified = true;
				}
			}
		}
		private int infoQueueCurrent = 0;
		public int InfoQueueCurrent
		{
			get { return infoQueueCurrent; }
			set
			{
				if (infoQueueCurrent != value)
				{
					infoQueueCurrent = value;
					this.Modified = true;
				}
			}
		}

		#endregion

		#region CHILDREN

		public IEnumerable<XGPacket> Packets
		{
			get { return base.Children.Cast<XGPacket>(); }
		}

		public XGPacket this[int id]
		{
			get
			{
				try
				{
					return this.Packets.First(pack => pack.Id == id);
				}
				catch {}
				return null;
			}
		}

		public void AddPacket(XGPacket aPacket)
		{
			base.AddChild(aPacket);
		}
		
		public void RemovePacket(XGPacket aPacket)
		{
			base.RemoveChild(aPacket);
		}

		public XGPacket GetOldestActivePacket()
		{
			try
			{
				return this.Packets.OrderBy(packet => packet.LastModified).First(pack => pack.Enabled);
			}
			catch (InvalidOperationException)
			{
				return null;
			}
		}

		public XGPacket GetCurrentQueuedPacket()
		{
			return this.currentQueuedPacket;
		}

		#endregion
	}
}
