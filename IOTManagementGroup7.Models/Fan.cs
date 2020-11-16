﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Diagnostics;
using System.Dynamic;
using System.Text;

namespace IOTManagementGroup7.Models
{
   public class Fan
    {

		[Key]
		public int Id { get; set; }
		[Required]
		public string ApplicationUserId { get; set; }
		[ForeignKey("ApplicationUserId")]
		public ApplicationUser ApplicationUser { get; set; }
		
		public string Name { get; set; }
        public bool PowerStatus { get; set; }
        public bool ConnectionStatus { get; set; }
		public int Speed { get; set; }
		public string SourceCode { get; set; }
        /*Id NVARCHAR(450) PRIMARY KEY,
	UserId NVARCHAR(450),
	Name NVARCHAR(MAX),
	PowerStatus BIT,
	ConnectionStatus BIT,
	Speed INT,
	SourceCode NVARCHAR(MAX)
		*/
    }
}
