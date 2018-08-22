﻿using Itm.DataValidation;

namespace Itm.Models
{
	public class UserModel
	{
		public UserModel()
		{
		}

		public int ID { get; set; }

		public string FirstName { get; set; }

		public string MiddleName { get; set; }

		public string LastName { get; set; }

		public string UserName { get; set; }

		public string Password { get; set; }
	}
}
